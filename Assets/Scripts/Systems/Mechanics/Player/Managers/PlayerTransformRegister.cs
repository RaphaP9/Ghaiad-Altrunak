using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformRegister : MonoBehaviour
{
    public static PlayerTransformRegister Instance {  get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private Transform playerTransform;

    public Transform PlayerTransform => playerTransform;

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

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one PlayerTransformRegister instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerTransform = e.playerTransform;
    }
}
