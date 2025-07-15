using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldCounterUI : MonoBehaviour
{
    [Header("UIComponents")]
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        GoldManager.OnGoldInitialized += GoldManager_OnGoldInitialized;
        GoldManager.OnGoldAdded += GoldManager_OnGoldAdded;
        GoldManager.OnGoldSpent += GoldManager_OnGoldSpent;
    }

    private void OnDisable()
    {
        GoldManager.OnGoldInitialized -= GoldManager_OnGoldInitialized;
        GoldManager.OnGoldAdded -= GoldManager_OnGoldAdded;
        GoldManager.OnGoldSpent -= GoldManager_OnGoldSpent;
    }


    private void UpdateGoldText(int gold) => goldText.text = gold.ToString();


    #region Subscriptions
    private void GoldManager_OnGoldInitialized(object sender, GoldManager.OnGoldEventArgs e)
    {
        UpdateGoldText(e.currentGold);
    }

    private void GoldManager_OnGoldAdded(object sender, GoldManager.OnGoldChangedEventArgs e)
    {
        UpdateGoldText(e.newGold);
    }

    private void GoldManager_OnGoldSpent(object sender, GoldManager.OnGoldChangedEventArgs e)
    {
        UpdateGoldText(e.newGold);
    }
    #endregion
}
