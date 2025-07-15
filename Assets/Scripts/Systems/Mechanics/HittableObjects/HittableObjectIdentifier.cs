using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObjectIdentifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private HittableObjectSO hittableObjectSO;

    public HittableObjectSO HittableObjectSO => hittableObjectSO;
}
