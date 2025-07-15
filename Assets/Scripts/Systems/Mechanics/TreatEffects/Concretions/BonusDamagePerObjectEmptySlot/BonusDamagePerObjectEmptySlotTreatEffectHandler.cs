using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDamagePerObjectEmptySlotTreatEffectHandler : StatusStackingTreatEffectHandler
{
    public static BonusDamagePerObjectEmptySlotTreatEffectHandler Instance { get; private set; }

    private BonusDamagePerObjectEmptySlotTreatEffectSO BonusDamagePerObjectEmptySlotTreatEffectSO => treatEffectSO as BonusDamagePerObjectEmptySlotTreatEffectSO;

    private void OnEnable()
    {
        ObjectsInventoryManager.OnObjectAddedToInventory += ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory += ObjectsInventoryManager_OnObjectRemovedFromInventory;
    }

    private void OnDisable()
    {
        ObjectsInventoryManager.OnObjectAddedToInventory -= ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory -= ObjectsInventoryManager_OnObjectRemovedFromInventory;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void SetProportionalStatForStacksByStatus()
    {
        base.SetProportionalStatForStacksByStatus();
        AddProportionalStatForStacks(BonusDamagePerObjectEmptySlotTreatEffectSO.statPerStack);
    }

    protected override string GetRefferencialGUID() => BonusDamagePerObjectEmptySlotTreatEffectSO.refferencialGUID;

    protected override int GetStacksByStatus()
    {
        int stacks = ObjectsInventoryManager.Instance.GetEmptySlots() < 0 ? 0 : ObjectsInventoryManager.Instance.GetEmptySlots();
        return stacks;
    }

    #region Subscriptions
    private void ObjectsInventoryManager_OnObjectAddedToInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;

        SetProportionalStatForStacksByStatus();
    }

    private void ObjectsInventoryManager_OnObjectRemovedFromInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;

        SetProportionalStatForStacksByStatus();
    }
    #endregion
}
