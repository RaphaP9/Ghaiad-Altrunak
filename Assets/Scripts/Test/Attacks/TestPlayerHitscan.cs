using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHitscan : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform testPrefab;

    private void OnEnable()
    {
        PlayerHitscanAttack.OnAnyPlayerHitscanRayShot += PlayerHitscanAttack_OnAnyPlayerHitscanRayShot;
    }

    private void OnDisable()
    {
        PlayerHitscanAttack.OnAnyPlayerHitscanRayShot -= PlayerHitscanAttack_OnAnyPlayerHitscanRayShot;
    }

    private void InstantiatePrefab(Vector2 position)
    {
        Instantiate(testPrefab, position, Quaternion.identity);
    }

    private void PlayerHitscanAttack_OnAnyPlayerHitscanRayShot(object sender, PlayerHitscanAttack.OnPlayerHitscanRayShotEventArgs e)
    {
        InstantiatePrefab(e.originPoint);
        InstantiatePrefab(e.hitPoint);
    }
}
