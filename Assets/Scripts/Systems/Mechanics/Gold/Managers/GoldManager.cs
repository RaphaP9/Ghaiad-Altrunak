using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance {  get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private int currentGold;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public int CurrentGold => currentGold;

    public static event EventHandler<OnGoldEventArgs> OnGoldInitialized;

    public static event EventHandler<OnGoldChangedEventArgs> OnGoldAdded;
    public static event EventHandler<OnGoldChangedEventArgs> OnGoldSpent;

    public static event EventHandler<OnGoldSpentDeniedEventArgs> OnGoldSpentDenied;

    public static event EventHandler<OnTangibleGoldEventArgs> OnProcessedGoldCollected;

    public class OnGoldEventArgs : EventArgs
    {
        public int currentGold;
    }

    public class OnGoldChangedEventArgs : EventArgs
    {
        public int previousGold;
        public int newGold;
    }

    public class OnGoldSpentDeniedEventArgs : EventArgs
    {
        public int goldToSpend;
        public int currentGold;
    }

    public class OnTangibleGoldEventArgs : EventArgs
    {
        public int goldAmount;
        public Vector2 position;
    }

    private void OnEnable()
    {
        GoldCollection.OnAnyGoldCollected += GoldCollection_OnAnyGoldCollected;
    }

    private void OnDisable()
    {
        GoldCollection.OnAnyGoldCollected -= GoldCollection_OnAnyGoldCollected;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeGold();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GoldResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeGold()
    {
        currentGold = currentGold < 0 ? 0 : currentGold;

        OnGoldInitialized?.Invoke(this, new OnGoldEventArgs { currentGold = currentGold });
    }

    public int AddGold(int goldToAdd) //Returns the real gold amount added
    {
        if (goldToAdd <= 0) return 0;

        int realGoldAdded = GoldResolver.Instance.ResolveStatInt(goldToAdd); //Increase or change gold according to buffs

        if (realGoldAdded <= 0) return 0;

        int previousGold = currentGold;
        currentGold += realGoldAdded;

        OnGoldAdded?.Invoke(this, new OnGoldChangedEventArgs { previousGold = previousGold, newGold = currentGold });

        return realGoldAdded;
    }

    public void AddGoldRaw(int goldToAdd) //Returns the real gold amount added
    {
        if (goldToAdd <= 0) return;

        int previousGold = currentGold;
        currentGold += goldToAdd;

        OnGoldAdded?.Invoke(this, new OnGoldChangedEventArgs { previousGold = previousGold, newGold = currentGold });
    }

    public int SpendGold(int goldToSpend) //Returns the real gold amount spent
    {
        if (goldToSpend <= 0) return 0;

        if (!CanSpendGold(goldToSpend))
        {
            OnGoldSpentDenied?.Invoke(this, new OnGoldSpentDeniedEventArgs { currentGold = currentGold, goldToSpend =  goldToSpend}); 

            if (debug) Debug.Log("Not enough gold to spend!");
            return 0;
        }

        int previousGold = currentGold;
        currentGold -= goldToSpend;

        OnGoldSpent?.Invoke(this, new OnGoldChangedEventArgs { previousGold = previousGold, newGold = currentGold });

        return goldToSpend;
    }

    public bool CanSpendGold(int goldToSpend) => currentGold >= goldToSpend;
    public void SetCurrentGold(int setterGold) => currentGold = setterGold;

    private void ProcessGoldCollected(int value, Vector2 position)
    {
        int processedGold = AddGold(value);
        OnProcessedGoldCollected?.Invoke(this, new OnTangibleGoldEventArgs { goldAmount = processedGold, position = position });
    }

    #region Subscriptions
    private void GoldCollection_OnAnyGoldCollected(object sender, GoldCollection.OnGoldEventArgs e)
    {
        ProcessGoldCollected(e.value, e.position);   
    }
    #endregion
}
