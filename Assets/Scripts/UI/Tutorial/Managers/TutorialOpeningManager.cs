using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialOpeningManager : MonoBehaviour
{
    public static TutorialOpeningManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private TutorializedActionUI currentActiveTutorializedActionUI;

    public static event EventHandler<OnTutorializedActionEventArgs> OnTutorializedActionOpen;
    public static event EventHandler<OnTutorializedActionEventArgs> OnCurrentTutorializedActionClosed;

    public static event EventHandler<OnTutorializedActionEventArgs> OnTutorializedActionClosed;

    public static event EventHandler OnEveryTutorializedActionClose;

    public TutorializedActionUI CurrentActiveTutorializedActionUI => currentActiveTutorializedActionUI;

    public class OnTutorializedActionEventArgs : EventArgs
    {
        public TutorializedAction tutorializedAction;
    }

    private void OnEnable()
    {
        TutorializedActionUI.OnTutorializedActionUIOpen += TutorializedActionUI_OnTutorializedActionUIOpen;
        TutorializedActionUI.OnTutorializedActionUIClose += TutorializedActionUI_OnTutorializedActionUIClose;
    }

    private void OnDisable()
    {
        TutorializedActionUI.OnTutorializedActionUIOpen -= TutorializedActionUI_OnTutorializedActionUIOpen;
        TutorializedActionUI.OnTutorializedActionUIClose -= TutorializedActionUI_OnTutorializedActionUIClose;
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
        }
        else
        {
            Debug.LogWarning("There is more than one TutorialOpeningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Get & Set
    private void ClearCurrentActiveTutorializedActionUI() => currentActiveTutorializedActionUI = null;
    private void SetCurrentActiveTutorializedActionUI(TutorializedActionUI tutorializedActionUI) => currentActiveTutorializedActionUI= tutorializedActionUI;
    #endregion

    public void OpenTutorializedAction(TutorializedAction tutorializedAction)
    {
        OnTutorializedActionOpen?.Invoke(this, new OnTutorializedActionEventArgs { tutorializedAction = tutorializedAction });
    }

    public void CloseTutorializedAction(TutorializedAction tutorializedAction)
    {
        OnCurrentTutorializedActionClosed?.Invoke(this, new OnTutorializedActionEventArgs { tutorializedAction = tutorializedAction });
    }

    public void CloseEveryTutorializedAction()
    {
        OnEveryTutorializedActionClose?.Invoke(this, EventArgs.Empty);
    }

    #region Subscriptions
    private void TutorializedActionUI_OnTutorializedActionUIOpen(object sender, TutorializedActionUI.OnTutorializedActionEventArgs e)
    {
        SetCurrentActiveTutorializedActionUI(e.tutorializedActionUI);
    }
    private void TutorializedActionUI_OnTutorializedActionUIClose(object sender, TutorializedActionUI.OnTutorializedActionEventArgs e)
    {
        if(currentActiveTutorializedActionUI == e.tutorializedActionUI)
        {
            OnCurrentTutorializedActionClosed?.Invoke(this, new OnTutorializedActionEventArgs { tutorializedAction = e.tutorializedActionUI.GetTutorializedAction() });
            OnTutorializedActionClosed?.Invoke(this, new OnTutorializedActionEventArgs { tutorializedAction = e.tutorializedActionUI.GetTutorializedAction() });
            ClearCurrentActiveTutorializedActionUI();
        }
        else
        {
            OnTutorializedActionClosed?.Invoke(this, new OnTutorializedActionEventArgs { tutorializedAction = e.tutorializedActionUI.GetTutorializedAction() });
            ClearCurrentActiveTutorializedActionUI();
        }
    }

    #endregion
}
