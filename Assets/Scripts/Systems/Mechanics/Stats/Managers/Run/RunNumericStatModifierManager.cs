using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunNumericStatModifierManager : NumericStatModifierManager
{
    //RunNumericStatModifierManager will register/unregister stats from objects added to/removed from inventory
    public static RunNumericStatModifierManager Instance { get; private set; }

    protected virtual void OnEnable()
    {
        ObjectsInventoryManager.OnObjectAddedToInventory += ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory += ObjectsInventoryManager_OnObjectRemovedFromInventory;

        TreatsInventoryManager.OnTreatAddedToInventory += TreatsInventoryManager_OnTreatAddedToInventory;
        TreatsInventoryManager.OnTreatRemovedFromInventory += TreatsInventoryManager_OnTreatRemovedFromInventory;
    }

    protected virtual void OnDisable()
    {
        ObjectsInventoryManager.OnObjectAddedToInventory -= ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory -= ObjectsInventoryManager_OnObjectRemovedFromInventory;

        TreatsInventoryManager.OnTreatAddedToInventory -= TreatsInventoryManager_OnTreatAddedToInventory;
        TreatsInventoryManager.OnTreatRemovedFromInventory -= TreatsInventoryManager_OnTreatRemovedFromInventory;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one RunNumericStatModifierManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Subscriptions
    private void ObjectsInventoryManager_OnObjectAddedToInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        AddStatModifiers(e.objectIdentified.assignedGUID, e.objectIdentified.objectSO);
    }

    private void ObjectsInventoryManager_OnObjectRemovedFromInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        RemoveStatModifiersByGUID(e.objectIdentified.assignedGUID);
    }

    private void TreatsInventoryManager_OnTreatAddedToInventory(object sender, TreatsInventoryManager.OnTreatEventArgs e)
    {
        AddStatModifiers(e.treatIdentified.assignedGUID, e.treatIdentified.treatSO);
    }

    private void TreatsInventoryManager_OnTreatRemovedFromInventory(object sender, TreatsInventoryManager.OnTreatEventArgs e)
    {
        RemoveStatModifiersByGUID(e.treatIdentified.assignedGUID);
    }
    #endregion
}
