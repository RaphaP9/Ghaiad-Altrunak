using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntityTakeDamage : MonoBehaviour
{
    [Header("Components - Assign On Runtime!")]
    [SerializeField] private EntityHealth entityHealth;

    private void Update()
    {
        TestTakeDamage();
    }

    private void TestTakeDamage()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            entityHealth.TakeDamage(new DamageData(1, true, null, false, false, false, true ));
        }
    }
}
