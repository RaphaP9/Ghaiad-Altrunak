using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantVisualHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform transformToReposition;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        MerchantSpawningHandler.OnMerchantSpawn += MerchantSpawningHandler_OnMerchantSpawn;
        MerchantSpawningHandler.OnMerchantDespawn += MerchantSpawningHandler_OnMerchantDespawn;
    }

    private void OnDisable()
    {
        MerchantSpawningHandler.OnMerchantSpawn -= MerchantSpawningHandler_OnMerchantSpawn;
        MerchantSpawningHandler.OnMerchantDespawn -= MerchantSpawningHandler_OnMerchantDespawn;
    }

    private void RepositionMerchant(Vector2 position)
    {
        transformToReposition.position = GeneralUtilities.Vector2ToVector3(position);
    }

    private void ShowMerchant()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideMerchant()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void MerchantSpawningHandler_OnMerchantSpawn(object sender, MerchantSpawningHandler.OnMerchantSpawnEventArgs e)
    {
        RepositionMerchant(e.position);
        ShowMerchant();
    }

    private void MerchantSpawningHandler_OnMerchantDespawn(object sender, System.EventArgs e)
    {
        HideMerchant();
    }
}
