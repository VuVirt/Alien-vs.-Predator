using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightProcessCharacters : MonoBehaviour {

    CharacterVision[] _cvs = null;

    // Use this for initialization
    void Start()
    {
        _cvs = (CharacterVision[])FindObjectsOfType(typeof(CharacterVision));
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnPreRender()
    {
        if (_cvs != null)
        {
            foreach (CharacterVision cv in _cvs)
            {
                cv.Apply(VisionMode.Night);
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
