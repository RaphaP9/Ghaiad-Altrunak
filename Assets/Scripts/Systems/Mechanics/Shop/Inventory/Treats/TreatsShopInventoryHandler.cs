using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatsShopInventoryHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform treatsShopInventoryContainer;
    [SerializeField] private Transform treatUIPrefab;
    [SerializeField] private Transform emptySlotPrefab;

    [Header("Debugg")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        TreatsInventoryManager.OnTreatsInventoryInitialized += TreatsInventoryManager_OnTreatsInventoryInitialized;
        TreatsInventoryManager.OnTreatAddedToInventory += TreatsInventoryManager_OnTreatAddedToInventory;
        TreatsInventoryManager.OnTreatRemovedFromInventory += TreatsInventoryManager_OnTreatRemovedFromInventory;
    }

    private void OnDisable()
    {
        TreatsInventoryManager.OnTreatsInventoryInitialized -= TreatsInventoryManager_OnTreatsInventoryInitialized;
        TreatsInventoryManager.OnTreatAddedToInventory -= TreatsInventoryManager_OnTreatAddedToInventory;
        TreatsInventoryManager.OnTreatRemovedFromInventory -= TreatsInventoryManager_OnTreatRemovedFromInventory;
    }

    private void UpdateUI()
    {
        ClearContainer();

        int emptySlots = TreatsInventoryManager.Instance.GetInventoryCapacity();

        foreach (TreatIdentified treatIdentified in TreatsInventoryManager.Instance.TreatsInventory)
        {
            CreateTreatUI(treatIdentified);
            emptySlots--;
        }

        for (int i = 0; i < emptySlots; i++)
        {
            CreateEmptySlot();
        }
    }


    private void ClearContainer()
    {
        foreach (Transform child in treatsShopInventoryContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateTreatUI(TreatIdentified treatIdentified)
    {
        Transform treatUITransform = Instantiate(treatUIPrefab, treatsShopInventoryContainer);

        TreatShopInventorySingleUI treatInventoryUI = treatUITransform.GetComponent<TreatShopInventorySingleUI>();

        if (treatInventoryUI == null)
        {
            if (debug) Debug.Log("Instantiated prefab does not contain an TreatShopInventorySingleUI component. Set will be ignored");
            return;
        }

        treatInventoryUI.SetLinkedTreat(treatIdentified);
    }

    private void CreateEmptySlot()
    {
        Transform emptySlotUI = Instantiate(emptySlotPrefab, treatsShopInventoryContainer);
    }


    private void TreatsInventoryManager_OnTreatsInventoryInitialized(object sender, TreatsInventoryManager.OnTreatsEventArgs e)
    {
        UpdateUI();
    }

    private void TreatsInventoryManager_OnTreatAddedToInventory(object sender, TreatsInventoryManager.OnTreatEventArgs e)
    {
        UpdateUI();
    }

    private void TreatsInventoryManager_OnTreatRemovedFromInventory(object sender, TreatsInventoryManager.OnTreatEventArgs e)
    {
        UpdateUI();
    }
}
