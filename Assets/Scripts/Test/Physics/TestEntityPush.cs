using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntityPush : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float pushForce;
    [SerializeField] private float actionRadius;
    [SerializeField] private LayerMask pushLayerMask;
    [Space]
    [SerializeField] private Transform playerTransform;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PhysicPushData pushData = new PhysicPushData(pushForce, null);
            MechanicsUtilities.PushAllEntitiesFromPoint(GeneralUtilities.TransformPositionVector2(playerTransform), pushData, pushLayerMask, actionRadius, new List<Transform> { playerTransform});
        }
    }
}
