using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    public static GlitchController instance;
    [SerializeField] private Material glitchMaterial;
    public float noiseAmount, glitchStrength, scanLinesStrength;
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
        
    }

    public void GlitchOn()
    {
        glitchMaterial.SetFloat("NoiseAmount", noiseAmount);
        glitchMaterial.SetFloat("GlitchStrength", glitchStrength);
        glitchMaterial.SetFloat("ScanLinesStrength", scanLinesStrength);
    }
}
