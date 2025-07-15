using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Globalization;

public class CooldownCounterUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI cooldownText;

    private float previousIntCooldownTimer;
    private float previousRoundedCooldownTimer;

    private const float ROUNDED_DISPLAY_THRESHOLD = 1f;

    public void UpdateCooldownValues(float cooldownTimer)
    {
        float intCooldownTimer = Mathf.FloorToInt(cooldownTimer);
        float roundedCooldownTimer = GeneralUtilities.CeilToNDecimalPlaces(cooldownTimer,1);

        if (cooldownTimer > ROUNDED_DISPLAY_THRESHOLD)
        {
            if (intCooldownTimer == previousIntCooldownTimer) return;

            SetCooldownText(intCooldownTimer);
            previousIntCooldownTimer = intCooldownTimer;
        }
        else
        {
            if(roundedCooldownTimer == previousRoundedCooldownTimer) return;

            SetCooldownText(roundedCooldownTimer);
            previousRoundedCooldownTimer = roundedCooldownTimer;
        }
    }

    public void ClearCooldownValues()
    {
        previousIntCooldownTimer = 0;
        previousRoundedCooldownTimer = 0f;
    }

    private void SetCooldownText(float timer) => cooldownText.text = UIUtilities.ProcessFloatToString(timer);
}
