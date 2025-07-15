using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFacingDirectionApplicator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityFacingDirectionHandler entityFacingDirectionHandler;

    private void Update()
    {
        UpdateRotation(entityFacingDirectionHandler.RawFacingAngle);
    }

    private void UpdateRotation(float aimAngle) => transform.rotation = Quaternion.Euler(0, 0, aimAngle);
}
