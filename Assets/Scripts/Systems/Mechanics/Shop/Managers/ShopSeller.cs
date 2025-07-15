using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSeller : MonoBehaviour
{
    public static ShopSeller Instance { get; private set; }

    public static event EventHandler<OnObjectSoldEventArgs> OnObjectSold;
    public static event EventHandler<OnTreatSoldEventArgs> OnTreatSold;

    public class OnObjectSoldEventArgs : EventArgs
    {
        public ObjectIdentified objectIdentified;
    }

    public class OnTreatSoldEventArgs : EventArgs
    {
        public TreatIdentified treatIdentified;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
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

    public void SellObject(ObjectIdentified objectIdentified)
    {
        GoldManager.Instance.AddGold(objectIdentified.objectSO.sellPrice);
        ObjectsInventoryManager.Instance.RemoveObjectFromInventoryByGUID(objectIdentified.assignedGUID);

        OnObjectSold?.Invoke(this, new OnObjectSoldEventArgs { objectIdentified = objectIdentified });
    }

    public void SellTreat(TreatIdentified treatIdentified)
    {
        GoldManager.Instance.AddGold(treatIdentified.treatSO.sellPrice);
        TreatsInventoryManager.Instance.RemoveTreatFromInventoryByGUID(treatIdentified.assignedGUID);

        OnTreatSold?.Invoke(this, new OnTreatSoldEventArgs { treatIdentified = treatIdentified });
    }
}
