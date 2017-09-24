//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class ProcessCharacters : MonoBehaviour {

    CharacterVision[] _cvs =  null;

    private VisionMode _mode = VisionMode.None;

    // Use this for initialization
    void Start ()
    {
        _cvs = (CharacterVision[])FindObjectsOfType(typeof(CharacterVision));
    }

    internal void Apply(VisionMode mode)
    {
        _mode = mode;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
            _mode = VisionMode.None;
    }

    void OnPreRender()
    {
        if (_cvs != null)
        {
            foreach (CharacterVision cv in _cvs)
            {
                cv.Apply(_mode);
            }
        }
    }
    void OnPostRender()
    {
        if (_cvs != null)
        {
            foreach (CharacterVision cv in _cvs)
            {
                cv.Apply(VisionMode.None);
            }
        }
    }
}
