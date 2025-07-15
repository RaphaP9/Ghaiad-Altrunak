using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumericFeedbackUI : FeedbackUI
{  
    public virtual void SetNumericFeedback(int value, Color textColor)
    {
        SetText(value.ToString());
        SetTextColor(textColor);
    }
}
