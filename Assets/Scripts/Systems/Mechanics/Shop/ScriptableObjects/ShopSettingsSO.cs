using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopSettingsSO", menuName = "ScriptableObjects/Shop/ShopSettings")]

public class ShopSettingsSO : ScriptableObject
{
    [Header("Shop Size")]
    [Range(3, 7)]public int shopSize;

    [Header("Type Settings")]
    public List<ShopInventoryObjectTypeSetting> shopInventoryObjectTypeSettings;

    [Header("Rarity Settings")]
    public List<StageShopInventoryObjectRaritySetting> stageShopInventoryObjectRaritySettings;

    [Header("Rerolls")]
    [Range(0, 100)] public int rerollBaseCost;
    [Range(0, 10)] public int rerollCostIncreasePerReroll;

    [Header("Pools")]
    public List<ObjectSO> objectsPool;
    public List<TreatSO> treatsPool;

    [Header("Other")]
    public List<InventoryObjectSO> randomBreakerInventoryObjectList;
    public InventoryObjectType defaultInventoryObjectType;
    public Rarity defaultInventoryObjectRarity;


    //Methods

    #region Get Type Methods

    public ShopInventoryObjectTypeSetting GetShopInventoryObjectTypeSettingByObjectType(InventoryObjectType inventoryObjectType)
    {
        foreach (ShopInventoryObjectTypeSetting inventoryObjectTypeSetting in shopInventoryObjectTypeSettings)
        {
            if (inventoryObjectTypeSetting.inventoryObjectType == inventoryObjectType) return inventoryObjectTypeSetting;
        }

        Debug.Log($"InventoryObjectTypeSetting with InventoryObjectType: {inventoryObjectType} was not found. Proceding to return null.");
        return null;
    }

    #endregion

    #region Get Rarity Methods

    public StageShopInventoryObjectRaritySetting GetStageShopInventoryObjectRaritySettingByStage(int stageNumber)
    {
        foreach (StageShopInventoryObjectRaritySetting stageShopInventoryObjectRaritySetting in stageShopInventoryObjectRaritySettings)
        {
            if (stageShopInventoryObjectRaritySetting.stageNumber == stageNumber) return stageShopInventoryObjectRaritySetting;
        } 

        //Following logic to return nonExistent settings, Ex. if(settings stop to change in wave 5.) Wave 6,7... Keep same settings
        if(stageShopInventoryObjectRaritySettings.Count > 0)
        {
            Debug.Log($"StageShopInventoryObjectRaritySetting with StageNumber: {stageNumber} was not found. Proceding to return last element from StageShopInventoryObjectRaritySetting list.");
            return stageShopInventoryObjectRaritySettings[^1];
        }

        Debug.Log($"StageShopInventoryObjectRaritySettingsList has no elements. Proceding to return null.");
        return null;
    }

    public ShopInventoryObjectRaritySetting GetShopInventoryObjectRaritySettingByRarity(StageShopInventoryObjectRaritySetting stageShopInventoryObjectRaritySetting, Rarity inventoryObjectRarity)
    {
        foreach (ShopInventoryObjectRaritySetting shopInventoryObjectRaritySetting in stageShopInventoryObjectRaritySetting.shopInventoryObjectRaritySettings)
        {
            if (shopInventoryObjectRaritySetting.inventoryObjectRarity == inventoryObjectRarity) return shopInventoryObjectRaritySetting;
        }

        Debug.Log($"ShopInventoryObjectRaritySetting with Rarity: {inventoryObjectRarity} was not found. Proceding to return null.");
        return null;
    }

    public ShopInventoryObjectRaritySetting GetShopInventoryObjectRaritySettingByRarityAndStage(Rarity inventoryObjectRatity, int stageNumber)
    {
        StageShopInventoryObjectRaritySetting stageShopInventoryObjectRaritySetting = GetStageShopInventoryObjectRaritySettingByStage(stageNumber);

        if(stageShopInventoryObjectRaritySetting == null) return null;

        ShopInventoryObjectRaritySetting shopInventoryObjectRaritySetting = GetShopInventoryObjectRaritySettingByRarity(stageShopInventoryObjectRaritySetting,inventoryObjectRatity);

        return shopInventoryObjectRaritySetting;
    }

    #endregion
}