using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoldCollection : MonoBehaviour
{
    [Header("Value Settings")]
    [SerializeField] private int value;

    [Header("Settings")]
    [SerializeField] private LayerMask playerLayerMask;

    public static event EventHandler<OnGoldEventArgs> OnAnyGoldCollected;
    public event EventHandler<OnGoldEventArgs> OnGoldCollected;

    public class OnGoldEventArgs : EventArgs
    {
        public int value;
        public Vector2 position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, playerLayerMask)) return;

        OnGoldCollected?.Invoke(this, new OnGoldEventArgs { value = value, position = GeneralUtilities.Vector3ToVector2(transform.position)});
        OnAnyGoldCollected?.Invoke(this, new OnGoldEventArgs { value = value, position = GeneralUtilities.Vector3ToVector2(transform.position)});

        Destroy(gameObject);
    }
}
