using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIgnoreHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<CollisionIgnoreGroup> collisionIgnoreGroups;

    [System.Serializable]
    public class CollisionIgnoreGroup
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
        foreach(CollisionIgnoreGroup collisionIgnoreGroup in collisionIgnoreGroups)
        {
            if (!collisionIgnoreGroup.enabled) continue;
            Physics2D.IgnoreLayerCollision(collisionIgnoreGroup.colliderLayerIndex, collisionIgnoreGroup.collideeLayerIndex);
        }
    }
}
