using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Vision/EMVision")]

public class EMVision : MonoBehaviour {

    // Use this for initialization
    public delegate void OnEMVisionEnabledDelegate(bool enabled);
    public static event OnEMVisionEnabledDelegate onEMVisionEnabled;

    public Shader shader;
    private Material m_Material;

    [Range(0.0f, 2.0f)] public float Coeff = 0.5f;
    public Color Low = new Color(0.01f, 0.65f, 0.0f);
    public Color High = new Color(0.25f, 0.5f, 0.25f);

    void OnEnable()
    {
        if (onEMVisionEnabled != null) onEMVisionEnabled(true);
    }

    protected virtual void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
        if (onEMVisionEnabled != null) onEMVisionEnabled(false);
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
            shader = Shader.Find("Hidden/EMVisionShader");

        // Disable the image effect if the shader can't
        // run on the users graphics card
        if (!shader || !shader.isSupported)
            enabled = false;

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Coeff", Coeff);
        material.SetColor("_Low", Low);
        material.SetColor("_High", High);
        Graphics.Blit(source, destination, material);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
