using UnityEngine;
//using UnityEngine.Networking;
using UnityEngine.UI;

public class FreeCamera : MonoBehaviour
{
	//public bool enableInputCapture = true;
	//public bool holdRightMouseCapture = false;

	public float lookSpeed = 5f;
	public float moveSpeed = 5f;
	public float sprintSpeed = 50f;

	bool	m_inputCaptured;
	float	m_yaw;
	float	m_pitch;

    int m_horz = 0;
    int m_vert = 0;

    Camera camTH;
    Camera camTHC;
    Camera camNV;
    Camera camNVC;
    Camera camEM;
    Camera camEMC;

    ProcessCharacters[] processCharacters;
    NightVision nightVision;
    ThermalVisionBackground thermalVisionBg;
    EMVisionBackground emVisionBg;

    ThermalVision thermalVision;
    EMVision emVision;

    Canvas cvnButtons = null;

    private Vector3 firstpoint;
    private Vector3 secondpoint;
    private float xAngle = 0.0f; //angle for axes x for rotation
    private float yAngle = 0.0f;
    private float xAngTemp = 0.0f; //temp variable for angle
    private float yAngTemp = 0.0f;
    private bool inTouch = false;

    void Start()
    {
        bool desktop = SystemInfo.deviceType == DeviceType.Desktop;
        string[] js = Input.GetJoystickNames();
        //bool hasInputDevice = Input.mousePresent || js.Length > 0;

        // PlayerController.instance.TabletMode == 1

        cvnButtons = GameObject.Find("cnvButtons").GetComponent<Canvas>();
        cvnButtons.enabled = false; // !hasInputDevice; // ??
        GameObject.Find("cnvHint").GetComponent<Canvas>().enabled = desktop; // hasInputDevice;


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

 //   void Awake() {
	//	enabled = enableInputCapture;
	//}

	//void OnValidate() {
	//	if(Application.isPlaying)
	//		enabled = enableInputCapture;
	//}

    public void OnHorz(int delta)
    {
        m_horz = delta;
    }

    public void OnVert(int delta)
    {
        m_vert = delta;
    }

 //   void CaptureInput() {
	//	Cursor.lockState = CursorLockMode.Locked;

	//	Cursor.visible = false;
	//	m_inputCaptured = true;

	//	m_yaw = transform.eulerAngles.y;
	//	m_pitch = transform.eulerAngles.x;
	//}

	//void ReleaseInput() {
	//	Cursor.lockState = CursorLockMode.None;
	//	Cursor.visible = true;
	//	m_inputCaptured = false;
	//}

	//void OnApplicationFocus(bool focus) {
	//	if(m_inputCaptured && !focus)
	//		ReleaseInput();
	//}



	void Update()
    {
        bool touchHandled = false;
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (!cvnButtons.enabled)
                cvnButtons.enabled = true;

            Touch touch = Input.GetTouch(i);
            if(touch.phase == TouchPhase.Stationary) // || touch.phase == TouchPhase.Began)
            {
                if (camTH.pixelRect.Contains(touch.position))
                {
                    if (camTH.enabled)
                    {
                        nightVision.enabled = false;
                        thermalVisionBg.enabled = true;
                        thermalVision.enabled = true;
                        emVision.enabled = false;
                        emVisionBg.enabled = false;
                        foreach (ProcessCharacters pc in processCharacters)
                            pc.Apply(VisionMode.Thermal);
                    }
                    else
                    {
                        camTH.enabled = true;
                        camTHC.enabled = true;
                        camNV.enabled = true;
                        camNVC.enabled = true;
                        camEM.enabled = true;
                        camEMC.enabled = true;
                    }
                    continue;
                }

                if (camNV.pixelRect.Contains(touch.position))
                {
                    if (camNV.enabled)
                    {
                        nightVision.enabled = true;
                        thermalVisionBg.enabled = false;
                        thermalVision.enabled = false;
                        emVision.enabled = false;
                        emVisionBg.enabled = false;
                        foreach (ProcessCharacters pc in processCharacters)
                            pc.Apply(VisionMode.Night);
                    }
                    else
                    {
                        camTH.enabled = true;
                        camTHC.enabled = true;
                        camNV.enabled = true;
                        camNVC.enabled = true;
                        camEM.enabled = true;
                        camEMC.enabled = true;
                    }
                    continue; 
                }

                if (camEM.pixelRect.Contains(touch.position))
                {
                    if (camEM.enabled)
                    {
                        nightVision.enabled = false;
                        thermalVisionBg.enabled = false;
                        thermalVision.enabled = false;
                        emVision.enabled = true;
                        emVisionBg.enabled = true;
                        foreach (ProcessCharacters pc in processCharacters)
                            pc.Apply(VisionMode.EM);
                    }
                    else
                    {
                        camTH.enabled = true;
                        camTHC.enabled = true;
                        camNV.enabled = true;
                        camNVC.enabled = true;
                        camEM.enabled = true;
                        camEMC.enabled = true;
                    }
                    continue;
                }

                if (touch.position.x < Screen.width / 2 && m_vert == 0 && m_horz == 0)
                {
                    if (camNV.enabled)
                    {
                        camTH.enabled = false;
                        camTHC.enabled = false;
                        camNV.enabled = false;
                        camNVC.enabled = false;
                        camEM.enabled = false;
                        camEMC.enabled = false;
                    }
                    else
                    {
                        nightVision.enabled = false;
                        thermalVisionBg.enabled = false;
                        thermalVision.enabled = false;
                        emVision.enabled = false;
                        emVisionBg.enabled = false;
                        foreach (ProcessCharacters pc in processCharacters)
                            pc.Apply(VisionMode.None);
                    }
                    continue;
                }

                if(m_vert != 0 || m_horz != 0)
                {
                    var fwd = moveSpeed * m_vert;
                    var rght = moveSpeed * m_horz;
                    var upV = moveSpeed * ((Input.GetKey(KeyCode.E) ? 1f : 0f) - (Input.GetKey(KeyCode.Q) ? 1f : 0f));
                    transform.position += transform.forward * fwd + transform.right * rght + Vector3.up * upV;

                    camTH.transform.position = transform.position;
                    camNV.transform.position = transform.position;
                    camEM.transform.position = transform.position;
                }
            }
            else 
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x > Screen.width / 2)
                    {
                        firstpoint = touch.position;
                        xAngTemp = xAngle;
                        yAngTemp = yAngle;
                        inTouch = true;
                    }
                }
                else if (touch.phase == TouchPhase.Moved && inTouch)
                {
                    secondpoint = touch.position;
                    //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
                    yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;

                    if (yAngle < 0)
                        yAngle += 360;
                    if (yAngle > 360)
                        yAngle -= 360;

                    if (yAngle > 90 && yAngle < 270)
                        xAngle = xAngTemp - (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                    else
                        xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;

                    if (xAngle < 0)
                        xAngle += 360;

                    if (xAngle > 360)
                        xAngle -= 360;

                    transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);

                    camTH.transform.rotation = transform.rotation;
                    camNV.transform.rotation = transform.rotation;
                    camEM.transform.rotation = transform.rotation;
                }
                else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && inTouch)
                {
                    inTouch = false;
                    transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);

                    camTH.transform.rotation = transform.rotation;
                    camNV.transform.rotation = transform.rotation;
                    camEM.transform.rotation = transform.rotation;
                }
            }

            touchHandled = true;
        }

        if (touchHandled)
            return;

        if (/*!m_inputCaptured && */Input.GetMouseButtonDown(0))
        {
            if(camTH.pixelRect.Contains(Input.mousePosition))
            {
                if (camTH.enabled)
                {
                    nightVision.enabled = false;
                    thermalVisionBg.enabled = true;
                    thermalVision.enabled = true;
                    emVision.enabled = false;
                    emVisionBg.enabled = false;
                    foreach (ProcessCharacters pc in processCharacters)
                        pc.Apply(VisionMode.Thermal);
                }
                else
                {
                    camTH.enabled = true;
                    camTHC.enabled = true;
                    camNV.enabled = true;
                    camNVC.enabled = true;
                    camEM.enabled = true;
                    camEMC.enabled = true;
                }
                return;
            }

            if (camNV.pixelRect.Contains(Input.mousePosition))
            {
                if (camNV.enabled)
                {
                    nightVision.enabled = true;
                    thermalVisionBg.enabled = false;
                    thermalVision.enabled = false;
                    emVision.enabled = false;
                    emVisionBg.enabled = false;
                    foreach (ProcessCharacters pc in processCharacters)
                        pc.Apply(VisionMode.Night);
                }
                else
                {
                    camTH.enabled = true;
                    camTHC.enabled = true;
                    camNV.enabled = true;
                    camNVC.enabled = true;
                    camEM.enabled = true;
                    camEMC.enabled = true;
                }
                return;
            }

            if (camEM.pixelRect.Contains(Input.mousePosition))
            {
                if (camEM.enabled)
                {
                    nightVision.enabled = false;
                    thermalVisionBg.enabled = false;
                    thermalVision.enabled = false;
                    emVision.enabled = true;
                    emVisionBg.enabled = true;
                    foreach (ProcessCharacters pc in processCharacters)
                        pc.Apply(VisionMode.EM);
                }
                else
                {
                    camTH.enabled = true;
                    camTHC.enabled = true;
                    camNV.enabled = true;
                    camNVC.enabled = true;
                    camEM.enabled = true;
                    camEMC.enabled = true;
                }
                return;
            }

            if (Input.mousePosition.x < Screen.width / 2 && m_vert == 0 && m_horz == 0)
            {
                if (camNV.enabled)
                {
                    camTH.enabled = false;
                    camTHC.enabled = false;
                    camNV.enabled = false;
                    camNVC.enabled = false;
                    camEM.enabled = false;
                    camEMC.enabled = false;
                }
                else
                {
                    nightVision.enabled = false;
                    thermalVisionBg.enabled = false;
                    thermalVision.enabled = false;
                    emVision.enabled = false;
                    emVisionBg.enabled = false;
                    foreach (ProcessCharacters pc in processCharacters)
                        pc.Apply(VisionMode.None);
                }
                return;
            }
        }

		//if(!m_inputCaptured) {
		//	if(!holdRightMouseCapture && Input.GetMouseButtonDown(0)) 
		//		CaptureInput();
		//	else if(holdRightMouseCapture && Input.GetMouseButtonDown(1))
		//		CaptureInput();
		//}

		//if(!m_inputCaptured)
		//	return;

		//if(m_inputCaptured) {
		//	if(!holdRightMouseCapture && Input.GetKeyDown(KeyCode.Escape))
		//		ReleaseInput();
		//	else if(holdRightMouseCapture && Input.GetMouseButtonUp(1))
		//		ReleaseInput();
		//}




		var rotStrafe = Input.GetAxis("Mouse X");
		var rotFwd = Input.GetAxis("Mouse Y");

		m_yaw = (m_yaw + lookSpeed * rotStrafe) % 360f;
		m_pitch = (m_pitch - lookSpeed * rotFwd) % 360f;
		transform.rotation = Quaternion.AngleAxis(m_yaw, Vector3.up) * Quaternion.AngleAxis(m_pitch, Vector3.right);

		var speed = Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed);
		var forward = speed * (m_vert + Input.GetAxis("Vertical"));
		var right = speed * (m_horz + Input.GetAxis("Horizontal"));
		var up = speed * ((Input.GetKey(KeyCode.E) ? 1f : 0f) - (Input.GetKey(KeyCode.Q) ? 1f : 0f));
		transform.position += transform.forward * forward + transform.right * right + Vector3.up * up;


        camTH.transform.rotation = transform.rotation;
        camTH.transform.position = transform.position;

        camNV.transform.rotation = transform.rotation;
        camNV.transform.position = transform.position;

        camEM.transform.rotation = transform.rotation;
        camEM.transform.position = transform.position;
    }
}
