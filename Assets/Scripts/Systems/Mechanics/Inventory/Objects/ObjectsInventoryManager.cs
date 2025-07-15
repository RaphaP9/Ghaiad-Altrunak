using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInventoryManager : MonoBehaviour
{
    public static ObjectsInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<ObjectIdentified> objectsInventory;

    [Header("Character Dependant - RuntimeFilled")]
    [SerializeField] private int objectsInventorySize;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<ObjectIdentified> ObjectsInventory => objectsInventory;
    public int ObjectsInventorySize => objectsInventorySize;

    public static event EventHandler<OnObjectsEventArgs> OnObjectsInventoryInitialized;
    public static event EventHandler<OnObjectEventArgs> OnObjectAddedToInventory;
    public static event EventHandler<OnObjectEventArgs> OnObjectRemovedFromInventory;

    public static event EventHandler<OnObjectInventorySizeEventArgs> OnObjectInventorySizeSet;

    public class OnObjectEventArgs : EventArgs
    {
        public ObjectIdentified objectIdentified;
    }
    public class OnObjectsEventArgs : EventArgs
    {
        public List<ObjectIdentified> objects;
    }

    public class OnObjectInventorySizeEventArgs : EventArgs
    {
        public int objectInventorySize;
    }

    private void OnEnable()
    {
        PlayerCharacterManager.OnPlayerCharacterSetPreInstantiation += PlayerCharacterManager_OnPlayerCharacterSetPreInstantiation;
    }

    private void OnDisable()
    {
        PlayerCharacterManager.OnPlayerCharacterSetPreInstantiation -= PlayerCharacterManager_OnPlayerCharacterSetPreInstantiation;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeObjectsInventory();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ObjectsInventoryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeObjectsInventory()
    {
        OnObjectsInventoryInitialized?.Invoke(this, new OnObjectsEventArgs { objects = objectsInventory }); 
    }

    #region Add Objects
    public void AddObjectToInventory(ObjectSO objectSO)
    {
        if (objectSO == null)
        {
            if (debug) Debug.Log("ObjectSO is null, addition will be ignored");
            return;
        }

        string objectGUID = GeneralUtilities.GenerateGUID();

        ObjectIdentified objectToAdd = new ObjectIdentified { assignedGUID = objectGUID, objectSO = objectSO };

        objectsInventory.Add(objectToAdd);

        OnObjectAddedToInventory?.Invoke(this, new OnObjectEventArgs { objectIdentified = objectToAdd });
    }
    #endregion

    #region Remove Objects
    public void RemoveObjectFromInventory(ObjectSO objectSO)
    {
        if (objectSO == null)
        {
            if (debug) Debug.Log("ObjectSO is null, remotion will be ignored");
            return;
        }

        ObjectIdentified objectToRemove = FindObjectByObjectSO(objectSO);

        if (objectToRemove == null)
        {
            if (debug) Debug.Log("Could not find object by ObjectSO");
            return;
        }

        objectsInventory.Remove(objectToRemove);

        OnObjectRemovedFromInventory?.Invoke(this, new OnObjectEventArgs { objectIdentified = objectToRemove });
    }

    public void RemoveObjectFromInventoryByGUID(string GUID)
    {
        ObjectIdentified objectToRemove = FindObjectByGUID(GUID);

        if (objectToRemove == null)
        {
            if (debug) Debug.Log("Could not find object by GUID");
            return;
        }

        objectsInventory.Remove(objectToRemove);

        OnObjectRemovedFromInventory?.Invoke(this, new OnObjectEventArgs { objectIdentified = objectToRemove });
    }
    #endregion

    #region Find Objects
    private ObjectIdentified FindObjectByObjectSO(ObjectSO objectSO)
    {
        foreach (ObjectIdentified @object in objectsInventory)
        {
            if (@object.objectSO == objectSO) return @object;
        }

        if (debug) Debug.Log($"ObjectSO with name: {objectSO._name} could not be found. Proceding to return null");
        return null;
    }

    private ObjectIdentified FindObjectByGUID(string GUID)
    {
        foreach (ObjectIdentified @object in objectsInventory)
        {
            if (@object.assignedGUID == GUID) return @object;
        }

        if (debug) Debug.Log($"Object with GUID {GUID} could not be found. Proceding to return null");
        return null;
    }
    #endregion

    public bool HasObjectSOInInventory(ObjectSO objectSO)
    {
        foreach(ObjectIdentified @object in objectsInventory)
        {
            if (@object.objectSO == objectSO) return true;
        }

        return false;
    }

    public void SetObjectsInventory(List<ObjectIdentified> setterObjectsInventory) => objectsInventory.AddRange(setterObjectsInventory); //Add, not Replace!

    public void SetObjectsInventorySize(int size)
    {
        objectsInventorySize = size;
        OnObjectInventorySizeSet?.Invoke(this, new OnObjectInventorySizeEventArgs { objectInventorySize = size });
    }

    public int GetInventoryCapacity() => objectsInventorySize;
    public int GetObjectsInInventory() => objectsInventory.Count;
    public int GetEmptySlots() => objectsInventorySize - objectsInventory.Count;
    public bool IsInventoryFull() => objectsInventory.Count >= objectsInventorySize;

    #region Subscriptions
    private void PlayerCharacterManager_OnPlayerCharacterSetPreInstantiation(object sender, PlayerCharacterManager.OnPlayerCharacterEventArgs e)
    {
        SetObjectsInventorySize(e.characterSO.objectsInventorySize);
    }
    #endregion
}