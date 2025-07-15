using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatShopInventorySingleUI : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private TreatIdentified treatIdentified;

    public TreatIdentified TreatIdentified => treatIdentified;

    public event EventHandler<OnTreatInventorySetEventArgs> OnTreatInventorySet;
    public class OnTreatInventorySetEventArgs : EventArgs
    {
        public TreatIdentified treatIdentified;
    }

    public void SetLinkedTreat(TreatIdentified treatIdentified)
    {
        this.treatIdentified = treatIdentified;
        OnTreatInventorySet?.Invoke(this, new OnTreatInventorySetEventArgs { treatIdentified = treatIdentified });
    }
}
