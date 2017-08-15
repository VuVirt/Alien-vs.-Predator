using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Vision/EMVisionBackground")]


public class EMVisionBackground : MonoBehaviour {

    // Use this for initialization
    public delegate void OnEMVisionBackgroundEnabledDelegate(bool enabled);
    public static event OnEMVisionBackgroundEnabledDelegate onEMVisionBackgroundEnabled;

    public Shader shader;
    private Material m_Material;

    void OnEnable()
    {
        if (onEMVisionBackgroundEnabled != null) onEMVisionBackgroundEnabled(true);
    }

    protected virtual void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
        if (onEMVisionBackgroundEnabled != null) onEMVisionBackgroundEnabled(false);
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
            shader = Shader.Find("Hidden/EMVisionBackgroundShader");

        // Disable the image effect if the shader can't
        // run on the users graphics card
        if (!shader || !shader.isSupported)
            enabled = false;

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
