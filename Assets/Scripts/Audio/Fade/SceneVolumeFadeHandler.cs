using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneVolumeFadeHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField,Range(0.25f,1f)] private float volumeFadeInTime = 0.5f;
    [SerializeField, Range(0.25f, 1f)] private float volumeFadeOutTime = 0.5f;
    [SerializeField] private List<ExceptionTransition> exceptionTransitions;

    protected VolumeFadeManager volumeFadeManager;

    [System.Serializable]
    public class ExceptionTransition
    {
        public string originScene;
        public string targetScene;
    }

    private void OnEnable()
    {
        ScenesManager.OnSceneTransitionOutStart += ScenesManager_OnSceneTransitionOutStart;
        ScenesManager.OnSceneTransitionInStart += ScenesManager_OnSceneTransitionInStart;
    }

    private void OnDisable()
    {
        ScenesManager.OnSceneTransitionOutStart -= ScenesManager_OnSceneTransitionOutStart;
        ScenesManager.OnSceneTransitionInStart -= ScenesManager_OnSceneTransitionInStart;
    }

    protected void SetVolumeFadeManager(VolumeFadeManager volumeFadeManager) => this.volumeFadeManager = volumeFadeManager;

    private bool IsExceptionTransition(string originScene, string targetScene)
    {
        foreach (ExceptionTransition exceptionTransition in exceptionTransitions)
        {
            if (exceptionTransition.targetScene == targetScene && exceptionTransition.originScene == originScene) return true;
        }

        return false;
    }

    #region SceneManager Subscriptions

    private void ScenesManager_OnSceneTransitionInStart(object sender, ScenesManager.OnSceneTransitionLoadEventArgs e)
    {
        if (!volumeFadeManager) return;
        if (IsExceptionTransition(e.originScene, e.targetScene)) return;
        volumeFadeManager.FadeInVolume(volumeFadeInTime);
    }

    private void ScenesManager_OnSceneTransitionOutStart(object sender, ScenesManager.OnSceneTransitionLoadEventArgs e)
    {
        if (!volumeFadeManager) return;
        if (IsExceptionTransition(e.originScene, e.targetScene)) return;
        volumeFadeManager.FadeOutVolume(volumeFadeOutTime);
    }

    #endregion

}
