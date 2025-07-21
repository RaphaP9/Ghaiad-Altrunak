using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfinerHandler : MonoBehaviour
{
    public static CameraConfinerHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private List<CinemachineVirtualCamera> CMVCAMs;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private Transform storedCameraFollowTransform;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwitchConfiner(CinemachineVirtualCamera CMVCAM, Collider2D confiner)
    {
        CinemachineConfiner2D cinemachineConfiner2D = CMVCAM.transform.GetComponent<CinemachineConfiner2D>();

        if (cinemachineConfiner2D == null) return;

        cinemachineConfiner2D.m_BoundingShape2D = confiner;
        cinemachineConfiner2D.enabled = false; //Force Reinitialization 
        cinemachineConfiner2D.enabled = true;
    }

    public void SwitchConfiner(List<CinemachineVirtualCamera> CMVCAMs, Collider2D confiner)
    {
        foreach(CinemachineVirtualCamera CMVCAM in CMVCAMs)
        {
            SwitchConfiner(CMVCAM, confiner);
        }
    }

    public void EnableConfiner(CinemachineVirtualCamera CMVCAM)
    {
        CinemachineConfiner2D cinemachineConfiner2D = CMVCAM.transform.GetComponent<CinemachineConfiner2D>();
        if (cinemachineConfiner2D == null) return;
        cinemachineConfiner2D.enabled = true;
    }

    public void DisableConfiner(CinemachineVirtualCamera CMVCAM)
    {
        CinemachineConfiner2D cinemachineConfiner2D = CMVCAM.transform.GetComponent<CinemachineConfiner2D>();
        if (cinemachineConfiner2D == null) return;
        cinemachineConfiner2D.enabled = false;
    }

    public void SaveCurrentCameraFollowTransform(CinemachineVirtualCamera CMVCAM) => storedCameraFollowTransform = CMVCAM.Follow;
    public void RemoveCameraFollowTransform(CinemachineVirtualCamera CMVCAM) => CMVCAM.Follow = null;
    public void RecoverCameraFollowTransform(CinemachineVirtualCamera CMVCAM) => CMVCAM.Follow = storedCameraFollowTransform;
}
