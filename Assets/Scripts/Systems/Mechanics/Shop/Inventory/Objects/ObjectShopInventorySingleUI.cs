using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShopInventorySingleUI : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private ObjectIdentified objectIdentified;

    public ObjectIdentified ObjectIdentified => objectIdentified;

    public event EventHandler<OnObjectInventorySetEventArgs> OnObjectInventorySet;

    public class OnObjectInventorySetEventArgs : EventArgs
    {
        public ObjectIdentified objectIdentified;
    }

    public void SetLinkedObject(ObjectIdentified objectIdentified)
    {
        this.objectIdentified = objectIdentified;
        OnObjectInventorySet?.Invoke(this, new OnObjectInventorySetEventArgs { objectIdentified = objectIdentified });
    }
}
