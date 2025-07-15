using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHoverHandler : MonoBehaviour
{
    [Header("Hover Handler Specifics")]
    [SerializeField] protected RectTransform rectTransformRefference;
    [Space]
    [SerializeField] protected List<PivotQuadrant> pivotQuadrants;

    [System.Serializable]
    public class PivotQuadrant
    {
        public ScreenQuadrant screenQuadrant;
        public RectTransform rectTransformPoint;
    }

    protected PivotQuadrant GetPivotQuadrantByScreenQuadrant(ScreenQuadrant screenQuadrant)
    {
        foreach (PivotQuadrant pivotQuadrant in pivotQuadrants)
        {
            if(pivotQuadrant.screenQuadrant == screenQuadrant) return pivotQuadrant;
        }

        return null;
    }
}
