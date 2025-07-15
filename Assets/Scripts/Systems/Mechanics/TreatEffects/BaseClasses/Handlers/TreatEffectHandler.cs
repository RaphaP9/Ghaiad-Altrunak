using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreatEffectHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected TreatEffectSO treatEffectSO;

    [Header("Runtime Filled")]
    [SerializeField] protected bool isCurrentlyActiveByInventoryObjects;
    [SerializeField] protected bool isMeetingCondition;

    protected List<InventoryObjectSO> InventoryObjectsToActivate => treatEffectSO.activatorInventoryObjects;

    protected bool previouslyActiveByInventoryObjects = false;
    protected bool previouslyMeetingCondition = false;

    public static event EventHandler<OnTreatEffectEventArgs> OnTreatEffectActivatedByInventoryObjects;
    public static event EventHandler<OnTreatEffectEventArgs> OnTreatEffectDeactivatedByInventoryObjects;
    public static event EventHandler<OnTreatEffectEventArgs> OnTreatEffectEnablementByCondition;
    public static event EventHandler<OnTreatEffectEventArgs> OnTreatEffectDisablementByCondition;

    public class OnTreatEffectEventArgs : EventArgs
    {
        public TreatEffectSO treatConfigSO;
    }

    private void Awake()
    {
        SetSingleton();
    }

    protected virtual void Update()
    {
        HandleTreatActiveByInventoryObjects();
        HandleTreatEnablementByCondition();
    }

    #region Abstract/Virtual Methods
    protected abstract void SetSingleton();

    protected virtual void OnTreatEffectActivatedByInventoryObjectsMethod() //Will trigger as soon as activeByInventoryObjects
    {
        OnTreatEffectActivatedByInventoryObjects?.Invoke(this, new OnTreatEffectEventArgs { treatConfigSO = treatEffectSO });
    }

    protected virtual void OnTreatEffectDeactivatedByInventoryObjectsMethod()//Will trigger as soon as deactiveByInventoryObjects
    {
        OnTreatEffectDeactivatedByInventoryObjects?.Invoke(this, new OnTreatEffectEventArgs { treatConfigSO = treatEffectSO});
    }

    protected virtual void OnTreatEffectEnablementByConditionMethod() //Will be triggered as soon as meeting condition and  was activeByInventoryObject/ as soon as active by activeByInventoryObjects and was meeting condition
    {
        OnTreatEffectEnablementByCondition?.Invoke(this, new OnTreatEffectEventArgs { treatConfigSO = treatEffectSO });
    }

    protected virtual void OnTreatEffectDisablementByConditionMethod() //Will be triggered as soon as not meeting condition and was activeByInventoryObject / as soon as deactiveByInventoryObject and was meeting condition
    {
        OnTreatEffectDisablementByCondition?.Invoke(this, new OnTreatEffectEventArgs { treatConfigSO = treatEffectSO });
    }

    protected virtual bool EnablementCondition() => true; //As default the EnablementCondition will always be met, override in inheritances otherwise
    #endregion

    #region Logic Of Activation By InvObjects / Enablement By Condition
    private bool IsActiveByInventoryObjects()
    {
        foreach(InventoryObjectSO inventoryObjectSO in treatEffectSO.activatorInventoryObjects)
        {
            switch (inventoryObjectSO.GetInventoryObjectType())
            {
                case InventoryObjectType.Object:
                    if (ObjectsInventoryManager.Instance.HasObjectSOInInventory(inventoryObjectSO as ObjectSO)) return true;
                    break;
                case InventoryObjectType.Treat:
                    if (TreatsInventoryManager.Instance.HasTreatSOInInventory(inventoryObjectSO as TreatSO)) return true;
                    break;
            }
        }

        return false;
    }

    private void HandleTreatActiveByInventoryObjects()
    {
        bool currentlyActiveByInventoryObjects = IsActiveByInventoryObjects();

        if(!previouslyActiveByInventoryObjects && currentlyActiveByInventoryObjects)
        {
            OnTreatEffectActivatedByInventoryObjectsMethod();
            if (isMeetingCondition) OnTreatEffectEnablementByConditionMethod();
        }
        else if(previouslyActiveByInventoryObjects && !currentlyActiveByInventoryObjects)
        {
            if (isMeetingCondition) OnTreatEffectDisablementByConditionMethod();
            OnTreatEffectDeactivatedByInventoryObjectsMethod();
        }

        isCurrentlyActiveByInventoryObjects = currentlyActiveByInventoryObjects;
        previouslyActiveByInventoryObjects = isCurrentlyActiveByInventoryObjects;       
    }

    private void HandleTreatEnablementByCondition()
    {
        bool meetingCondition = EnablementCondition();

        if (!previouslyMeetingCondition && meetingCondition)
        {
            if(isCurrentlyActiveByInventoryObjects) OnTreatEffectEnablementByConditionMethod();
        }
        else if (previouslyMeetingCondition && !meetingCondition)
        {
            if (isCurrentlyActiveByInventoryObjects) OnTreatEffectDisablementByConditionMethod();
        }

        isMeetingCondition = meetingCondition;
        previouslyMeetingCondition = isMeetingCondition;
    }
    #endregion
}
