using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAimDirectionApplicator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityAimDirectionHandler entityAimDirectionHandler;

    private void Update()
    {
        UpdateRotation(entityAimDirectionHandler.AimAngle);
    }

    private void UpdateRotation(float aimAngle) => transform.rotation = Quaternion.Euler(0, 0, aimAngle);
}
