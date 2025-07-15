using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIdentifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilitySO abilitySO;
    [SerializeField] private Ability ability;

    public AbilitySO AbilitySO => abilitySO;
    public Ability Ability => ability;
}
