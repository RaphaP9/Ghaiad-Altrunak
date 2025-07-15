using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShopGenerator : MonoBehaviour
{
    public static ShopGenerator Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private ShopSettingsSO shopSettingsSO;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private const int MAX_OBJECT_GENERATION_ITERATIONS = 20;

    private void Awake()
    {
        SetSingleton();
    }
    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShopGenerator instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region GetAvailableInventoryObjects
    private List<ObjectSO> GetShopAvailableObjectsFromCompleteObjectsList(ShopSettingsSO shopSettingsSO)
    {
        List<ObjectSO> validObjects = new List<ObjectSO>();

        foreach (ObjectSO @object in shopSettingsSO.objectsPool)
        {
            validObjects.Add(@object); //There is no actual restriction to objects
        }

        return validObjects;
    }

    private List<TreatSO> GetShopAvailableTreatsFromCompleteTreatsList(ShopSettingsSO shopSettingsSO)
    {
        List<TreatSO> validTreats = new List<TreatSO>();

        foreach (TreatSO treat in shopSettingsSO.treatsPool)
        {
            if (TreatsInventoryManager.Instance.HasTreatSOInInventory(treat)) continue; //Unique Treats
            validTreats.Add(treat);
        }

        return validTreats;
    }

    private List<InventoryObjectSO> GetShopAvailableInventoryObjectsList(ShopSettingsSO shopSettingsSO)
    {
        List<InventoryObjectSO> validObjectsAsInventoryObjects = GetShopAvailableObjectsFromCompleteObjectsList(shopSettingsSO).Select(x => x as InventoryObjectSO).ToList();
        List<InventoryObjectSO> validTreatsAsInventoryObjects = GetShopAvailableTreatsFromCompleteTreatsList(shopSettingsSO).Select(x => x as InventoryObjectSO).ToList();

        List<List<InventoryObjectSO>> unnappendedAvailableInventoryObjectsList = new List<List<InventoryObjectSO>> { validObjectsAsInventoryObjects, validTreatsAsInventoryObjects };
        List<InventoryObjectSO> availableInventoryObjects = GeneralUtilities.AppendListsOfLists(unnappendedAvailableInventoryObjectsList);

        return availableInventoryObjects;
    }
    #endregion

    #region Generate Shop 

    public List<InventoryObjectSO> GenerateShopObjectsList(int stageNumber)
    {
        List<InventoryObjectSO> availableInventoryObjectsList = GetShopAvailableInventoryObjectsList(shopSettingsSO);

        List<InventoryObjectSO> generatedList = new List<InventoryObjectSO>();

        for (int i = 0; i < shopSettingsSO.shopSize; i++)
        {
            InventoryObjectSO shopObject = GenerateShopObject(shopSettingsSO, stageNumber, availableInventoryObjectsList, generatedList);

            if (shopObject == null)
            {
                if (debug) Debug.Log("Shop object is null and will not be added to generated list. List will be short sized.");
                continue;
            }

            generatedList.Add(shopObject);
        }

        return generatedList;
    }

    private InventoryObjectSO GenerateShopObject(ShopSettingsSO shopSettingsSO, int stageNumber, List<InventoryObjectSO> availableInventoryObjectsList, List<InventoryObjectSO> currentGeneratedList)
    {
        bool validObject = false;
        int iterations = 0;

        InventoryObjectSO selectedInventoryObject = null;

        List<ShopInventoryObjectTypeSetting> typeSettings = shopSettingsSO.shopInventoryObjectTypeSettings; //General for all stages
        List<ShopInventoryObjectRaritySetting> raritySettings = shopSettingsSO.GetStageShopInventoryObjectRaritySettingByStage(stageNumber).shopInventoryObjectRaritySettings; //Depends on stage number

        while (!validObject && iterations < MAX_OBJECT_GENERATION_ITERATIONS)
        {
            iterations++;

            InventoryObjectType targetObjectType = GenerateInventoryObjectType(typeSettings); //Generate Type & Rarity
            Rarity targetObjectRarity = GenerateInventoryObjectRarity(raritySettings);

            ShopInventoryObjectTypeSetting typeSetting = shopSettingsSO.GetShopInventoryObjectTypeSettingByObjectType(targetObjectType); //Get the setting that matches type
            ShopInventoryObjectRaritySetting raritySetting = shopSettingsSO.GetShopInventoryObjectRaritySettingByRarityAndStage(targetObjectRarity, stageNumber); //Get the setting that matches rarity & stage

            if (HasReachedTypeCap(typeSetting, targetObjectType, currentGeneratedList)) continue;
            if (HasReachedRarityCap(raritySetting, targetObjectRarity, currentGeneratedList)) continue;

            InventoryObjectSO foundInventoryObject = GetRandomInventoryObjectFromListByTypeAndRarity(availableInventoryObjectsList, targetObjectType, targetObjectRarity);

            if (foundInventoryObject != null)
            {
                selectedInventoryObject = foundInventoryObject;
                validObject = true;
            }
        }

        if (selectedInventoryObject == null) //In case all iterations failed to find a valid inventory object (respecting the caps limit), find a random unrestricted object from the randomBreaker list
        {
            if (debug) Debug.Log("Selecting a random element from the Random Breaker List.");
            selectedInventoryObject = GetRandomInventoryObjectFromList(shopSettingsSO.randomBreakerInventoryObjectList);
        }

        if (selectedInventoryObject == null)
        {
            if (debug) Debug.Log("Could not find an inventory object.");
        }

        return selectedInventoryObject;
    }

    #endregion

    #region Generate Rarity&Type

    private Rarity GenerateInventoryObjectRarity(List<ShopInventoryObjectRaritySetting> shopInventoryObjectRaritySettings)
    {
        int totalWeight = shopInventoryObjectRaritySettings.Sum(x => x.weight);

        if (totalWeight <= 0) return shopSettingsSO.defaultInventoryObjectRarity;

        System.Random random = new System.Random();
        int randomValue = random.Next(0, totalWeight) +1;

        int currentWeight = 0;

        foreach (ShopInventoryObjectRaritySetting raritySetting in shopInventoryObjectRaritySettings)
        {
            currentWeight += raritySetting.weight;

            if (randomValue <= currentWeight) return raritySetting.inventoryObjectRarity;
        }

        return shopInventoryObjectRaritySettings[0].inventoryObjectRarity;
    }

    private InventoryObjectType GenerateInventoryObjectType(List<ShopInventoryObjectTypeSetting> shopInventoryObjectTypeSettings)
    {
        int totalWeight = shopInventoryObjectTypeSettings.Sum(x => x.weight);

        if (totalWeight <= 0) return shopSettingsSO.defaultInventoryObjectType;

        System.Random random = new System.Random();
        int randomValue = random.Next(0, totalWeight) +1;

        int currentWeight = 0;

        foreach (ShopInventoryObjectTypeSetting typeSetting in shopInventoryObjectTypeSettings)
        {
            currentWeight += typeSetting.weight;

            if (randomValue <= currentWeight) return typeSetting.inventoryObjectType;
        }

        return shopInventoryObjectTypeSettings[0].inventoryObjectType;
    }
    #endregion

    #region CapReached
    private bool HasReachedTypeCap(ShopInventoryObjectTypeSetting inventoryObjectTypeSetting, InventoryObjectType inventoryObjectType, List<InventoryObjectSO> generatedInventoryObjectList)
    {
        if (inventoryObjectTypeSetting == null)
        {
            if (debug) Debug.Log("InventoryObjectTypeSetting is null. Can not define if type cap reached.");
            return false;
        }

        int accumulator = 0;

        foreach (InventoryObjectSO inventoryObjectSO in generatedInventoryObjectList)
        {
            if (IsInventoryObjectOfType(inventoryObjectSO, inventoryObjectType)) accumulator += 1;
        }

        if (accumulator >= inventoryObjectTypeSetting.cap) return true;
        return false;
    }

    private bool HasReachedRarityCap(ShopInventoryObjectRaritySetting inventoryObjectRaritySetting, Rarity inventoryObjectRarity, List<InventoryObjectSO> generatedInventoryObjectList)
    {
        if (inventoryObjectRaritySetting == null)
        {
            if (debug) Debug.Log("InventoryObjectRaritySetting is null. Can not define if rarity cap reached.");
            return false;
        }

        int accumulator = 0;

        foreach (InventoryObjectSO inventoryObjectSO in generatedInventoryObjectList)
        {
            if (IsInventoryObjectOfRarity(inventoryObjectSO, inventoryObjectRarity)) accumulator += 1;
        }

        if (accumulator >= inventoryObjectRaritySetting.cap) return true;
        return false;
    }

    #endregion

    #region Get Inventory Object From List with Type & Rarity
    private InventoryObjectSO GetRandomInventoryObjectFromListByTypeAndRarity(List<InventoryObjectSO> inventoryObjectList, InventoryObjectType targetObjectType, Rarity targetObjectRarity)
    {
        List<InventoryObjectSO> filteredInventoryObjectList = new List<InventoryObjectSO>();

        foreach (InventoryObjectSO inventoryObject in inventoryObjectList) //First on list that matches conditions(Rarity,Type) is returned
        {
            if (!IsInventoryObjectOfRarity(inventoryObject, targetObjectRarity)) continue;
            if (!IsInventoryObjectOfType(inventoryObject, targetObjectType)) continue;

            filteredInventoryObjectList.Add(inventoryObject);
        }

        if (filteredInventoryObjectList.Count <= 0)
        {
            //if (debug) Debug.Log($"No matching inventory objects found for the given Type: {targetObjectType} and Rarity: {targetObjectRarity}. Proceding to return null.");
            return null;
        }

        return GetRandomInventoryObjectFromList(filteredInventoryObjectList);
    }

    private InventoryObjectSO GetRandomInventoryObjectFromListByTypeAndRarityFisherYatesShuffle(List<InventoryObjectSO> inventoryObjectList, InventoryObjectType targetObjectType, Rarity targetObjectRarity)
    {
        List<InventoryObjectSO> shuffledInventoryObjectList = GeneralUtilities.FisherYatesShuffle(inventoryObjectList);

        foreach (InventoryObjectSO inventoryObject in shuffledInventoryObjectList) //First on list that matches conditions(Rarity,Type) is returned
        {
            if (!IsInventoryObjectOfRarity(inventoryObject, targetObjectRarity)) continue;
            if (!IsInventoryObjectOfType(inventoryObject, targetObjectType)) continue;

            return inventoryObject;
        }

        //if (debug) Debug.Log($"No object in inventoryObjectList matches Type: {targetObjectType} & Rarity: {targetObjectRarity}. Proceding to return null.");
        return null;
    }

    private InventoryObjectSO GetRandomInventoryObjectFromList(List<InventoryObjectSO> inventoryObjectList)
    {
        if (inventoryObjectList.Count <= 0)
        {
            if (debug) Debug.Log("List does not contain any elements. Proceding to return null");
            return null;
        }

        InventoryObjectSO randomInventoryObject = GeneralUtilities.ChooseRandomElementFromList(inventoryObjectList);
        return randomInventoryObject;
    }
    #endregion

    #region Check Type & Rarity
    private bool IsInventoryObjectOfType(InventoryObjectSO inventoryObjectSO, InventoryObjectType inventoryObjectType)
    {
        if (inventoryObjectSO.GetInventoryObjectType() == inventoryObjectType) return true;
        return false;
    }

    private bool IsInventoryObjectOfRarity(InventoryObjectSO inventoryObjectSO, Rarity inventoryObjectRarity)
    {
        if (inventoryObjectSO.objectRarity == inventoryObjectRarity) return true;
        return false;
    }
    #endregion
}

