using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCounterManager : MonoBehaviour
{
    public static PlayerAttackCounterManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private int attacksPerformed;

    public int AttacksPerformed => attacksPerformed;

    private void OnEnable()
    {
        PlayerAttack.OnAnyPlayerAttack += PlayerAttack_OnAnyPlayerAttack;
    }

    private void OnDisable()
    {
        PlayerAttack.OnAnyPlayerAttack -= PlayerAttack_OnAnyPlayerAttack;
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

    private void IncreaseAttacksPerformed(int quantity) => attacksPerformed += quantity;
    public void ResetAttacksPerformed() => attacksPerformed = 0;

    private void PlayerAttack_OnAnyPlayerAttack(object sender, PlayerAttack.OnPlayerAttackEventArgs e)
    {
        IncreaseAttacksPerformed(1);
    }
}
