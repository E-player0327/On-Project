using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance { get; private set; } 

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private float shakeTimer;
    private float intensity;
    private float time;
    private bool isSmoothShake = false;

    private void Awake()
    {
        instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void shakeCamera(float _intensity, float _time)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _intensity;
        shakeTimer = time;
        isSmoothShake = false;
    }

    public void smoothShakeCamera(float _intensity, float _time)
    {
        intensity = _intensity;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _intensity;
        shakeTimer = _time;
        time = _time;
        isSmoothShake = true;
    }

    private void FixedUpdate()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.fixedDeltaTime;
            if(isSmoothShake)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = cinemachineBasicMultiChannelPerlin.m_AmplitudeGain - intensity / (50 * time) ;
            }
        }
        if(shakeTimer <= 0)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            intensity = 0;
            time = 0;
            isSmoothShake = false;
        }
    }
}
