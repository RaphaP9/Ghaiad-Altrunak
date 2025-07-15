using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenMeleeEnemyAnimationController : MeleeEnemyAnimationController
{
    [Header("Regeneration Components")]
    [SerializeField] private MeleeEnemyRegeneration meleeEnemyRegeneration;

    protected const string REGEN_MOVEMENT_BLEND_TREE_NAME = "RegenMovementBlendTree";

    protected override void OnEnable()
    {
        base.OnEnable();
        meleeEnemyRegeneration.OnEnemyRegenerationStart += MeleeEnemyRegeneration_OnEnemyRegenerationStart;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        meleeEnemyRegeneration.OnEnemyRegenerationStart -= MeleeEnemyRegeneration_OnEnemyRegenerationStart;
    }

    #region Subcriptions
    private void MeleeEnemyRegeneration_OnEnemyRegenerationStart(object sender, System.EventArgs e)
    {
        PlayAnimation(REGEN_MOVEMENT_BLEND_TREE_NAME);
    }
    #endregion
}
