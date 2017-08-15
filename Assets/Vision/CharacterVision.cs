using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Vision/CharacterVision")]

internal enum VisionMode
{
    None,
    Thermal, 
    Night,
    EM
}
public class CharacterVision : MonoBehaviour {
    // public string materialEmissionProperty = "_EmissionColor";

    [Range(0.0f, 2.0f)] public float ThermalEmission = 1.0f;
    [Range(0.0f, 2.0f)] public float NightEmission = 1.0f;
    [Range(0.0f, 2.0f)] public float EMEmission = 1.0f;

    void Start()
    {
        Apply(VisionMode.None);
    }

    void Update()
    {
    }

    internal void Apply(VisionMode mode)
    {
        Renderer[] rn = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rn)
        {
            switch(mode)
            {
                case VisionMode.Thermal:
                    r.sharedMaterial.SetColor("_EmissionColor", Color.white * ThermalEmission);
                    break;

                case VisionMode.Night:
                    r.sharedMaterial.SetColor("_EmissionColor", Color.white * NightEmission);
                    break;

                case VisionMode.EM:
                    r.sharedMaterial.SetColor("_EmissionColor", Color.white * EMEmission);
                    break;

                default:
                    r.sharedMaterial.SetColor("_EmissionColor", Color.black);
                    break;
            }
        }
    }
}
