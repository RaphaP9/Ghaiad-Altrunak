using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    private const string START_DESPAWN_TRIGGER = "StartDespawn";
    private const string STOP_DESPAWN_TRIGGER = "StopDespawn";
}
