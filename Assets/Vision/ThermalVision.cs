using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Vision/ThermalVision")]

public class ThermalVision : MonoBehaviour
{

    // Use this for initialization
    public delegate void OnThermalVisionEnabledDelegate(bool enabled);
    public static event OnThermalVisionEnabledDelegate onThermalVisionEnabled;

    [Range(0.0f, 2.0f)] public float Coeff = 0.5f;
    public Color Low = new Color(0, 0, 0.05f);
    public Color Medium = new Color(1, 0, 0);
    public Color High = new Color(1, 1, 0);

    public Shader shader;
    private Material m_Material;

    void OnEnable()
    {
        if (onThermalVisionEnabled != null) onThermalVisionEnabled(true);
    }

    protected virtual void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
        if (onThermalVisionEnabled != null) onThermalVisionEnabled(false);
    }

    protected Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(shader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }
            return m_Material;
        }
    }

    void Start()
    {
        // Disable if we don't support image effects
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
        if (shader == null)
            shader = Shader.Find("Hidden/ThermalVisionShader");

        // Disable the image effect if the shader can't
        // run on the users graphics card
        if (!shader || !shader.isSupported)
            enabled = false;

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Coeff", Coeff);
        material.SetColor("_Low", Low);
        material.SetColor("_Medium", Medium);
        material.SetColor("_High", High);
        Graphics.Blit(source, destination, material);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
