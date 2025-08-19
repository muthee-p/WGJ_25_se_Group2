using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noise;

    float frequency;
    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    void Update()
    {
        if (noise != null)
        {
            noise.m_FrequencyGain = frequency;
        }
    }

    // Optional: public function to trigger shakes
    public void SetNoise(float freq, float duration = 0f)
    {
        frequency = freq;

        if (duration > 0f)
            StartCoroutine(ResetAfter(duration));
    }

    IEnumerator ResetAfter(float time)
    {
        yield return new WaitForSeconds(time);

        frequency = 0f;
    }


}

