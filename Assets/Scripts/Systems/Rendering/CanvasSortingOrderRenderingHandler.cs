using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSortingOrderRenderingHandler : SortingOrderRenderingHandler
{
    [Header("Canvas Components")]
    [SerializeField] private List<Canvas> UICanvases;

    protected override void UpdateSortingOrder(int sortingOrder)
    {
        if (sortingOrder == previousSortingOrder) return;

        foreach (Canvas canvas in UICanvases)
        {
            canvas.sortingOrder = sortingOrder + 1; //In Case Of Object with SpriteRenderer & Canvas, Canvas Always On Top
        }
    }
}
