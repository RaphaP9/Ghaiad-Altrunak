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
    private Transform cameraFollowTransform;

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

    public void SetDamping(Vector3 damping)
    {
        cinemachineTransposer.m_XDamping = damping.x;
        cinemachineTransposer.m_YDamping = damping.y;
        cinemachineTransposer.m_ZDamping = damping.z;
    }

    public void DisableDamping() => SetDamping(Vector3.zero);
    public void RecoverOriginalDamping() => SetDamping(originalDamping);

    public IEnumerator SmoothSwitchConfinerCoroutine(PolygonCollider2D newConfiner, float switchTime)
    {
        SaveCurrentCameraFollowTransform();
        RemoveCameraFollowTransform();

        DisableConfiner();

        yield return new WaitForSeconds(switchTime);

        SetCameraFollowTransform(cameraFollowTransform);
        SwitchConfiner(newConfiner);
    }

    public void SwitchConfiner(Collider2D confiner)
    {
        cinemachineConfiner2D.m_BoundingShape2D = confiner;
        cinemachineConfiner2D.enabled = false; //Force Reinitialization 
        cinemachineConfiner2D.enabled = true;
    }

    public void EnableConfiner() => cinemachineConfiner2D.enabled = true;
    public void DisableConfiner() => cinemachineConfiner2D.enabled = false;

    public void SaveCurrentCameraFollowTransform() => cameraFollowTransform = CMVCAM.Follow;
    public void RemoveCameraFollowTransform() => CMVCAM.Follow = null;
    public void RecoverCameraFollowTransform() => CMVCAM.Follow = cameraFollowTransform;
    public void SetCameraFollowTransform(Transform followTransform) => CMVCAM.Follow = followTransform;
}
