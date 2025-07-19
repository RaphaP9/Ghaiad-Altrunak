using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfinerHandler : MonoBehaviour
{
    public static CameraConfinerHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;
    [SerializeField] private CinemachineVirtualCamera CMVCAM;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 originalDamping;

    private void OnEnable()
    {
        CameraTransitionHandler.OnCameraTransitionPositionDeterminedPreFollow += CameraTransitionHandler_OnCameraTransitionPositionDeterminedPreFollow;
        CameraTransitionHandler.OnCameraTransitionPositionDeterminedEnd += CameraTransitionHandler_OnCameraTransitionPositionDeterminedEnd;
    }

    private void OnDisable()
    {
        CameraTransitionHandler.OnCameraTransitionPositionDeterminedPreFollow -= CameraTransitionHandler_OnCameraTransitionPositionDeterminedPreFollow;
        CameraTransitionHandler.OnCameraTransitionPositionDeterminedEnd -= CameraTransitionHandler_OnCameraTransitionPositionDeterminedEnd;
    }

    private void Awake()
    {
        SetSingleton();
        cinemachineTransposer = CMVCAM.GetCinemachineComponent<CinemachineTransposer>();
        InitializeDamping();
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

    private void InitializeDamping()
    {
        originalDamping = new Vector3(cinemachineTransposer.m_XDamping, cinemachineTransposer.m_YDamping, cinemachineTransposer.m_ZDamping);
    }

    private void SetDamping(Vector3 damping)
    {
        cinemachineTransposer.m_XDamping = damping.x;
        cinemachineTransposer.m_YDamping = damping.y;
        cinemachineTransposer.m_ZDamping = damping.z;
    }

    public IEnumerator SmoothSwitchConfinerCoroutine(PolygonCollider2D newConfiner, float switchTime)
    {
        DisableConfiner();

        yield return new WaitForSeconds(switchTime);

        SwitchConfiner(newConfiner);
    }

    private void SwitchConfiner(PolygonCollider2D confiner)
    {
        cinemachineConfiner2D.m_BoundingShape2D = confiner;
        cinemachineConfiner2D.enabled = false; //Force Reinitialization 
        cinemachineConfiner2D.enabled = true;
    }

    private void EnableConfiner() => cinemachineConfiner2D.enabled = true;
    private void DisableConfiner() => cinemachineConfiner2D.enabled = false;

    private void CameraTransitionHandler_OnCameraTransitionPositionDeterminedPreFollow(object sender, CameraTransitionHandler.OnCameraTransitionEventArgs e)
    {
        DisableConfiner();
    }

    private void CameraTransitionHandler_OnCameraTransitionPositionDeterminedEnd(object sender, CameraTransitionHandler.OnCameraTransitionEventArgs e)
    {
        EnableConfiner();
    }
}
