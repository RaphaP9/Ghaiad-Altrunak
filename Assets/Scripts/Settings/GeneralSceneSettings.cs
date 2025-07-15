using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSceneSettings : MonoBehaviour
{
    public static GeneralSceneSettings Instance { get; private set; }

    [Header("Starting Scene Settings")]
    [SerializeField] private string regularStartingScene;
    [SerializeField] private TransitionType regularStartingSceneTransitionType;
    [Space]
    [SerializeField] private string firstSessionStartingScene;
    [SerializeField] private TransitionType firstSessionStartingSceneTransitionType;

    [Header("New Game Scene Settings")]
    [SerializeField] private string characterSelectionScene;
    [SerializeField] private TransitionType characterSelectionSceneTransitionType;

    [Header("Run Scene Settings")]
    [SerializeField] private string regularRunScene;
    [SerializeField] private TransitionType regularRunSceneTransitionType;

    [Header("Win Scene Settings")]
    [SerializeField] private string regularWinScene;
    [SerializeField] private TransitionType regularWinSceneTransitionType;

    [Header("Lose Scene Settings")]
    [SerializeField] private string regularLoseScene;
    [SerializeField] private TransitionType regularLoseSceneTransitionType;

    [Header("Character Specific Scenes")]
    [SerializeField] private List<CharacterSpecificScenes> characterSpecificScenesList;

    #region Initialization
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
            //Debug.LogWarning("There is more than one GeneralSceneSettings instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    #endregion

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Starting Scene
    public bool ShouldTransitionToFirstSessionStartingScene() => SessionPerpetualDataContainer.Instance.PerpetualData.timesEnteredGame <= 0;

    public void TransitionToStartingScene()
    {
        if (ShouldTransitionToFirstSessionStartingScene()) ScenesManager.Instance.TransitionLoadTargetScene(firstSessionStartingScene, firstSessionStartingSceneTransitionType);
        else ScenesManager.Instance.TransitionLoadTargetScene(regularStartingScene, regularStartingSceneTransitionType);
    }
    #endregion

    #region PlayScene
    private bool ShouldTransitionToCharacterSelectionScene() => SessionPerpetualDataContainer.Instance.HasUnlockedCharacters();

    public void TransitionToNewGameScene()
    {
        if (ShouldTransitionToCharacterSelectionScene()) ScenesManager.Instance.TransitionLoadTargetScene(characterSelectionScene, characterSelectionSceneTransitionType);
        else TransitionToRunScene();
    }
    #endregion

    #region Run Scene
    public void TransitionToRunScene()
    {
        int runCharacterID = SessionRunDataContainer.Instance.RunData.currentCharacterID;

        CharacterSpecificScenes characterSpecificScenes = GetCharacterSpecificScenesByCharacterID(runCharacterID);
        DataModeledCharacterData dataModeledCharacterData = SessionPerpetualDataContainer.Instance.GetDataModeledCharacterDataByCharacterID(runCharacterID);

        if(characterSpecificScenes == null)
        {
            Debug.Log("CharacterSpecificScenes is null. Transitioning to regular run scene;");
            ScenesManager.Instance.TransitionLoadTargetScene(regularRunScene, regularRunSceneTransitionType);
            return;
        }

        if(dataModeledCharacterData == null)
        {
            Debug.Log("DataModeledCharacterData is null. Transitioning to regular run scene;");
            ScenesManager.Instance.TransitionLoadTargetScene(regularRunScene, regularRunSceneTransitionType);
            return;
        }

        //If has not played a run with that character, trasition to the firstRunScene
        if(dataModeledCharacterData.runsPlayed <= 0) ScenesManager.Instance.TransitionLoadTargetScene(characterSpecificScenes.firstRunScene, characterSpecificScenes.firstRunSceneTransitionType);
        else ScenesManager.Instance.TransitionLoadTargetScene(regularRunScene, regularRunSceneTransitionType);
    }
    #endregion

    #region Win Scene
    public void TransitionToWinScene()
    {
        int runCharacterID = SessionRunDataContainer.Instance.RunData.currentCharacterID;

        CharacterSpecificScenes characterSpecificScenes = GetCharacterSpecificScenesByCharacterID(runCharacterID);
        DataModeledCharacterData dataModeledCharacterData = SessionPerpetualDataContainer.Instance.GetDataModeledCharacterDataByCharacterID(runCharacterID);

        if (characterSpecificScenes == null)
        {
            Debug.Log("CharacterSpecificScenes is null. Transitioning to regular win scene;");
            ScenesManager.Instance.TransitionLoadTargetScene(regularWinScene, regularWinSceneTransitionType);
            return;
        }

        if (dataModeledCharacterData == null)
        {
            Debug.Log("DataModeledCharacterData is null. Transitioning to regular win scene;");
            ScenesManager.Instance.TransitionLoadTargetScene(regularWinScene, regularWinSceneTransitionType);
            return;
        }

        //If has only won a run with that character, trasition to the firstWinScene
        //NOTE: On Win, first update the data (runsWon +=1) and then load win scene
        if (dataModeledCharacterData.runsWon <= 1) ScenesManager.Instance.TransitionLoadTargetScene(characterSpecificScenes.firstWinScene, characterSpecificScenes.firstWinSceneTransitionType);
        else ScenesManager.Instance.TransitionLoadTargetScene(regularWinScene, regularWinSceneTransitionType);
    }
    #endregion

    #region Lose Scene
    public void TransitionToLoseScene()
    {
        int runCharacterID = SessionRunDataContainer.Instance.RunData.currentCharacterID;

        CharacterSpecificScenes characterSpecificScenes = GetCharacterSpecificScenesByCharacterID(runCharacterID);
        DataModeledCharacterData dataModeledCharacterData = SessionPerpetualDataContainer.Instance.GetDataModeledCharacterDataByCharacterID(runCharacterID);

        if (characterSpecificScenes == null)
        {
            Debug.Log("CharacterSpecificScenes is null. Transitioning to regular lose scene;");
            ScenesManager.Instance.TransitionLoadTargetScene(regularLoseScene, regularLoseSceneTransitionType);
            return;
        }

        if (dataModeledCharacterData == null)
        {
            Debug.Log("DataModeledCharacterData is null. Transitioning to regular lose scene;");
            ScenesManager.Instance.TransitionLoadTargetScene(regularLoseScene, regularLoseSceneTransitionType);
            return;
        }

        //If has only won a run with that character, trasition to the firstLoseScene
        //NOTE: On Win, first update the data (runsLost +=1) and then load lose scene
        if (dataModeledCharacterData.runsLost <= 1) ScenesManager.Instance.TransitionLoadTargetScene(characterSpecificScenes.firstLoseScene, characterSpecificScenes.firstLoseSceneTransitionType);
        else ScenesManager.Instance.TransitionLoadTargetScene(regularLoseScene, regularLoseSceneTransitionType);
    }
    #endregion

    #region Utilities
    private CharacterSpecificScenes GetCharacterSpecificScenesByCharacterID(int characterID)
    {
        foreach (CharacterSpecificScenes characterSpecificScenes in characterSpecificScenesList)
        {
            if (characterSpecificScenes.characterSO.id == characterID) return characterSpecificScenes;
        }

        Debug.Log($"Could not find CharacterSpecificScenes for CharacterID: {characterID}");
        return null;
    }

    #endregion
}

[System.Serializable]
public class CharacterSpecificScenes
{
    public CharacterSO characterSO;
    [Space]
    public string firstRunScene;
    public TransitionType firstRunSceneTransitionType;
    [Space]
    public string firstWinScene;
    public TransitionType firstWinSceneTransitionType;
    [Space]
    public string firstLoseScene;
    public TransitionType firstLoseSceneTransitionType;
}
