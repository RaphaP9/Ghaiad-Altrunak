using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBodyAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private EntityFacingDirectionHandler facingDirectionHandler;
    [Space]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;

    protected const string SPEED_FLOAT = "Speed";
    protected const string FACE_X_FLOAT = "FaceX";
    protected const string FACE_Y_FLOAT = "FaceY";

    protected const string SPAWN_ANIMATION_NAME = "Spawn";
    protected const string MOVEMENT_BLEND_TREE_NAME = "MovementBlendTree";
    protected const string DEATH_ANIMATION_NAME = "Death";

    protected const string TAKE_DAMAGE_ANIMATION_NAME = "TakeDamage";

    protected bool isDead = false;

    protected virtual void OnEnable()
    {
        playerHealth.OnPlayerHealthTakeDamage += PlayerHealth_OnPlayerHealthTakeDamage;
        playerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
    }

    protected virtual void OnDisable()
    {
        playerHealth.OnPlayerHealthTakeDamage -= PlayerHealth_OnPlayerHealthTakeDamage;
        playerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
    }

    protected virtual void Update()
    {
        HandleSpeedBlend();
        HandleFacingBlend();
    }

    private void HandleSpeedBlend()
    {
        animator.SetFloat(SPEED_FLOAT, playerMovement.FinalMoveValue.magnitude);
    }

    private void HandleFacingBlend()
    {
        animator.SetFloat(FACE_X_FLOAT, facingDirectionHandler.CurrentFacingDirection.x);
        animator.SetFloat(FACE_Y_FLOAT, facingDirectionHandler.CurrentFacingDirection.y);
    }

    protected void PlayAnimation(string animationName, int layer = 0, bool playEvenAfterDeath = false)
    {
        if (isDead && !playEvenAfterDeath) return;

        animator.Play(animationName, layer);
    }

    #region Subscriptions

    private void PlayerHealth_OnPlayerHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        PlayAnimation(TAKE_DAMAGE_ANIMATION_NAME,1);
    }

    private void PlayerHealth_OnPlayerDeath(object sender, System.EventArgs e)
    {
        PlayAnimation(DEATH_ANIMATION_NAME);
        isDead = true;
    }
    #endregion
}
