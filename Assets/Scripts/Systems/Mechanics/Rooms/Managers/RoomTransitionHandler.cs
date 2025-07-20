using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class RoomTransitionHandler : MonoBehaviour
{
    public static RoomTransitionHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera mainCMVCAM;
    [SerializeField] private CinemachineVirtualCamera auxCMVCAM;

    [Header("Settings")]
    [SerializeField, Range(0.1f, 2f)] private float roomTransitionTime;

    [Header("Runtime Filled")]
    [SerializeField] private bool transitioningToRoom;
    public bool TransitioningToRoom => transitioningToRoom;
    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        transitioningToRoom = false;
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

    public void TransitionToRoom(RoomHandler previousRoom, RoomHandler nextRoom, Transform targetTransformToPosition)
    {
        if (transitioningToRoom) return;
        if (previousRoom == nextRoom) return;

        StartCoroutine(TransitionToRoomCoroutineCameraSwitch(previousRoom, nextRoom, targetTransformToPosition));   
    }

    //Depends on CinemachineConfiner2D Damping
    private IEnumerator TransitionToRoomCoroutineConfinerSwitch(RoomHandler previousRoom, RoomHandler nextRoom, Transform targetTransformToPosition)
    {
        transitioningToRoom = true;

        CameraConfinerHandler.Instance.SaveCurrentCameraFollowTransform();
        CameraConfinerHandler.Instance.RemoveCameraFollowTransform();

        CameraConfinerHandler.Instance.DisableConfiner();
        PlayerTeleporterManager.Instance.InstantPositionPlayer(GeneralUtilities.TransformPositionVector2(targetTransformToPosition));

        CameraConfinerHandler.Instance.SwitchConfiner(nextRoom.RoomConfiner);
        CameraConfinerHandler.Instance.RecoverCameraFollowTransform();

        CinemachineCore.Instance.GetActiveBrain(0).ManualUpdate();

        yield return new WaitForSeconds(roomTransitionTime);

        transitioningToRoom = false;
    }

    //Depends on CinemachineBrain Blend Settings
    private IEnumerator TransitionToRoomCoroutineCameraSwitch(RoomHandler previousRoom, RoomHandler nextRoom, Transform targetTransformToPosition)
    {
        transitioningToRoom = true;

        CameraConfinerHandler.Instance.SwitchConfiner(auxCMVCAM, nextRoom.RoomConfiner);
        PlayerTeleporterManager.Instance.InstantPositionPlayer(GeneralUtilities.TransformPositionVector2(targetTransformToPosition));

        auxCMVCAM.enabled = true;
        mainCMVCAM.enabled = false;

        yield return new WaitForSeconds(roomTransitionTime);

        CameraConfinerHandler.Instance.SwitchConfiner(mainCMVCAM, nextRoom.RoomConfiner);

        auxCMVCAM.enabled = false;
        mainCMVCAM.enabled = true;

        transitioningToRoom = false;
    }
}
