using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;

    public enum State { Idle, TransitionOut, MiddleTransition, TransitionIn }

    public State SceneState => state;

    [Header("Settings")]
    [SerializeField] private TransitionType openingTransitionType;
    [SerializeField, Range (0.05f, 0.5f)] private float transitionInInterval; //Value > 0 to avoid stutter on scene transition

    public static event EventHandler<OnSceneTransitionLoadEventArgs> OnSceneTransitionOutStart;
    public static event EventHandler<OnSceneTransitionLoadEventArgs> OnSceneTransitionInStart;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneLoadStart;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneLoadCompleteReal;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneLoadComplete;

    public float loadProgress;
    public bool isLoadingScene;

    public float LoadProgress => loadProgress;
    public bool IsLoadingScene => isLoadingScene;

    private string sceneToLoad;

    private const float SCENE_READY_PERCENT = 0.9f;
    private const float SCENE_READY_PAUSE_TIME = 0.5f;

    public class OnSceneLoadEventArgs : EventArgs
    {
        public string originScene;
        public string targetScene;
    }

    public class OnSceneTransitionLoadEventArgs : EventArgs
    {
        public string originScene;
        public string targetScene;
        public TransitionType transitionType;
    }

    private void OnEnable()
    {
        SceneTransitionUIHandler.OnTransitionOutEnd += TransitionUIHandler_OnTransitionOutEnd;
        SceneTransitionUIHandler.OnTransitionInEnd += TransitionUIHandler_OnTransitionInEnd;
    }

    private void OnDisable()
    {
        SceneTransitionUIHandler.OnTransitionOutEnd -= TransitionUIHandler_OnTransitionOutEnd;
        SceneTransitionUIHandler.OnTransitionInEnd -= TransitionUIHandler_OnTransitionInEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeVariables();
        SimulateTransitionIn(openingTransitionType);
    }

    private void InitializeVariables()
    {
        isLoadingScene = false;
        loadProgress = 0f;
    }

    private void SimulateTransitionIn(TransitionType transitionType)
    {
        //Simulate the load of current scene, only to TransitionIn the first scene played
        OnSceneLoadComplete?.Invoke(this, new OnSceneLoadEventArgs { originScene = "", targetScene = SceneManager.GetActiveScene().name });
        OnSceneTransitionInStart.Invoke(this, new OnSceneTransitionLoadEventArgs { originScene = "", targetScene = SceneManager.GetActiveScene().name, transitionType = transitionType});
        SetSceneState(State.TransitionIn);
    }

    private void SimulateRegularLoad()
    {
        OnSceneLoadComplete?.Invoke(this, new OnSceneLoadEventArgs { originScene = "", targetScene = SceneManager.GetActiveScene().name });
        SetSceneState(State.Idle);
    }

    private void SetSceneState(State state) => this.state = state;
    private bool CanChangeScene() => state == State.Idle;
    private void SetSceneToLoad(string sceneToLoad) => this.sceneToLoad = sceneToLoad;
    private void ClearSceneToLoad() => sceneToLoad = "";

    private void ResetTimeScale() => Time.timeScale = 1f; //Ex. for transitioning to MainMenu from the Gameplay Pause Menu

    #region SimpleLoad
    public void SimpleReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SimpleLoadTargetScene(currentSceneName);
    }

    public void SimpleLoadTargetScene(string targetScene)
    {
        if (!CanChangeScene()) return;
        StartCoroutine(LoadSceneAsyncCoroutine(targetScene));
    }
    #endregion

    #region TransitionLoad
    public void TransitionReloadCurrentScene(TransitionType transitionType)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        TransitionLoadTargetScene(currentSceneName, transitionType);
    }

    public void TransitionLoadTargetScene(string targetScene, TransitionType transitionType) //Transition Out Method
    {
        if (!CanChangeScene()) return;

        string originScene = SceneManager.GetActiveScene().name;

        SetSceneState(State.TransitionOut);
        OnSceneTransitionOutStart?.Invoke(this, new OnSceneTransitionLoadEventArgs { originScene = originScene, targetScene = targetScene, transitionType = transitionType });
        SetSceneToLoad(targetScene);
    }

    private IEnumerator LoadSceneTransitionInCoroutine(string targetScene, TransitionType transitionType) //Transition In Coroutine
    {
        string originScene = SceneManager.GetActiveScene().name;

        yield return StartCoroutine(LoadSceneAsyncCoroutine(targetScene)); //Async Scene Load happens here
        yield return new WaitForSecondsRealtime(transitionInInterval);

        OnSceneTransitionInStart?.Invoke(this, new OnSceneTransitionLoadEventArgs { originScene = originScene, targetScene = targetScene, transitionType = transitionType });

        SetSceneState(State.TransitionIn);
    }

    #endregion

    #region LoadScene

    public void LoadScene(string targetScene) //No Async
    {
        string originScene = SceneManager.GetActiveScene().name;

        SceneManager.LoadSceneAsync(targetScene);
        OnSceneLoadComplete?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });

        ResetTimeScale();
    }

    private IEnumerator LoadSceneAsyncCoroutine(string targetScene)
    {
        //VERY IMPORTANT: If for some reason, the scene fails to load(Ex. Not added to build settings), is LoadingScene never becomes false. Resulting in no further scene loads.
        //In that context, the line "if(isLoadingScene) yield break;" is commented.
        //if(isLoadingScene) yield break;

        isLoadingScene = true;

        string originScene = SceneManager.GetActiveScene().name;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);

        asyncLoad.allowSceneActivation = false;
        loadProgress = 0f;

        OnSceneLoadStart?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });

        while (!asyncLoad.isDone)
        {
            loadProgress = Mathf.Clamp01(asyncLoad.progress / SCENE_READY_PERCENT);

            if(asyncLoad.progress >= SCENE_READY_PERCENT)
            {
                OnSceneLoadCompleteReal?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });

                yield return new WaitForSecondsRealtime(SCENE_READY_PAUSE_TIME);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        OnSceneLoadComplete?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });

        ResetTimeScale();

        isLoadingScene = false;
    }

    #endregion

    public void QuitGame() => Application.Quit();

    #region TransitionUIHandler Subscriptions
    private void TransitionUIHandler_OnTransitionOutEnd(object sender, SceneTransitionUIHandler.OnTransitionUIEventArgs e)
    {
        SetSceneState(State.MiddleTransition);
        StartCoroutine(LoadSceneTransitionInCoroutine(sceneToLoad, e.transitionType));
    }
    private void TransitionUIHandler_OnTransitionInEnd(object sender, SceneTransitionUIHandler.OnTransitionUIEventArgs e)
    {
        ClearSceneToLoad();
        SetSceneState(State.Idle);
    }
    #endregion

}
