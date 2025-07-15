using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMeleeEnemyAnimationController : MeleeEnemyAnimationController
{
    [Header("Ghosting Components")]
    [SerializeField] private MeleeEnemyGhosting meleeEnemyGhosting;

    protected const string GHOST_MOVEMENT_BLEND_TREE_NAME = "GhostMovementBlendTree";

    protected override void OnEnable()
    {
        base.OnEnable();
        meleeEnemyGhosting.OnEnemyGhosting += MeleeEnemyGhosting_OnEnemyGhosting;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        meleeEnemyGhosting.OnEnemyGhosting -= MeleeEnemyGhosting_OnEnemyGhosting;
    }

    #region Subcriptions
    private void MeleeEnemyGhosting_OnEnemyGhosting(object sender, System.EventArgs e)
    {
        PlayAnimation(GHOST_MOVEMENT_BLEND_TREE_NAME);
    }
    #endregion
}
