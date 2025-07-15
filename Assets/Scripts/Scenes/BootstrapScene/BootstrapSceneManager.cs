using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BootstrapSceneManager : MonoBehaviour
{
    public static BootstrapSceneManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField, Range(2f, 5f)] private float timeToLoadData;
    [SerializeField, Range(2f, 5f)] private float timeToWaitAfterLoad;
    [Space]
    [SerializeField] private string nextScene;
    [SerializeField] private TransitionType nextSceneTransitionType;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        StartCoroutine(BootstrapSceneCoroutine());
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator BootstrapSceneCoroutine()
    {
        yield return new WaitForSeconds(timeToLoadData); 

        yield return WaitForDataLoad();

        yield return new WaitForSeconds(timeToWaitAfterLoad); 

        ScenesManager.Instance.TransitionLoadTargetScene(nextScene, nextSceneTransitionType);
    }

    private IEnumerator WaitForDataLoad() //Async Method!
    {
        Task loadTask = GeneralDataManager.Instance.CompleteDataLoadAsync();

        while (!loadTask.IsCompleted)
        {
            yield return null;
        }
    }
}
