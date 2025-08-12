using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    public static GlitchController instance;
    [SerializeField] private Material glitchMaterial;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetNoise(0.1f);
        SetGlitchStrength(0.1f);
        SetScanLines(0.8f);
    }

    public void SetNoise(float noiseAmount)
    {
        glitchMaterial.SetFloat("_NoiseAmount", noiseAmount);
    }
    public void SetGlitchStrength( float glitchStrength)
    {
        glitchMaterial.SetFloat("_GlitchStrength", glitchStrength);
    }
    public void SetScanLines( float scanLinesStrength)
    {
        glitchMaterial.SetFloat("_ScanLinesStrength", scanLinesStrength);
    }
}
