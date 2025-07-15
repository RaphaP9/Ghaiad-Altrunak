using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SortingOrderRenderingHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform renderingRefference;

    protected int previousSortingOrder = 0;

    private const int DECIMAL_PRECISION = 2;

    private void Start()
    {
        SetPreviousSortingOrder(0);
    }

    private void Update()
    {
        HandleSortingOrder();
    }

    private void HandleSortingOrder()
    {
        int newSortingOrder = CalculateSortingOrderDueToPosition();

        UpdateSortingOrder(newSortingOrder);

        SetPreviousSortingOrder(newSortingOrder);
    }

    private int CalculateSortingOrderDueToPosition()
    {
        int sortingOrder = Mathf.RoundToInt(-renderingRefference.position.y * Mathf.Pow(10f, DECIMAL_PRECISION +1));
        return sortingOrder;
    }

    private void SetPreviousSortingOrder(int sortingOrder) => previousSortingOrder = sortingOrder;
    protected abstract void UpdateSortingOrder(int newSortingOrder);
}
