using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStagesManager : MonoBehaviour
{
    public static GeneralStagesManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<CharacterStageGroupList> characterStageGroupLists;

    [Header("Components")]
    [SerializeField] private Transform stagePoint;

    [Header("Settings")]
    [SerializeField] private int startingStageNumber;
    [SerializeField] private int startingRoundNumber;

    [Header("States - Runtime Filled")]
    [SerializeField] private RoundState roundState;

    [Header("Runtime Filled")]
    [SerializeField] private CharacterStageGroupList currentCharacterStageGroupList;
    [SerializeField] private StageGroup currentStageGroup;
    [SerializeField] private RoundGroup currentRoundGroup;
    [SerializeField] private RoundSO currentRound;
    [Space]
    [SerializeField] private int currentStageNumber;
    [SerializeField] private int currentRoundNumber;
    [Space]
    [SerializeField] private StageGroup lastCompletedStageGroup;
    [SerializeField] private RoundGroup lastCompletedRoundGroup;
    [Space]
    [SerializeField] private bool hasCompletedAllRounds;

    [Header("Debug")]
    [SerializeField] private bool debug;

    #region Properties
    public bool HasCompletedAllRounds => hasCompletedAllRounds;
    public StageGroup CurrentStageGroup => currentStageGroup;
    public int CurrentStageNumber => currentStageNumber;
    public int CurrentRoundNumber => currentRoundNumber;
    #endregion

    private enum RoundState {NotOnRound, OnRound}
    private StageGroup previousStageGroup;
    private RoundGroup previousRoundGroup;

    #region Events
    public static event EventHandler<OnStageAndRoundEventArgs> OnStageAndRoundInitialized;
    public static event EventHandler<OnStageAndRoundLoadEventArgs> OnStageAndRoundLoad;

    public static event EventHandler<OnRoundEventArgs> OnRoundStart;
    public static event EventHandler<OnRoundEventArgs> OnRoundEnd;

    public static event EventHandler<OnStageChangeEventArgs> OnStageInitialized;
    public static event EventHandler<OnStageChangeEventArgs> OnStageChange;
    #endregion

    #region EventArgs Classes
    public class OnStageAndRoundEventArgs : EventArgs
    {
        public StageGroup stageGroup;
        public RoundGroup roundGroup;

        public int stageNumber;
        public int roundNumber;
    }

    public class OnRoundEventArgs : EventArgs
    {
        public StageGroup stageGroup;
        public RoundGroup roundGroup;

        public int stageNumber;
        public int roundNumber;

        public RoundSO roundSO;
    }

    public class OnStageAndRoundLoadEventArgs : EventArgs
    {
        public StageGroup previousStageGroup;
        public RoundGroup previousRoundGroup;
        public int previousStageNumber;
        public int previousRoundNumber;

        public StageGroup newStageGroup;
        public RoundGroup newRoundGroup;
        public int newStageNumber;
        public int newRoundNumber;
    }

    public class OnStageChangeEventArgs : EventArgs
    {
        public StageGroup stageGroup;
        public int stageNumber;
    }
    #endregion

    #region Consts

    private const int FIRTS_STAGE_NUMBER = 1;
    private const int FIRST_ROUND_NUMBER = 1;

    private const int NOT_FOUND_VALUE = 0;
    private const int LAST_VALUE = -1;
    #endregion

    #region Flags
    private bool currentRoundEnded = false;
    #endregion

    private void OnEnable()
    {
        PlayerCharacterManager.OnPlayerCharacterSetPreInstantiation += PlayerCharacterManager_OnPlayerCharacterSetPreInstantiation;
        RoundHandler.OnRoundCompleted += RoundHandler_OnRoundCompleted;
    }

    private void OnDisable()
    {
        PlayerCharacterManager.OnPlayerCharacterSetPreInstantiation -= PlayerCharacterManager_OnPlayerCharacterSetPreInstantiation;
        RoundHandler.OnRoundCompleted -= RoundHandler_OnRoundCompleted;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetRoundState(RoundState.NotOnRound);
        ClearLastCompletedStageGroup();
        ClearLastCompletedtRoundGroup();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GeneralStagesManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Called By Player Character Manager Subscription
    private void SelectCharacterStageGroupList(CharacterSO characterSO)
    {
        bool found = false;

        foreach(CharacterStageGroupList characterStageGroupList in characterStageGroupLists)
        {
            if(characterStageGroupList.chartacterSO == characterSO)
            {
                currentCharacterStageGroupList = characterStageGroupList;
                found = true;
                break;
            }
        }

        if(!found)
        {
            if (debug) Debug.Log($"CharacterStageGroupList for Character: {characterSO.entityName} could not be found. Selecting first element from CharacterStageGroupLists");
            currentCharacterStageGroupList = characterStageGroupLists[0];
        }
    }

    private void InitializeStage()
    {
        currentStageNumber = startingStageNumber <= 0 ? FIRTS_STAGE_NUMBER : startingStageNumber;
        currentRoundNumber = startingRoundNumber <= 0 ? FIRST_ROUND_NUMBER : startingRoundNumber;

        StageGroup stageGroup = LocateStageGroupByStageNumber(currentStageNumber);
        RoundGroup roundGroup = LocateRoundGroupByRoundNumber(currentStageNumber, currentRoundNumber);

        //Verify if currentValues do exist
        if (stageGroup == null)
        {
            SetCurrentStageNumber(FIRTS_STAGE_NUMBER);
            SetCurrentRoundNumber(FIRST_ROUND_NUMBER);

            stageGroup = LocateStageGroupByStageNumber(currentStageNumber);
            roundGroup = LocateRoundGroupByRoundNumber(currentStageNumber, currentRoundNumber);

            if (debug) Debug.Log("Could not locate StageGroup on Initialization, reseting Stage & Round Numbers to Initials.");
        }
        else if (roundGroup == null)
        {
            SetCurrentStageNumber(FIRTS_STAGE_NUMBER);
            SetCurrentRoundNumber(FIRST_ROUND_NUMBER);

            stageGroup = LocateStageGroupByStageNumber(currentStageNumber);
            roundGroup = LocateRoundGroupByRoundNumber(currentStageNumber, currentRoundNumber);

            if (debug) Debug.Log("Could not locate RoundGroup on Initialization, reseting Stage & Round Numbers to Initials.");
        }

        SetCurrentStageGroup(stageGroup);
        SetCurrentRoundGroup(roundGroup);

        OnStageAndRoundInitialized?.Invoke(this, new OnStageAndRoundEventArgs {stageGroup = stageGroup, roundGroup = roundGroup, stageNumber = currentStageNumber, roundNumber = currentRoundNumber });
    }
    #endregion

    #region Rounds
    public void StartCurrentRound() //RoundBlockIncludes Starting and Ending Time periods
    {
        if (!CanStartRound()) return;

        if (currentStageGroup == null)
        {
            if (debug) Debug.Log("Current Stage Group is null. Can not start current round.");
            return;
        }

        if (currentRoundGroup == null)
        {
            if (debug) Debug.Log("Current Round Group is null. Can not start current round.");
            return;
        }

        if(currentRoundGroup.rounds.Count <= 0)
        {
            if (debug) Debug.Log("Current Round Group has no rounds. Can not start current round.");
            return;
        }

        RoundSO roundSO = currentRoundGroup.GetRandomRoundFromRoundsList();

        StartCoroutine(CurrentRoundCoroutine(roundSO));
    }

    private IEnumerator CurrentRoundCoroutine(RoundSO roundSO)
    {
        SetCurrentRound(roundSO);

        StageGroup stageGroup = currentStageGroup;
        RoundGroup roundGroup = currentRoundGroup;
        int stageNumber = currentStageNumber;
        int roundNumber = currentRoundNumber;

        SetRoundState(RoundState.OnRound);
        OnRoundStart?.Invoke(this, new OnRoundEventArgs { stageGroup = stageGroup, roundGroup = roundGroup, stageNumber = stageNumber, roundNumber = roundNumber, roundSO = roundSO });

        currentRoundEnded = false;
        StartRoundLogic(roundSO, stageGroup);
        yield return new WaitUntil(() => currentRoundEnded);
        currentRoundEnded = false;

        SetLastCompletedStageGroup(currentStageGroup);
        SetLastCompletedRoundGroup(currentRoundGroup);

        SetRoundState(RoundState.NotOnRound);

        CheckAllRoundsCompleted();

        OnRoundEnd?.Invoke(this, new OnRoundEventArgs { stageGroup = stageGroup, roundGroup = roundGroup, stageNumber = stageNumber, roundNumber = roundNumber, roundSO = roundSO });

        ClearCurrentRound();
    }

    private void StartRoundLogic(RoundSO roundSO, StageGroup stageGroup)
    {
        switch (roundSO.GetRoundType())
        {
            case RoundType.Timed:
                TimedRoundHandler.Instance.StartTimedRound(roundSO as TimedRoundSO, stageGroup.stageHandler.StageSpawnPointsHandler);
                break;
            case RoundType.Waves:
                WavesRoundHandler.Instance.StartWavesRound(roundSO as WavesRoundSO, stageGroup.stageHandler.StageSpawnPointsHandler);
                break;
            case RoundType.BossFight:
                BossFightRoundHandler.Instance.StartBossFightRound(roundSO as BossFightRoundSO, stageGroup.stageHandler.StageSpawnPointsHandler);
                break;
        }
    }

    private bool CanStartRound()
    {
        if(roundState != RoundState.NotOnRound) return false;
        return true;
    }

    private void CheckAllRoundsCompleted()
    {
        if(LastCompletedStageAndRoundNumberAreLasts()) hasCompletedAllRounds = true;
        else hasCompletedAllRounds = false;
    }
    #endregion

    #region UtilityMethods
    private StageGroup LocateStageGroupByStageNumber(int stageNumber)
    {
        if (stageNumber <= 0) return null;

        if (stageNumber > currentCharacterStageGroupList.stageGroups.Count)
        {
            //if (debug) Debug.Log($"Stages are less than Stage Number: {stageNumber}. Returning null.");
            return null;
        }

        return currentCharacterStageGroupList.stageGroups[stageNumber - 1];
    }

    private RoundGroup LocateRoundGroupByRoundNumber(int stageNumber, int roundNumber)
    {
        if (stageNumber <= 0) return null;
        if (roundNumber <= 0) return null;

        StageGroup stageGroup = LocateStageGroupByStageNumber(stageNumber);

        if (stageGroup == null)
        {
            //if (debug) Debug.Log($"As StageGroup is null, RoundGroup can not be found. Returning null.");
            return null;
        }

        if (roundNumber > stageGroup.stageSO.roundGroups.Count)
        {
            //if (debug) Debug.Log($"Rounds are less than Round Number: {roundNumber}. Returning null.");
            return null;
        }

        return stageGroup.stageSO.roundGroups[roundNumber - 1];
    }

    private bool IsFirstStageGroup(StageGroup stageGroup) => stageGroup == currentCharacterStageGroupList.stageGroups[0];
    private bool IsLastStageGroup(StageGroup stageGroup) => stageGroup == currentCharacterStageGroupList.stageGroups[^1];
    private bool IsFirstRoundGroupFromStageGroup(StageGroup stageGroup, RoundGroup roundGroup) => roundGroup == stageGroup.stageSO.roundGroups[0];
    private bool IsLastRoundGroupFromStageGroup(StageGroup stageGroup, RoundGroup roundGroup) => roundGroup == stageGroup.stageSO.roundGroups[^1];

    private bool StageAndRoundNumberAreFirsts(StageGroup stageGroup, RoundGroup roundGroup)
    {
        if (IsFirstStageGroup(stageGroup))
        {
            if (IsFirstRoundGroupFromStageGroup(stageGroup, roundGroup)) return true;
        }

        return false;
    }

    private bool StageAndRoundNumberAreLasts(StageGroup stageGroup, RoundGroup roundGroup)
    {
        if (IsLastStageGroup(stageGroup))
        {
            if (IsLastRoundGroupFromStageGroup(stageGroup, roundGroup)) return true;
        }

        return false;
    }

    #endregion

    #region Public Methods

    public bool CurrentStageAndRoundAreValues(int stageNumber, int roundNumber)
    {
        if(currentStageNumber != stageNumber) return false;
        if(currentRoundNumber != roundNumber) return false;

        return true;
    }

    public bool CurrentStageAndRoundAreFirsts() => StageAndRoundNumberAreFirsts(currentStageGroup, currentRoundGroup);
    public bool CurrentRoundIsFirstFromCurrentStage() => IsFirstRoundGroupFromStageGroup(currentStageGroup, currentRoundGroup);
    public bool LastCompletedStageAndRoundNumberAreLasts() => StageAndRoundNumberAreLasts(lastCompletedStageGroup, lastCompletedRoundGroup);
    public bool LastCompletedRoundIsLastFromStage() => IsLastRoundGroupFromStageGroup(lastCompletedStageGroup, lastCompletedRoundGroup);

    public void LoadNextRoundAndStage()
    {
        int previousStageNumber = currentStageNumber;
        int previousRoundNumber = currentRoundNumber;

        StageGroup previousStageGroup = LocateStageGroupByStageNumber(previousStageNumber);
        RoundGroup previousRoundGroup = LocateRoundGroupByRoundNumber(previousStageNumber, previousRoundNumber);

        if (previousStageGroup == null)
        {
            if (debug) Debug.Log("Can not define previousStageGroup. Can not load next Round And Stage.");
            return;
        }

        if (previousRoundGroup == null)
        {
            if (debug) Debug.Log("Cant not define previousRoundGroup. Can not load next Round And Stage.");
            return;
        }

        if (StageAndRoundNumberAreLasts(previousStageGroup, previousRoundGroup))
        {
            if (debug) Debug.Log("Previous Stage and Group are lasts. Can not load next Round and Stage.");
            return;
        }

        int newStageNumber;
        int newRoundNumber;

        if (IsLastRoundGroupFromStageGroup(previousStageGroup, previousRoundGroup))
        {
            newStageNumber = currentStageNumber + 1;
            newRoundNumber = 1;
        }
        else
        {
            newStageNumber = currentStageNumber;
            newRoundNumber = currentRoundNumber + 1;
        }

        //FinalCheck

        StageGroup newStageGroup = LocateStageGroupByStageNumber(newStageNumber);
        RoundGroup newRoundGroup = LocateRoundGroupByRoundNumber(newStageNumber, newRoundNumber);

        if (newStageGroup == null)
        {
            if (debug) Debug.Log("Cant not define newStageGroup. Can not load next Round And Stage.");
            return;
        }

        if (newRoundGroup == null)
        {
            if (debug) Debug.Log("Cant not define newRoundGroup. Can not load next Round And Stage.");
            return;
        }

        SetPreviousStageGroup(previousStageGroup);
        SetPreviousRoundGroup(previousRoundGroup);

        SetCurrentStageGroup(newStageGroup);
        SetCurrentRoundGroup(newRoundGroup);

        SetCurrentStageNumber(newStageNumber);
        SetCurrentRoundNumber(newRoundNumber);

        OnStageAndRoundLoad?.Invoke(this, new OnStageAndRoundLoadEventArgs { previousStageGroup = previousStageGroup, previousRoundGroup = previousRoundGroup, previousStageNumber = previousStageNumber, previousRoundNumber = previousRoundNumber, 
        newStageGroup = newStageGroup, newRoundGroup = newRoundGroup, newStageNumber = newStageNumber, newRoundNumber = newRoundNumber});
    }
    #endregion

    #region Get & Set
    public void SetStartingStageNumber(int setterStartingStageNumber) => startingStageNumber = setterStartingStageNumber;
    public void SetStartingRoundNumber(int setterStartingRoundNumber) => startingRoundNumber = setterStartingRoundNumber;

    private void SetRoundState(RoundState roundState) => this.roundState = roundState;

    private void SetCurrentStageGroup(StageGroup stageGroup) => currentStageGroup = stageGroup;
    private void SetCurrentRoundGroup(RoundGroup roundGroup) => currentRoundGroup = roundGroup;
    private void SetCurrentRound(RoundSO round) => currentRound = round;
    private void ClearCurrentRound() => currentRound = null;

    private void SetPreviousStageGroup(StageGroup stageGroup) => previousStageGroup = stageGroup;
    private void SetPreviousRoundGroup(RoundGroup roundGroup) => previousRoundGroup = roundGroup;

    private void SetCurrentStageNumber(int stageNumber) => currentStageNumber = stageNumber;
    private void SetCurrentRoundNumber(int roundNumber) => currentRoundNumber = roundNumber;

    private void SetLastCompletedStageGroup(StageGroup stageGroup) => lastCompletedStageGroup = stageGroup;
    private void ClearLastCompletedStageGroup() => lastCompletedStageGroup = null;

    private void SetLastCompletedRoundGroup(RoundGroup roundGroup) => lastCompletedRoundGroup = roundGroup;
    private void ClearLastCompletedtRoundGroup() => lastCompletedRoundGroup = null;
    #endregion

    #region StageChange
    public void InitializeToCurrentStage() //Called By Game Manager
    {
        if (currentStageGroup == null)
        {
            if (debug) Debug.Log("Current Stage Group is null. Can not change stage.");
            return;
        }

        RepositionStageGroupToStagePoint(currentStageGroup);
        OnStageInitialized?.Invoke(this, new OnStageChangeEventArgs { stageGroup = currentStageGroup, stageNumber = currentStageNumber });
    }

    public void ChangeToCurrentStage() //Called By Game Manager
    {
        if (!CanChangeStage()) return;

        if (currentStageGroup == null)
        {
            if (debug) Debug.Log("Current Stage Group is null. Can not change stage.");
            return;
        }

        RepositionStageGroupToOriginalStagePoint(previousStageGroup);
        RepositionStageGroupToStagePoint(currentStageGroup);

        OnStageChange?.Invoke(this, new OnStageChangeEventArgs { stageGroup = currentStageGroup, stageNumber = currentStageNumber });        
    }

    private bool CanChangeStage()
    {
        if (roundState != RoundState.NotOnRound) return false;
        return true;
    }

    private void RepositionStageGroupToStagePoint(StageGroup stageGroup) => stageGroup.stageHandler.transform.position = stagePoint.position;
    private void RepositionStageGroupToOriginalStagePoint(StageGroup stageGroup) => stageGroup.stageHandler.transform.position = stageGroup.stageHandler.OriginalStagePoint.position;

    #endregion

    #region Subscriptions
    private void PlayerCharacterManager_OnPlayerCharacterSetPreInstantiation(object sender, PlayerCharacterManager.OnPlayerCharacterEventArgs e)
    {
        SelectCharacterStageGroupList(e.characterSO);
        InitializeStage();
    }

    private void RoundHandler_OnRoundCompleted(object sender, RoundHandler.OnRoundEventArgs e)
    {
        if (currentRound != e.roundSO) return;
        currentRoundEnded = true;
    }
    #endregion
}

[System.Serializable]
public class CharacterStageGroupList
{
    public CharacterSO chartacterSO;
    public List<StageGroup> stageGroups;
}

[System.Serializable]
public class StageGroup
{
    public StageSO stageSO;
    public StageHandler stageHandler;
}