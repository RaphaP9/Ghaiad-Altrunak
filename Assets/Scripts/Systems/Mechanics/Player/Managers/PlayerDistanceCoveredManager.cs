using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDistanceCoveredManager : MonoBehaviour
{
    public static PlayerDistanceCoveredManager Instance {  get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private float playerDistanceCovered;
    [SerializeField] private PlayerMovement playerMovement;

    public float PlayerDistanceCovered => playerDistanceCovered;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        HandleDistanceCoveredUpdate();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HandleDistanceCoveredUpdate()
    {
        if (playerMovement == null) return;

        playerDistanceCovered = playerMovement.DistanceCovered;
    }

    public void ResetDistanceCovered() => playerDistanceCovered = 0f;

    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerMovement = e.playerTransform.GetComponentInChildren<PlayerMovement>();
    }
}
