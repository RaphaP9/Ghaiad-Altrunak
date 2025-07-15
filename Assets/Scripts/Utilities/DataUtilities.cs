using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DataUtilities 
{
    private const bool DEBUG = true;

    private const string PERPETUAL_DATA_PATH = "perpetualData.atdata";
    private const string RUN_DATA_PATH = "runData.atdata";

    #region Files Handling

    public static bool HasSavedRunData()
    {
        return CheckIfDataPathExists(RUN_DATA_PATH);
    }

    public static void WipeRunData()
    {
        DeleteDataInPath(RUN_DATA_PATH);
    }

    public static void WipeAllData()
    {
        DeleteDataInPath(PERPETUAL_DATA_PATH);
        DeleteDataInPath(RUN_DATA_PATH);
    }

    public static void DeleteDataInPaths(IEnumerable<string> dataPaths)
    {
        foreach (string dataPath in dataPaths)
        {
            DeleteDataInPath(dataPath);
        }
    }

    public static void DeleteDataInPath(string dataPath)
    {
        string dirPath = Application.persistentDataPath;

        string path = Path.Combine(dirPath, dataPath);

        if (!File.Exists(path))
        {
            Debug.Log("No data to delete");
        }
        else
        {
            File.Delete(path);
            Debug.Log("Data Deleted");
        }
    }

    public static bool CheckIfDataPathsExist(IEnumerable<string> dataPaths)
    {
        foreach (string dataPath in dataPaths)
        {
            if (!CheckIfDataPathExists(dataPath)) return false;
        }

        return true;
    }


    public static bool CheckIfDataPathExists(string dataPath)
    {
        string dirPath = Application.persistentDataPath;
        string path = Path.Combine(dirPath, dataPath);

        if (File.Exists(path)) return true;

        return false;
    }

    #endregion

    #region CharacterSO Translation
    public static CharacterSO TranslateCharacterIDToCharacterSO(int characterID)
    {
        if(CharacterAssetLibrary.Instance == null)
        {
            if (DEBUG) Debug.Log("CharacterAssetLibrary is null. Can not resolve CharacterSO Asset");
            return null;
        }

        CharacterSO characterSO = CharacterAssetLibrary.Instance.GetCharacterSOByID(characterID);
        return characterSO;
    }
    #endregion

    #region Numeric Stat Translation
    public static List<NumericStatModifier> TranslateDataModeledNumericStatsToNumericStatModifiers(List<DataModeledNumericStat> dataModeledNumericStats)
    {
        List<NumericStatModifier> numericStatModifiers = new List<NumericStatModifier>();

        foreach(DataModeledNumericStat dataModeledNumericStat in dataModeledNumericStats)
        {
            NumericStatModifier numericStatModifier = TranslateDataModeledNumericStatToNumericStatModifier(dataModeledNumericStat);
            if (numericStatModifier == null) continue;
            numericStatModifiers.Add(numericStatModifier);  
        }

        return numericStatModifiers;
    }
    private static NumericStatModifier TranslateDataModeledNumericStatToNumericStatModifier(DataModeledNumericStat dataModeledNumericStat)
    {
        NumericStatModifier numericStatModifier = new NumericStatModifier();

        numericStatModifier.originGUID = dataModeledNumericStat.originGUID;

        if (Enum.TryParse<NumericStatType>(dataModeledNumericStat.numericStatType, true, out var numericStatType)) numericStatModifier.numericStatType = numericStatType;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataModeledNumericStat.numericStatType}");
            return null;
        }

        if (Enum.TryParse<NumericStatModificationType>(dataModeledNumericStat.numericStatModificationType, true, out var numericStatModificationType)) numericStatModifier.numericStatModificationType = numericStatModificationType;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataModeledNumericStat.numericStatModificationType}");
            return null;
        }

        numericStatModifier.value = dataModeledNumericStat.value;

        return numericStatModifier;
    }

    //

    public static List<DataModeledNumericStat> TranslateNumericStatModifiersToDataModeledNumericStats(List<NumericStatModifier> numericStatModifiers)
    {
        List<DataModeledNumericStat> dataModeledNumericStats = new List<DataModeledNumericStat>();

        foreach(NumericStatModifier numericStatModifier in numericStatModifiers)
        {
            DataModeledNumericStat dataModeledNumericStat = TranslateNumericStatModifierToDataModeledNumericStat(numericStatModifier);
            if (dataModeledNumericStat == null) continue;
            dataModeledNumericStats.Add(dataModeledNumericStat);
        }

        return dataModeledNumericStats;
    }

    private static DataModeledNumericStat TranslateNumericStatModifierToDataModeledNumericStat(NumericStatModifier numericStatModifier)
    {
        string originGUID = numericStatModifier.originGUID;
        string numericStatType = numericStatModifier.numericStatType.ToString();
        string numericStatModificationType = numericStatModifier.numericStatModificationType.ToString();
        float value = numericStatModifier.value;    

        DataModeledNumericStat dataModeledNumericStat = new DataModeledNumericStat(originGUID,numericStatType,numericStatModificationType,value);
        return dataModeledNumericStat;
    }

    #endregion

    #region Ability Level Group Translation

    public static List<PrimitiveAbilityLevelGroup> TranslateDataModeledAbilityLevelGroupsToPrimitiveAbilityLevelGroups(List<DataModeledAbilityLevelGroup> dataModeledAbilityLevelGroups)
    {
        List<PrimitiveAbilityLevelGroup> primitiveAbilityLevelGroups = new List<PrimitiveAbilityLevelGroup>();

        foreach (DataModeledAbilityLevelGroup dataModeledAbilityLevelGroup in dataModeledAbilityLevelGroups)
        {
            PrimitiveAbilityLevelGroup primitiveAbilityLevelGroup = TranslateDataModeledAbilityLevelGroupToPrimitiveAbilityLevelGroup(dataModeledAbilityLevelGroup);
            if (primitiveAbilityLevelGroups == null) continue;
            primitiveAbilityLevelGroups.Add(primitiveAbilityLevelGroup);
        }

        return primitiveAbilityLevelGroups;
    }

    private static PrimitiveAbilityLevelGroup TranslateDataModeledAbilityLevelGroupToPrimitiveAbilityLevelGroup(DataModeledAbilityLevelGroup dataModeledAbilityLevelGroup)
    {
        PrimitiveAbilityLevelGroup primitiveAbilityLevelGroup = new PrimitiveAbilityLevelGroup();

        if (Enum.TryParse<AbilityLevel>(dataModeledAbilityLevelGroup.abilityLevel, true, out var abilityLevel)) primitiveAbilityLevelGroup.abilityLevel = abilityLevel;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataModeledAbilityLevelGroup.abilityLevel}");
            return null;
        }

        if (AbilitiesAssetLibrary.Instance == null)
        {
            if (DEBUG) Debug.Log("AbilitiesAssetLibrary is null. Can not resolve AbilitySO Asset.");
            return null;
        }

        primitiveAbilityLevelGroup.abilitySO = AbilitiesAssetLibrary.Instance.GetAbilitySOByID(dataModeledAbilityLevelGroup.abilityID);

        return primitiveAbilityLevelGroup;
    }

    public static List<DataModeledAbilityLevelGroup> TranslatePrimitiveAbilityLevelGroupsToDataModeledAbilityLevelGroups(List<PrimitiveAbilityLevelGroup> primitiveAbilityLevelGroups)
    {
        List<DataModeledAbilityLevelGroup> dataModeledAbilityLevelGroups = new List<DataModeledAbilityLevelGroup>();

        foreach (PrimitiveAbilityLevelGroup primitiveAbilityLevelGroup in primitiveAbilityLevelGroups)
        {
            DataModeledAbilityLevelGroup dataModeledAbilityLevelGroup = TranslatePrimitiveAbilityLevelGroupToDataModeledAbilityLevelGroup(primitiveAbilityLevelGroup);
            if (dataModeledAbilityLevelGroup == null) continue;
            dataModeledAbilityLevelGroups.Add(dataModeledAbilityLevelGroup);
        }

        return dataModeledAbilityLevelGroups;
    }

    private static DataModeledAbilityLevelGroup TranslatePrimitiveAbilityLevelGroupToDataModeledAbilityLevelGroup(PrimitiveAbilityLevelGroup primitiveAbilityLevelGroup)
    {
        int abilityID = primitiveAbilityLevelGroup.abilitySO.id;
        string abilityLevel = primitiveAbilityLevelGroup.abilityLevel.ToString();

        DataModeledAbilityLevelGroup dataModeledAbilityLevelGroup = new DataModeledAbilityLevelGroup(abilityID,abilityLevel);

        return dataModeledAbilityLevelGroup;
    }

    #endregion

    #region Ability Slots Variants Translation

    public static List<PrimitiveAbilitySlotGroup> TranslateDataModeledAbilitySlotGroupsToPrimitiveAbilitySlotGroups(List<DataModeledAbilitySlotGroup> dataModeledAbilitySlotGroups)
    {
        List<PrimitiveAbilitySlotGroup> primitiveAbilitySlotGroups = new List<PrimitiveAbilitySlotGroup>();

        foreach (DataModeledAbilitySlotGroup dataModeledAbilitySlotGroup in dataModeledAbilitySlotGroups)
        {
            PrimitiveAbilitySlotGroup primitiveAbilitySlotGroup = TranslateDataModeledAbilitySlotGroupToPrimitiveAbilitySlotGroup(dataModeledAbilitySlotGroup);
            if (primitiveAbilitySlotGroup == null) continue;
            primitiveAbilitySlotGroups.Add(primitiveAbilitySlotGroup);
        }

        return primitiveAbilitySlotGroups;
    }

    private static PrimitiveAbilitySlotGroup TranslateDataModeledAbilitySlotGroupToPrimitiveAbilitySlotGroup(DataModeledAbilitySlotGroup dataModeledAbilitySlotGroup)
    {
        PrimitiveAbilitySlotGroup primitiveAbilitySlotGroup = new PrimitiveAbilitySlotGroup();

        if (Enum.TryParse<AbilitySlot>(dataModeledAbilitySlotGroup.abilitySlot, true, out var abilitySlot)) primitiveAbilitySlotGroup.abilitySlot = abilitySlot;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataModeledAbilitySlotGroup.abilitySlot}");
            return null;
        }

        if (AbilitiesAssetLibrary.Instance == null)
        {
            if (DEBUG) Debug.Log("AbilitiesAssetLibrary is null. Can not resolve AbilitySO Asset.");
            return null;
        }

        primitiveAbilitySlotGroup.abilitySO = AbilitiesAssetLibrary.Instance.GetAbilitySOByID(dataModeledAbilitySlotGroup.abilityID);

        return primitiveAbilitySlotGroup;
    }

    public static List<DataModeledAbilitySlotGroup> TranslatePrimitiveAbilitySlotGroupsToDataModeledAbilitySlotGroups(List<PrimitiveAbilitySlotGroup> primitiveAbilitySlotGroups)
    {
        List<DataModeledAbilitySlotGroup> dataModeledAbilitySlotGroups = new List<DataModeledAbilitySlotGroup>();

        foreach (PrimitiveAbilitySlotGroup primitiveAbilitySlotGroup in primitiveAbilitySlotGroups)
        {
            DataModeledAbilitySlotGroup dataModeledAbilitySlotGroup = TranslatePrimitiveAbilitySlotGroupToDataModeledAbilitySlotGroup(primitiveAbilitySlotGroup);
            if (dataModeledAbilitySlotGroup == null) continue;
            dataModeledAbilitySlotGroups.Add(dataModeledAbilitySlotGroup);
        }

        return dataModeledAbilitySlotGroups;
    }

    private static DataModeledAbilitySlotGroup TranslatePrimitiveAbilitySlotGroupToDataModeledAbilitySlotGroup(PrimitiveAbilitySlotGroup primitiveAbilitySlotGroup)
    {
        string abilitySlot = primitiveAbilitySlotGroup.abilitySlot.ToString();
        int abilityID = primitiveAbilitySlotGroup.abilitySO.id;

        DataModeledAbilitySlotGroup dataModeledAbilitySlotGroup = new DataModeledAbilitySlotGroup(abilitySlot,abilityID);

        return dataModeledAbilitySlotGroup;
    }
    #endregion

    #region Objects Translation
    public static List<DataModeledObject> TranslateObjectsIdentifiedToDataModeledObjects(List<ObjectIdentified> objectsIdentified)
    {
        List<DataModeledObject> dataModeledObjects = new List<DataModeledObject>();

        foreach (ObjectIdentified objectIdentified in objectsIdentified)
        {
            DataModeledObject dataModeledObject = TranslateObjectIdentifiedToDataModeledObject(objectIdentified);
            if (dataModeledObject == null) continue;
            dataModeledObjects.Add(dataModeledObject);
        }

        return dataModeledObjects;
    }

    private static DataModeledObject TranslateObjectIdentifiedToDataModeledObject(ObjectIdentified objectIdentified)
    {
        string assignedGUID = objectIdentified.assignedGUID;
        int objectID = objectIdentified.objectSO.id;

        DataModeledObject dataModeledObject = new DataModeledObject(assignedGUID,objectID);  

        return dataModeledObject;
    }

    public static List<ObjectIdentified> TranslateDataModeledObjectsToObjectsIdentified(List<DataModeledObject> dataModeledObjects)
    {
        List<ObjectIdentified> objectsIdentified = new List<ObjectIdentified>();

        foreach (DataModeledObject dataModeledObject in dataModeledObjects)
        {
            ObjectIdentified objectIdentified = TranslateDataModeledObjectToObjectIdentified(dataModeledObject);
            if (objectIdentified == null) continue;
            objectsIdentified.Add(objectIdentified);
        }

        return objectsIdentified;
    }

    private static ObjectIdentified TranslateDataModeledObjectToObjectIdentified(DataModeledObject dataModeledObject)
    {
        ObjectIdentified objectIdentified = new ObjectIdentified();

        objectIdentified.assignedGUID = dataModeledObject.assignedGUID;

        if (ObjectsAssetLibrary.Instance == null)
        {
            if (DEBUG) Debug.Log("ObjectsAssetLibrary is null. Can not resolve ObjectSO Asset.");
            return null;
        }

        objectIdentified.objectSO = ObjectsAssetLibrary.Instance.GetObjectSOByID(dataModeledObject.objectID);

        return objectIdentified;
    }

    #endregion

    #region Treats Translation
    public static List<DataModeledTreat> TranslateTreatsIdentifiedToDataModeledTreats(List<TreatIdentified> treatsIdentified)
    {
        List<DataModeledTreat> dataModeledTreats = new List<DataModeledTreat>();

        foreach (TreatIdentified treatIdentified in treatsIdentified)
        {
            DataModeledTreat dataModeledTreat = TranslateTreatIdentifiedToDataModeledTreat(treatIdentified);
            if (dataModeledTreat == null) continue;
            dataModeledTreats.Add(dataModeledTreat);
        }

        return dataModeledTreats;
    }

    private static DataModeledTreat TranslateTreatIdentifiedToDataModeledTreat(TreatIdentified treatIdentified)
    {
        string assignedGUID = treatIdentified.assignedGUID;
        int treatID = treatIdentified.treatSO.id;

        DataModeledTreat dataModeledTreat = new DataModeledTreat(assignedGUID, treatID);

        return dataModeledTreat;
    }

    public static List<TreatIdentified> TranslateDataModeledTreatsToTreatsIdentified(List<DataModeledTreat> dataModeledTreats)
    {
        List<TreatIdentified> treatsIdentified = new List<TreatIdentified>();

        foreach (DataModeledTreat dataModeledTreat in dataModeledTreats)
        {
            TreatIdentified treatIdentified = TranslateDataModeledTreatToTreatIdentified(dataModeledTreat);
            if (treatIdentified == null) continue;
            treatsIdentified.Add(treatIdentified);
        }

        return treatsIdentified;
    }

    private static TreatIdentified TranslateDataModeledTreatToTreatIdentified(DataModeledTreat dataModeledTreat)
    {
        TreatIdentified treatIdentified = new TreatIdentified();

        treatIdentified.assignedGUID = dataModeledTreat.assignedGUID;

        if (TreatsAssetLibrary.Instance == null)
        {
            if (DEBUG) Debug.Log("TreatsAssetLibrary is null. Can not resolve TreatSO Asset.");
            return null;
        }

        treatIdentified.treatSO = TreatsAssetLibrary.Instance.GetTreatSOByID(dataModeledTreat.treatID);

        return treatIdentified;
    }
    #endregion
}
