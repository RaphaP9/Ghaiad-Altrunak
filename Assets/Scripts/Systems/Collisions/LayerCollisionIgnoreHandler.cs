using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCollisionIgnoreHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<LayerCollisionIgnoreGroup> layerCollisionIgnoreGroups;

    [System.Serializable]
    public class LayerCollisionIgnoreGroup
    {
        public int colliderLayerIndex;
        public int collideeLayerIndex;
        [Space]
        public bool enabled;
    }

    private void Awake()
    {
        IgnoreCollisions();
    }

    private void IgnoreCollisions()
    {
        foreach(LayerCollisionIgnoreGroup collisionIgnoreGroup in layerCollisionIgnoreGroups)
        {
            if (!collisionIgnoreGroup.enabled) continue;
            Physics2D.IgnoreLayerCollision(collisionIgnoreGroup.colliderLayerIndex, collisionIgnoreGroup.collideeLayerIndex);
        }
    }
}
