using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStatAddition : MonoBehaviour, IHasEmbeddedNumericStats
{
    [Header("Components")]
    [SerializeField] private NumericStatModifierManager numericStatModifierManager;

    [Header("Settings")]
    [SerializeField] private List<NumericEmbeddedStat> numericEmbeddedStats;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            numericStatModifierManager.AddStatModifiers("TestGUID", this);
        }
    }

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats()
    {
        return numericEmbeddedStats;
    }
}
