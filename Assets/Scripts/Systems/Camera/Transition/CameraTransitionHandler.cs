using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionHandler : MonoBehaviour
{
    public static CameraTransitionHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Transform cameraRefferenceTransform;
    [SerializeField] private CinemachineVirtualCamera CMVCam;

    [Header("States")]
    [SerializeField] private State state;

    [Header("Runtime Filled")]
    [SerializeField] private Transform playerCameraFollowPoint;
    [SerializeField] private Vector3 cameraRefferenceBasePosition;

    public enum State {NotInitialized, FollowingPlayer, StallingIn, MovingIn, LookingTarget, MovingOut, StallingOut}

    public State CameraState => state;

    private const float MOVE_CAMERA_TIME_FACTOR = 0.1f;
    private const float DISTANCE_CAMERA_TIME_FACTOR = 0.08f;

    private const float POSITION_DIFFERENCE_THRESHOLD = 0.2f;
    private const float DISTANCE_DIFFERENCE_THRESHOLD = 0.05f;

    private Transform currentCameraFollowTransform;
    private float previousCameraDistance;

    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionInStart;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionInEnd;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionOutStart;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionOutEnd;

    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionPositionDeterminedPreFollow;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionPositionDeterminedPostFollow;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionPositionDeterminedEnd;

    public class OnCameraTransitionEventArgs : EventArgs
    {
        public CameraTransition cameraTransition;
    }

    private void OnEnable()
    {
        CameraFollowHandler.OnCameraFollowPointSet += CameraFollowHandler_OnCameraFollowPointSet;
    }

    private void OnDisable()
    {
        CameraFollowHandler.OnCameraFollowPointSet -= CameraFollowHandler_OnCameraFollowPointSet;
    }

    private void Awake()
    {
        SetSingleton();
        SetCameraState(State.NotInitialized);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CameraFollowPointHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetCameraState(State state) => this.state = state;
    private void SetCurrentCameraFollowTransform(Transform transform) => currentCameraFollowTransform = transform;
    private void ClearCurrentCameraFollowTransform() => currentCameraFollowTransform = null;
    private void SetPreviousCameraDistance(float distance) => previousCameraDistance = distance;

    public void TransitionMoveCamera(CameraTransition cameraTransition)
    {
        if (state == State.NotInitialized) return;
        if (state == State.MovingIn) return;

        StopAllCoroutines();
        StartCoroutine(TransitionMoveCameraCoroutine(cameraTransition));
    }

    public void EndTransition(CameraTransition cameraTransition)
    {
        if (state == State.MovingOut) return;
        if (state == State.FollowingPlayer) return;

        StopAllCoroutines();
        StartCoroutine(EndTransitionCoroutine(cameraTransition));
    }

    private IEnumerator TransitionMoveCameraCoroutine(CameraTransition cameraTransition)
    {
        OnCameraTransitionInStart?.Invoke(this, new OnCameraTransitionEventArgs { cameraTransition = cameraTransition});

        Transform previousCameraFollowTransform = currentCameraFollowTransform;

        GameObject cameraFollowGameObject = new GameObject("CameraFollowGameObject");
        cameraRefferenceBasePosition = GeneralUtilities.Vector3ToVector2(cameraRefferenceTransform.position);
        cameraFollowGameObject.transform.position = cameraRefferenceBasePosition;

        Transform cameraFollowTransform = cameraFollowGameObject.transform;

        SetCurrentCameraFollowTransform(cameraFollowTransform);
        SetPreviousCameraDistance(CameraOrthoSizeHandler.Instance.OrthoSizeDefault);

        CMVCam.Follow = currentCameraFollowTransform;

        OnCameraTransitionPositionDeterminedPreFollow?.Invoke(this, new OnCameraTransitionEventArgs { cameraTransition = cameraTransition });

        yield return null; //Wait 3 Frames
        yield return null;

        OnCameraTransitionPositionDeterminedPostFollow?.Invoke(this, new OnCameraTransitionEventArgs { cameraTransition = cameraTransition });

        Vector3 startingPositionIn = currentCameraFollowTransform.position;
        Transform targetTransform = cameraTransition.usePlayerFollowPoint ? playerCameraFollowPoint : cameraTransition.targetTransform;

        //If previous CameraFollowTransform wasn't the original playerCameraFollowTransform (Transition started while another transition was happening)
        if(previousCameraFollowTransform != playerCameraFollowPoint) Destroy(previousCameraFollowTransform.gameObject);

        SetCameraState(State.StallingIn);

        yield return new WaitForSeconds(cameraTransition.stallTimeIn);

        SetCameraState(State.MovingIn);

        float time = 0f;
        float positionDifferenceMagnitude = float.MaxValue;
        float distanceDifferenceMagnitude = float.MaxValue;

        while (positionDifferenceMagnitude > POSITION_DIFFERENCE_THRESHOLD || distanceDifferenceMagnitude > DISTANCE_DIFFERENCE_THRESHOLD)
        {
            currentCameraFollowTransform.position = Vector3.Lerp(currentCameraFollowTransform.position, targetTransform.position, time/(cameraTransition.moveInTime) * 1/(MOVE_CAMERA_TIME_FACTOR * cameraTransition.moveInTime) * Time.deltaTime);
            positionDifferenceMagnitude = (currentCameraFollowTransform.position - targetTransform.position).magnitude;

            CameraOrthoSizeHandler.Instance.LerpTowardsTargetDistance(cameraTransition.targetDistance, time / (cameraTransition.moveInTime) * 1 / (DISTANCE_CAMERA_TIME_FACTOR * cameraTransition.moveInTime));
            distanceDifferenceMagnitude = Math.Abs(CameraOrthoSizeHandler.Instance.Distance - cameraTransition.targetDistance);

            time += Time.deltaTime;
            yield return null;
        }

        currentCameraFollowTransform.position = targetTransform.position;

        OnCameraTransitionInEnd?.Invoke(this, new OnCameraTransitionEventArgs {cameraTransition = cameraTransition});

        SetCameraState(State.LookingTarget);

        if (!cameraTransition.endInTime) yield break;
        
        yield return new WaitForSeconds(cameraTransition.stallTime);

        yield return StartCoroutine(EndTransitionCoroutine(cameraTransition));
    }

    private IEnumerator EndTransitionCoroutine(CameraTransition cameraTransition)
    {
        if (state == State.MovingOut) yield break;
        if (state == State.FollowingPlayer) yield break;

        SetCameraState(State.MovingOut);

        OnCameraTransitionOutStart?.Invoke(this, new OnCameraTransitionEventArgs {cameraTransition = cameraTransition});

        float time = 0f;
        float positionDifferenceMagnitude = float.MaxValue;
        float distanceDifferenceMagnitude = float.MaxValue;

        while (positionDifferenceMagnitude > POSITION_DIFFERENCE_THRESHOLD || distanceDifferenceMagnitude > DISTANCE_DIFFERENCE_THRESHOLD)
        {
            currentCameraFollowTransform.position = Vector3.Lerp(currentCameraFollowTransform.position, cameraRefferenceBasePosition, time / (cameraTransition.moveOutTime) * 1 / (MOVE_CAMERA_TIME_FACTOR * cameraTransition.moveOutTime) * Time.deltaTime);
            positionDifferenceMagnitude = (currentCameraFollowTransform.position - cameraRefferenceBasePosition).magnitude;

            CameraOrthoSizeHandler.Instance.LerpTowardsTargetDistance(previousCameraDistance, time / (cameraTransition.moveOutTime) * 1 / (DISTANCE_CAMERA_TIME_FACTOR * cameraTransition.moveOutTime));
            distanceDifferenceMagnitude = Math.Abs(CameraOrthoSizeHandler.Instance.Distance - previousCameraDistance);

            time += Time.deltaTime;
            yield return null;
        }

        SetCameraState(State.StallingOut);

        time = 0f;

        while (time <= cameraTransition.stallTimeOut) //To rectify position & camera distance during stallTimeOut
        {
            currentCameraFollowTransform.position = cameraRefferenceBasePosition;
            CameraOrthoSizeHandler.Instance.LerpTowardsTargetDistance(previousCameraDistance, time / (cameraTransition.stallTimeOut) * 1 / (DISTANCE_CAMERA_TIME_FACTOR * cameraTransition.stallTimeOut));

            time += Time.deltaTime;
            yield return null;
        }

        currentCameraFollowTransform.position = playerCameraFollowPoint.position;
        CMVCam.Follow = playerCameraFollowPoint;

        Destroy(currentCameraFollowTransform.gameObject);
        SetCurrentCameraFollowTransform(playerCameraFollowPoint);

        OnCameraTransitionPositionDeterminedEnd?.Invoke(this, new OnCameraTransitionEventArgs { cameraTransition = cameraTransition });

        SetCameraState(State.FollowingPlayer);

        OnCameraTransitionOutEnd?.Invoke(this, new OnCameraTransitionEventArgs {cameraTransition = cameraTransition});
    }

    #region Subscriptions

    private void CameraFollowHandler_OnCameraFollowPointSet(object sender, CameraFollowHandler.OnCameraFollowPointEventArgs e)
    {
        playerCameraFollowPoint = e.cameraFollowPoint;

        SetCurrentCameraFollowTransform(e.cameraFollowPoint);
        SetCameraState(State.FollowingPlayer);
    }

    #endregion
}

[Serializable]
public class CameraTransition
{
    public int id;
    public string logToStart;
    public string logToEnd;
    [Space]
    public bool usePlayerFollowPoint;
    public Transform targetTransform;
    [Space]
    [Range(0f, 4f)] public float stallTimeIn;
    [Range(0.5f, 4f)] public float moveInTime;
    [Range(0.5f, 10f)] public float stallTime;
    [Range(0.5f, 4f)] public float moveOutTime;
    [Range(0.01f, 4f)] public float stallTimeOut;
    [Range(2.5f, 10f)] public float targetDistance;
    [Space]
    public bool endInTime;
    [Space]
    public bool enabled;
}