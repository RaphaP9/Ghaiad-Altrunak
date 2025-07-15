using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGold : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int gold;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GoldManager.Instance.AddGold(gold);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            GoldManager.Instance.SpendGold(gold);
        }
    }
}
