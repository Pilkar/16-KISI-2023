using Cinemachine;
using System;
using UnityEngine;

public class CameraWithPulse : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer transposer;
    private CinemachineBasicMultiChannelPerlin noise;
    private float pulse;
    private float timer;

    float pulseToDamping()
    {
        float p = Connect.ReadDiff();
        p = Math.Max(0,p);
        return p/90f * 2f;
        
    }

    void Awake()
    {
        //pulse = GameObject.FindObjectOfType<DummyPulse>();

        Camera.main.TryGetComponent<CinemachineBrain>(out var brain);
        if (brain == null)
        {
            Camera.main.gameObject.AddComponent<CinemachineBrain>();
        }
        
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        transposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noise.m_AmplitudeGain = pulseToDamping();
        //noise.m_FrequencyGain = pulseToDamping();
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            noise.m_AmplitudeGain = pulseToDamping();
            //noise.m_FrequencyGain = pulseToDamping();
            timer -= 1;
        }
    }
}
