using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsShopInventoryHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform objectsShopInventoryContainer;
    [SerializeField] private Transform objectUIPrefab;
    [SerializeField] private Transform emptySlotPrefab;

    [Header("Debugg")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        ObjectsInventoryManager.OnObjectsInventoryInitialized += ObjectsInventoryManager_OnObjectsInventoryInitialized;
        ObjectsInventoryManager.OnObjectAddedToInventory += ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory += ObjectsInventoryManager_OnObjectRemovedFromInventory;
    }

    private void OnDisable()
    {
        ObjectsInventoryManager.OnObjectsInventoryInitialized -= ObjectsInventoryManager_OnObjectsInventoryInitialized;
        ObjectsInventoryManager.OnObjectAddedToInventory -= ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory -= ObjectsInventoryManager_OnObjectRemovedFromInventory;
    }

    private void UpdateUI()
    {
        ClearContainer();

        int emptySlots = ObjectsInventoryManager.Instance.GetInventoryCapacity();

        foreach (ObjectIdentified objectIdentified in ObjectsInventoryManager.Instance.ObjectsInventory)
        {
            CreateObjectUI(objectIdentified);
            emptySlots--;
        }

        for (int i = 0; i < emptySlots; i++)
        {
            CreateEmptySlot();
        }
    }


    private void ClearContainer()
    {
        foreach(Transform child in objectsShopInventoryContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateObjectUI(ObjectIdentified objectIdentified)
    {
        Transform objectUITransform = Instantiate(objectUIPrefab, objectsShopInventoryContainer);

        ObjectShopInventorySingleUI objectInventoryUI = objectUITransform.GetComponent<ObjectShopInventorySingleUI>();

        if(objectInventoryUI == null)
        {
            if (debug) Debug.Log("Instantiated prefab does not contain an ObjectShopInventorySingleUI component. Set will be ignored");
            return;
        }

        objectInventoryUI.SetLinkedObject(objectIdentified);
    }

    private void CreateEmptySlot()
    {
        Transform emptySlotUI = Instantiate(emptySlotPrefab, objectsShopInventoryContainer);
    }

    private void ObjectsInventoryManager_OnObjectsInventoryInitialized(object sender, ObjectsInventoryManager.OnObjectsEventArgs e)
    {
        UpdateUI();
    }

    private void ObjectsInventoryManager_OnObjectAddedToInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        UpdateUI();
    }

    private void ObjectsInventoryManager_OnObjectRemovedFromInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        UpdateUI();
    }
}
