using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GoldDespawnHandler goldDespawnHandler;

    private const string START_DESPAWN_TRIGGER = "StartDespawn";
    private const string STOP_DESPAWN_TRIGGER = "StopDespawn";

    private void OnEnable()
    {
        goldDespawnHandler.OnGoldStartDespawning += GoldDespawnHandler_OnGoldStartDespawning;
        goldDespawnHandler.OnGoldCancelDespawning += GoldDespawnHandler_OnGoldCancelDespawning;
    }

    private void OnDisable()
    {
        goldDespawnHandler.OnGoldStartDespawning -= GoldDespawnHandler_OnGoldStartDespawning;
        goldDespawnHandler.OnGoldCancelDespawning -= GoldDespawnHandler_OnGoldCancelDespawning;
    }
    private void GoldDespawnHandler_OnGoldStartDespawning(object sender, System.EventArgs e)
    {
        animator.SetTrigger(START_DESPAWN_TRIGGER);
    }

    private void GoldDespawnHandler_OnGoldCancelDespawning(object sender, System.EventArgs e)
    {
        animator.SetTrigger(STOP_DESPAWN_TRIGGER);
    }
}
