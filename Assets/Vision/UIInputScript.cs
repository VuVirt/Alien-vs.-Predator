//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputScript : MonoBehaviour
{
    ProcessCharacters[] processCharacters;
    NightVision nightVision;
    ThermalVisionBackground thermalVisionBg;
    EMVisionBackground emVisionBg;

    ThermalVision thermalVision;
    EMVision emVision;

    //bool thumbnails = false;
    Camera camTH;
    Camera camTHC;
    Camera camNV;
    Camera camNVC;
    Camera camEM;
    Camera camEMC;

    // Use this for initialization
    void Start ()
    {
        //if (SystemInfo.deviceType == DeviceType.Desktop)
        //{
        //    GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        //}

        processCharacters = Camera.main.gameObject.GetComponentsInChildren<ProcessCharacters>();

        nightVision = Camera.main.gameObject.GetComponentInChildren<NightVision>();
        thermalVisionBg = Camera.main.gameObject.GetComponentInChildren<ThermalVisionBackground>();
        emVisionBg = Camera.main.gameObject.GetComponentInChildren<EMVisionBackground>();

        Camera camChar = GameObject.Find("Camera").GetComponent<Camera>();
        thermalVision = camChar.gameObject.GetComponentInChildren<ThermalVision>();
        emVision = camChar.gameObject.GetComponentInChildren<EMVision>();

        camTH = GameObject.Find("ThermalCam").GetComponent<Camera>();
        camTH.enabled = false;
        camTHC = GameObject.Find("ThermalCamChar").GetComponent<Camera>();
        camTHC.enabled = false;
        camNV = GameObject.Find("NVCam").GetComponent<Camera>();
        camNV.enabled = false;
        camNVC = GameObject.Find("NVCamChar").GetComponent<Camera>();
        camNVC.enabled = false;
        camEM = GameObject.Find("EMCam").GetComponent<Camera>();
        camEM.enabled = false;
        camEMC = GameObject.Find("EMCamChar").GetComponent<Camera>();
        camEMC.enabled = false;
    }

    // Update is called once per frame
    void Update ()
    {
        //bool temp = thumbnails;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            nightVision.enabled = false;
            thermalVisionBg.enabled = false;
            thermalVision.enabled = false;
            emVision.enabled = false;
            emVisionBg.enabled = false;
            foreach(ProcessCharacters pc in processCharacters)
                pc.Apply(VisionMode.None);
            //thumbnails = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            nightVision.enabled = false;
            thermalVisionBg.enabled = true;
            thermalVision.enabled = true;
            emVision.enabled = false;
            emVisionBg.enabled = false;
            foreach (ProcessCharacters pc in processCharacters)
                pc.Apply(VisionMode.Thermal);
            //thumbnails = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            nightVision.enabled = true;
            thermalVisionBg.enabled = false;
            thermalVision.enabled = false;
            emVision.enabled = false;
            emVisionBg.enabled = false;
            foreach (ProcessCharacters pc in processCharacters)
                pc.Apply(VisionMode.Night);
            //thumbnails = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            nightVision.enabled = false;
            thermalVisionBg.enabled = false;
            thermalVision.enabled = false;
            emVision.enabled = true;
            emVisionBg.enabled = true;
            foreach (ProcessCharacters pc in processCharacters)
                pc.Apply(VisionMode.EM);
            //thumbnails = false;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            //nightVision.enabled = false;
            //thumbnails = !thumbnails;

            camTH.enabled = !camTH.enabled;
            camTHC.enabled = !camTHC.enabled;
            camNV.enabled = !camNV.enabled;
            camNVC.enabled = !camNVC.enabled;
            camEM.enabled = !camEM.enabled;
            camEMC.enabled = !camEMC.enabled;
        }

        //if (temp != thumbnails)
        //{
        //    camTH.enabled = thumbnails;
        //    camTHC.enabled = thumbnails;
        //    camNV.enabled = thumbnails;
        //    camNVC.enabled = thumbnails;
        //    camEM.enabled = thumbnails;
        //    camEMC.enabled = thumbnails;
        //}
    }
}
