using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeHandler : MonoBehaviour
{
    public static CameraShakeHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [Header("Settings")]
    [SerializeField] private ShakeReplacementCondition shakeReplacementCondition;

    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private float currentShakeAmplitude;

    private enum ShakeReplacementCondition { AnyShake, OnlyGreaterAmplitudes, WaitForCurrentShakeEnd}

    private void Awake()
    {
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        SetSingleton();
    }

    private void Start()
    {
        SetCurrentShakeAmplitude(0f);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CameraShakeHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void ShakeCamera(CameraShake cameraShake)
    {
        if (shakeReplacementCondition == ShakeReplacementCondition.WaitForCurrentShakeEnd && currentShakeAmplitude != 0) return;
        if (shakeReplacementCondition == ShakeReplacementCondition.OnlyGreaterAmplitudes && currentShakeAmplitude > cameraShake.amplitude) return;

        StopAllCoroutines();
        StartCoroutine(ShakeCameraCoroutine(cameraShake));
    }

    private IEnumerator ShakeCameraCoroutine(CameraShake cameraShake)
    {
        SetCurrentShakeAmplitude(cameraShake.amplitude);

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;

        float time = 0f;

        while (time <= cameraShake.shakeFadeInTime)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(0f, cameraShake.amplitude, time / cameraShake.shakeFadeInTime);
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(0f, cameraShake.frequency, time / cameraShake.shakeFadeInTime);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = cameraShake.amplitude;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = cameraShake.frequency;

        yield return new WaitForSecondsRealtime(cameraShake.shakeTime);

        time = 0f;

        while (time <= cameraShake.shakeFadeOutTime)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(cameraShake.amplitude, 0f, time / cameraShake.shakeFadeOutTime);
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(cameraShake.frequency, 0f, time / cameraShake.shakeFadeOutTime);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;

        ClearCurrentShakeAmplitude();
    }

    private void SetCurrentShakeAmplitude(float value) => currentShakeAmplitude = value;
    private void ClearCurrentShakeAmplitude() => currentShakeAmplitude = 0f;
}

[Serializable]
public class CameraShake
{
    public int id;
    public string logToShake;
    [Range(0, 10f)] public float amplitude;
    [Range(0, 5f)] public float frequency;
    [Range(0.1f, 10f)] public float shakeTime;
    [Range(0f, 10f)] public float shakeFadeInTime;
    [Range(0f, 10f)] public float shakeFadeOutTime;
    [Space]
    public bool enabled;
}