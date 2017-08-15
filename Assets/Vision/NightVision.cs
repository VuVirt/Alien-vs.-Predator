using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Vision/NightVision")]

public class NightVision : MonoBehaviour {

    // Use this for initialization
    public delegate void OnNightVisionEnabledDelegate(bool enabled);
    public static event OnNightVisionEnabledDelegate onNightVisionEnabled;

    public Shader shader;
    private Material m_Material;

    void OnEnable()
    {
        if (onNightVisionEnabled != null) onNightVisionEnabled(true);
    }

    protected virtual void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
        if (onNightVisionEnabled != null) onNightVisionEnabled(false);
    }


    void Start () {
        // Disable if we don't support image effects
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
        if (shader == null)
            shader = Shader.Find("Hidden/NightVisionShader");

        // Disable the image effect if the shader can't
        // run on the users graphics card
        if (!shader || !shader.isSupported)
            enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
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

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
