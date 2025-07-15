using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIdentifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EntitySO entitySO;

    public EntitySO EntitySO => entitySO;
}
