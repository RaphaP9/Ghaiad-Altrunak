using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopObjectCardContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShopObjectCardUI shopObjectCardUI;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI objectNameText;
    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI objectClassificationText;
    [Space]
    [SerializeField] private Transform numericStatsContainer;
    [SerializeField] private Transform numericStatUISample;
    [Space]
    [SerializeField] private TextMeshProUGUI objectDescriptionText;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        shopObjectCardUI.OnInventoryObjectSet += ShopObjectCardUI_OnInventoryObjectSet;
    }
    private void OnDisable()
    {
        shopObjectCardUI.OnInventoryObjectSet -= ShopObjectCardUI_OnInventoryObjectSet;
    }

    public void CompleteSetUI(InventoryObjectSO inventoryObjectSO)
    {
        SetObjectNameText(inventoryObjectSO);
        SetObjectImage(inventoryObjectSO);
        SetObjectClassificationText(inventoryObjectSO);
        SetObjectDescriptionText(inventoryObjectSO);

        GenerateNumericStats(inventoryObjectSO);
    }

    private void SetObjectNameText(InventoryObjectSO inventoryObjectSO) => objectNameText.text = inventoryObjectSO._name;
    private void SetObjectImage(InventoryObjectSO inventoryObjectSO) => objectImage.sprite = inventoryObjectSO.sprite;
    private void SetObjectClassificationText(InventoryObjectSO inventoryObjectSO) => objectClassificationText.text = MappingUtilities.MapInventoryObjectRarityType(inventoryObjectSO);
    private void SetObjectDescriptionText(InventoryObjectSO inventoryObjectSO) => objectDescriptionText.text = inventoryObjectSO.description;
    private void GenerateNumericStats(InventoryObjectSO inventoryObjectSO)
    {
        ClearNumericStatsContainer();

        int printedNumericStats = 0; 

        foreach (NumericEmbeddedStat numericEmbeddedStat in inventoryObjectSO.GetNumericEmbeddedStats())
        {
            if (numericEmbeddedStat.numericStatModificationType == NumericStatModificationType.Replacement) continue; //NOTE: Replacement Stats Are not Printed!

            CreateNumericStat(numericEmbeddedStat);
            printedNumericStats++;
        }

        if (printedNumericStats <= 0) numericStatsContainer.gameObject.SetActive(false);
    }

    private void CreateNumericStat(NumericEmbeddedStat numericEmbeddedStat)
    {
        Transform numericStatUI = Instantiate(numericStatUISample, numericStatsContainer);

        InventoryObjectNumericStatUI inventoryObjectNumericStatUI = numericStatUI.GetComponent<InventoryObjectNumericStatUI>();

        if (inventoryObjectNumericStatUI == null)
        {
            if (debug) Debug.Log("Instantiated Numeric Stat UI does not contain a InventoryObjectNumericStatUI component. Set will be ignored.");
            return;
        }

        inventoryObjectNumericStatUI.SetNumericEmbededStat(numericEmbeddedStat);
        numericStatUI.gameObject.SetActive(true);
    }

    private void ClearNumericStatsContainer()
    {
        foreach (Transform child in numericStatsContainer)
        {
            if (child == numericStatUISample)
            {
                child.gameObject.SetActive(false);
                continue;
            }

            Destroy(child.gameObject);
        }
    }

    private void ShopObjectCardUI_OnInventoryObjectSet(object sender, ShopObjectCardUI.OnInventoryObjectEventArgs e)
    {
        CompleteSetUI(e.inventoryObjectSO);
    }

}
