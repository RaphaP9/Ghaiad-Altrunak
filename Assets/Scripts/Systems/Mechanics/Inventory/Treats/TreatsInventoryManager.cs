using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatsInventoryManager : MonoBehaviour
{
    public static TreatsInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<TreatIdentified> treatsInventory;

    [Header("Character Dependant - RuntimeFilled")]
    [SerializeField] private int treatsInventorySize;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<TreatIdentified> TreatsInventory => treatsInventory;
    public int TeamsInventorySize => treatsInventorySize;

    public static event EventHandler<OnTreatsEventArgs> OnTreatsInventoryInitialized;
    public static event EventHandler<OnTreatEventArgs> OnTreatAddedToInventory;
    public static event EventHandler<OnTreatEventArgs> OnTreatRemovedFromInventory;

    public static event EventHandler<OnTreatsInventorySizeEventArgs> OnTreatsInventorySizeSet;

    public class OnTreatEventArgs : EventArgs
    {
        public TreatIdentified treatIdentified;
    }
    public class OnTreatsEventArgs : EventArgs
    {
        public List<TreatIdentified> treats;
    }

    public class OnTreatsInventorySizeEventArgs : EventArgs
    {
        public int treatsInventorySize;
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
        InitializeTreatsInventory();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one TreatsInventoryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeTreatsInventory()
    {
        OnTreatsInventoryInitialized?.Invoke(this, new OnTreatsEventArgs { treats = treatsInventory });
    }

    #region Add Treats
    public void AddTreatToInventory(TreatSO treatSO)
    {
        if (treatSO == null)
        {
            if (debug) Debug.Log("TreatSO is null, addition will be ignored");
            return;
        }

        string treatGUID = GeneralUtilities.GenerateGUID();

        TreatIdentified treatToAdd = new TreatIdentified { assignedGUID = treatGUID, treatSO = treatSO };

        treatsInventory.Add(treatToAdd);

        OnTreatAddedToInventory?.Invoke(this, new OnTreatEventArgs { treatIdentified = treatToAdd });
    }
    #endregion

    #region Remove Treats
    public void RemoveTreatsFromInventory(TreatSO treatSO)
    {
        if (treatSO == null)
        {
            if (debug) Debug.Log("TreatSO is null, remotion will be ignored");
            return;
        }

        TreatIdentified treatToRemove = FindTreatByTreatSO(treatSO);

        if (treatToRemove == null)
        {
            if (debug) Debug.Log("Could not find treat by TreatSO");
            return;
        }

        treatsInventory.Remove(treatToRemove);

        OnTreatRemovedFromInventory?.Invoke(this, new OnTreatEventArgs { treatIdentified = treatToRemove });
    }

    public void RemoveTreatFromInventoryByGUID(string GUID)
    {
        TreatIdentified treatToRemove = FindTreatByGUID(GUID);

        if (treatToRemove == null)
        {
            if (debug) Debug.Log("Could not find treat by GUID");
            return;
        }

        treatsInventory.Remove(treatToRemove);

        OnTreatRemovedFromInventory?.Invoke(this, new OnTreatEventArgs { treatIdentified = treatToRemove });
    }
    #endregion

    #region Find Treats
    private TreatIdentified FindTreatByTreatSO(TreatSO treatSO)
    {
        foreach (TreatIdentified treat in treatsInventory)
        {
            if (treat.treatSO == treatSO) return treat;
        }

        if (debug) Debug.Log($"TreatSO with name: {treatSO._name} could not be found. Proceding to return null");
        return null;
    }

    private TreatIdentified FindTreatByGUID(string GUID)
    {
        foreach (TreatIdentified treat in treatsInventory)
        {
            if (treat.assignedGUID == GUID) return treat;
        }

        if (debug) Debug.Log($"Treat with GUID {GUID} could not be found. Proceding to return null");
        return null;
    }
    #endregion

    public bool HasTreatSOInInventory(TreatSO treatSO)
    {
        foreach (TreatIdentified treat in treatsInventory)
        {
            if (treat.treatSO == treatSO) return true;
        }

        return false;
    }

    public void SetTreatsInventory(List<TreatIdentified> setterTreatsInventory) => treatsInventory.AddRange(setterTreatsInventory); //Add, not Replace!

    public void SetTreatsInventorySize(int size)
    {
        treatsInventorySize = size;
        OnTreatsInventorySizeSet?.Invoke(this, new OnTreatsInventorySizeEventArgs { treatsInventorySize = size });
    }

    public int GetInventoryCapacity() => treatsInventorySize;
    public int GetTreatsInInventory() => treatsInventory.Count;
    public int GetEmptySlots() => treatsInventorySize - treatsInventory.Count;
    public bool IsInventoryFull() => treatsInventory.Count >= treatsInventorySize;

    #region Subscriptions
    private void PlayerCharacterManager_OnPlayerCharacterSetPreInstantiation(object sender, PlayerCharacterManager.OnPlayerCharacterEventArgs e)
    {
        SetTreatsInventorySize(e.characterSO.treatsInventorySize);
    }
    #endregion
}
