using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntityHeal : MonoBehaviour
{
    [Header("Components - Assign On Runtime!")]
    [SerializeField] private EntityHealth entityHealth;

    private void Update()
    {
        TestHeal();
    }

    private void TestHeal()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            entityHealth.Heal(new HealData(2,null));
        }
    }
}
