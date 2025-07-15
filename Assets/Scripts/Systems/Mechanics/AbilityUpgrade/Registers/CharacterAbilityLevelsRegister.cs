using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAbilityLevelsRegister : MonoBehaviour
{
    public static CharacterAbilityLevelsRegister Instance { get; private set; }

    [Header("Lists - Runtime Filled")]
    [SerializeField] private List<AbilityLevelHandler> abilityLevelHandlers;

    public List<AbilityLevelHandler> AbilityLevelHandlers => abilityLevelHandlers;

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
            Destroy(gameObject);
        }
    }

    private void PerformPlayerTransformAbilitiesSearch(Transform playerTransform)
    {
        ClearRegisteredAbilityLevelsList();

        AbilityLevelHandler[] abilityLevelHandlers = playerTransform.GetComponentsInChildren<AbilityLevelHandler>();
        SetRegisteredAbilityLevelsList(abilityLevelHandlers.ToList());
    }

    private void SetRegisteredAbilityLevelsList(List<AbilityLevelHandler> abilityLevelHandlers) => this.abilityLevelHandlers = abilityLevelHandlers;
    private void ClearRegisteredAbilityLevelsList() => abilityLevelHandlers.Clear();

    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        PerformPlayerTransformAbilitiesSearch(e.playerTransform);
    }
}
