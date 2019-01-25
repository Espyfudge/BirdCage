using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class CameraMover : MonoBehaviour {
	//open com5 too, function for that
	SerialPort sp = new SerialPort("COM5",9600);

    [HideInInspector] public int switchData, c1left, c1right, c2left, c2right, c3left, c3right;

    public GameObject keypad;
    public GameObject door;

	public GameObject camera1;
	public GameObject camera2;
    public GameObject camera3;
	public GameObject currentCamera;
	public Camera cam1;
    public Camera cam2;
    public Camera cam3;

	public GameObject overlay1;
	public GameObject overlay2;
	public GameObject overlay3;
	public GameObject offOverlay;
	public GameObject offStartOverlay;
    public GameObject audioOverlay;
    public GameObject timeText;

    [HideInInspector]public bool systemOnline;

    //timers for cameras
    [HideInInspector] public float c1time, c2time, c3time;

    Transform currentLocation;
	Transform targetRight;

	int audioState = 8;
	int videoState = 7;
	int leftState = 4;
	int rightState = 3;
    int zoominState = 5;
    int zoomoutState = 6;
    bool camOn;
    [HideInInspector]public bool audioOn;
    
	int switchLeft = 2;
	int switchRight = 1;
	int currentCam = 1;

    bool enterPressed;
    bool movingLeft;
    bool movingRight;
    bool zoomingIn;
    bool zoomingOut;

    Quaternion cam1Base, cam2Base, cam3Base;

    float sensitivity = 25f;

    AudioSource sound;

    //temp for switching
    bool switched;

    private void Awake()
    {
        cam1Base = camera1.transform.rotation;
        cam2Base = camera2.transform.rotation;
        cam3Base = camera3.transform.rotation;

        sound = GetComponent<AudioSource>();

    }

    // Use this for initialization
    void Start () {
	    //sp.Open();
		sp.ReadTimeout = 1;

        currentCamera = camera1;
        camOn = true;
        CameraOn();
        offStartOverlay.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {

        if (keypad.GetComponent<KeypadCode>().puzzle != 4)
        {
            if (currentCam == 1)
                c1time += Time.deltaTime;
            if (currentCam == 2)
                c2time += Time.deltaTime;
            if (currentCam == 3)
                c3time += Time.deltaTime;
        }

		//temp go online
		//if ( (Input.GetKeyDown(KeyCode.Return)) && (!enterPressed) ) {
  //          camOn = true;
  //          CameraOn();
  //          enterPressed = true;
  //      }

        if ( (Input.GetKeyDown(KeyCode.M)) )
        {
            TempSwitch();
        }

        //temp cam switch
        if ((Input.GetKeyDown(KeyCode.DownArrow)))
        {
            if (currentCam == 2)
            {
                switchData++;
                overlay1.SetActive(true);
                overlay2.SetActive(false);
                overlay3.SetActive(false);
                currentCam = 1;
                currentCamera = camera1;
                cam1.gameObject.SetActive(true);
                cam2.gameObject.SetActive(false);
                cam3.gameObject.SetActive(false);

            }

            if (currentCam == 3)
            {
                switchData++;
                overlay1.SetActive(false);
                overlay2.SetActive(true);
                overlay3.SetActive(false);
                currentCam = 2;
                currentCamera = camera2;
                cam2.gameObject.SetActive(true);
                cam1.gameObject.SetActive(false);
                cam3.gameObject.SetActive(false);

            }
            Debug.Log(currentCam);
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (currentCam == 2)
            {
                switchData++;
                overlay3.SetActive(true);
                overlay1.SetActive(false);
                overlay2.SetActive(false);
                currentCam = 3;
                currentCamera = camera3;
                cam3.gameObject.SetActive(true);
                cam2.gameObject.SetActive(false);
                cam1.gameObject.SetActive(false);
            }

            if (currentCam == 1)
            {
                switchData++;
                overlay1.SetActive(false);
                overlay2.SetActive(true);
                overlay3.SetActive(false);
                currentCam = 2;
                currentCamera = camera2;
                cam2.gameObject.SetActive(true);
                cam1.gameObject.SetActive(false);
                cam3.gameObject.SetActive(false);

            }


            //Debug.Log(currentCam);
        }
        //temp camera movement
        if (Input.GetKey(KeyCode.LeftArrow)) {
            //currentCamera.transform.Rotate (Vector3.down / 1.5f, Space.World);

            if ((currentCam == 1) && (currentCamera.transform.eulerAngles.z > (cam1Base.eulerAngles.z - 5.0f)))
            {
                currentCamera.transform.Rotate(Vector3.back, 1);
            }

            if ((currentCam == 2) && (currentCamera.transform.eulerAngles.z > (cam2Base.eulerAngles.z - 25.0f)))
            {
                currentCamera.transform.Rotate(Vector3.back, 1);
            }


            if ((currentCam == 3) && (currentCamera.transform.eulerAngles.z > (cam3Base.eulerAngles.z - 45.0f)))
            {
                currentCamera.transform.Rotate(Vector3.back, 1);
            }
        }


        if (Input.GetKey(KeyCode.RightArrow)) {

            if ((currentCam == 1) && (currentCamera.transform.eulerAngles.z < (cam1Base.eulerAngles.z + 65.0f)))
            {
                currentCamera.transform.Rotate(Vector3.forward, 1);
            }

            if ((currentCam == 2) && (currentCamera.transform.eulerAngles.z < (cam2Base.eulerAngles.z + 28.0f)))
            {
                currentCamera.transform.Rotate(Vector3.forward, 1);
            }

            if ((currentCam == 3) && (currentCamera.transform.eulerAngles.z < (cam3Base.eulerAngles.z + 25.0f)))
            {
                currentCamera.transform.Rotate(Vector3.forward, 1);
            }

        }

        if (currentCam == 1)
        {
            float minFov = 20f;
            float maxFov = 45f;
            float fov = cam1.fieldOfView;
            fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity * -1f;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            cam1.fieldOfView = fov;
        }

        if (currentCam == 2)
        {
            float minFov = 8f;
            float maxFov = 32f;
            float fov = cam2.fieldOfView;
            fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity * -1f;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            cam2.fieldOfView = fov;
        }

        if (currentCam == 3)
        {
            float minFov = 15f;
            float maxFov = 29f;
            float fov = cam3.fieldOfView;
            fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity * -1f;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            cam3.fieldOfView = fov;
        }

        if (zoomingOut)
        {
            if (currentCam == 1)
            {
                float minFov = 20f;
                float maxFov = 45f;
                float fov = cam1.fieldOfView;
                fov += Time.deltaTime * sensitivity;
                fov = Mathf.Clamp(fov, minFov, maxFov);
                cam1.fieldOfView = fov;
            }

            if (currentCam == 2)
            {
                float minFov = 8f;
                float maxFov = 32f;
                float fov = cam2.fieldOfView;
                fov += Time.deltaTime * sensitivity;
                fov = Mathf.Clamp(fov, minFov, maxFov);
                cam2.fieldOfView = fov;
            }

            if (currentCam == 3)
            {
                float minFov = 15f;
                float maxFov = 29f;
                float fov = cam3.fieldOfView;
                fov += Time.deltaTime * sensitivity;
                fov = Mathf.Clamp(fov, minFov, maxFov);
                cam3.fieldOfView = fov;
            }
        }

        if (zoomingIn)
        {
            if (currentCam == 1)
            {
                float minFov = 20f;
                float maxFov = 45f;
                float fov = cam1.fieldOfView;
                fov -= Time.deltaTime * sensitivity;
                fov = Mathf.Clamp(fov, minFov, maxFov);
                cam1.fieldOfView = fov;
            }

            if (currentCam == 2)
            {
                float minFov = 8f;
                float maxFov = 32f;
                float fov = cam2.fieldOfView;
                fov -= Time.deltaTime * sensitivity;
                fov = Mathf.Clamp(fov, minFov, maxFov);
                cam2.fieldOfView = fov;
            }

            if (currentCam == 3)
            {
                float minFov = 15f;
                float maxFov = 29f;
                float fov = cam3.fieldOfView;
                fov -= Time.deltaTime * sensitivity;
                fov = Mathf.Clamp(fov, minFov, maxFov);
                cam3.fieldOfView = fov;
            }
        }


        //analytics
        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            if (currentCam == 1)
                c1left++;
            if (currentCam == 2)
                c2left++;
            if (currentCam == 3)
                c3left++;
        }

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
            if (currentCam == 1)
                c1right++;
            if (currentCam == 2)
                c2right++;
            if (currentCam == 3)
                c3right++;
        }

        if (movingLeft)
        {
            if ((currentCam == 1) && (currentCamera.transform.eulerAngles.z > (cam1Base.eulerAngles.z - 5.0f)))
            {
                currentCamera.transform.Rotate(Vector3.back, 1);
            }

            if ((currentCam == 2) && (currentCamera.transform.eulerAngles.z > (cam2Base.eulerAngles.z - 45.0f)))
            {
                currentCamera.transform.Rotate(Vector3.back, 1);
            }

            if ((currentCam == 3) && (currentCamera.transform.eulerAngles.z > (cam3Base.eulerAngles.z - 45.0f)))
            {
                currentCamera.transform.Rotate(Vector3.back, 1);
            }
        } 

        if (movingRight)
        {
            if ((currentCam == 1) && (currentCamera.transform.eulerAngles.z < (cam1Base.eulerAngles.z + 65.0f)))
            {
                currentCamera.transform.Rotate(Vector3.forward, 1);
            }

            if ((currentCam == 2) && (currentCamera.transform.eulerAngles.z < (cam2Base.eulerAngles.z + 35.0f)))
            {
                currentCamera.transform.Rotate(Vector3.forward, 1);
            }

            if ((currentCam == 3) && (currentCamera.transform.eulerAngles.z < (cam3Base.eulerAngles.z + 25.0f)))
            {
                currentCamera.transform.Rotate(Vector3.forward, 1);
            }
        }

        


		// checks for data (bytes) sent by arduino
		if (sp.IsOpen) {
			try {
				gameState(sp.ReadByte());
			}
			catch (System.Exception) {

			}
		}

        

	} // end update

	// runs something depending on what byte was sent by arduino
	void gameState (int state) {
		//print(state);

		if (state == videoState) {
			camOn = true;
			audioOn = false;
			CameraOn ();
			AudioOn ();
		}

		if (state == audioState) {
			camOn = false;
			audioOn = true;
			CameraOn ();
			AudioOn ();
		}

		// CHANGE TO ARDUINO JOYSTICK MOVEMENT //
		//left button
		if ( (camOn) && (systemOnline) ) {

            if (state == zoominState)
            {
                zoomingIn = true;
                //StartCoroutine(StopMove());
            } else
            {
                zoomingIn = false;
            }

            if (state == zoomoutState)
            {
                zoomingOut = true;
                //StartCoroutine(StopMove());
            } else
            {
                zoomingOut = false;
            }

            if (state == leftState)
            {
                movingLeft = true;
                //StartCoroutine(StopMove());
            } else
            {
                movingLeft = false; 
            }

            //right button
            if (state == rightState)
            {
                movingRight = true;
                //StartCoroutine(StopMove());
            } else
            {
                movingRight = false;
            }


            if (state == switchRight) {
                if (currentCam == 2)
                {
                    switchData++;
                    overlay1.SetActive(true);
                    overlay2.SetActive(false);
                    overlay3.SetActive(false);
                    currentCam = 1;
                    currentCamera = camera1;
                    cam1.gameObject.SetActive(true);
                    cam2.gameObject.SetActive(false);
                    cam3.gameObject.SetActive(false);

                }

                if (currentCam == 3)
                {
                    switchData++;
                    overlay1.SetActive(false);
                    overlay2.SetActive(true);
                    overlay3.SetActive(false);
                    currentCam = 2;
                    currentCamera = camera2;
                    cam2.gameObject.SetActive(true);
                    cam1.gameObject.SetActive(false);
                    cam3.gameObject.SetActive(false);

                }
                Debug.Log(currentCam);
			}

			if (state == switchLeft) {
                if (currentCam == 2)
                {
                    switchData++;
                    overlay3.SetActive(true);
                    overlay1.SetActive(false);
                    overlay2.SetActive(false);
                    currentCam = 3;
                    currentCamera = camera3;
                    cam3.gameObject.SetActive(true);
                    cam2.gameObject.SetActive(false);
                    cam1.gameObject.SetActive(false);
                }

                if (currentCam == 1)
                {
                    switchData++;
                    overlay1.SetActive(false);
                    overlay2.SetActive(true);
                    overlay3.SetActive(false);
                    currentCam = 2;
                    currentCamera = camera2;
                    cam2.gameObject.SetActive(true);
                    cam1.gameObject.SetActive(false);
                    cam3.gameObject.SetActive(false);

                }


                Debug.Log(currentCam);
            }
		} // end if camon + systemonline
	}

	public void CameraOn() {
		if (camOn == false) {
            timeText.SetActive(false);
			overlay1.SetActive (false);
			overlay2.SetActive (false);
			overlay3.SetActive (false);
		}

        if ((camOn))
        {
            timeText.SetActive(true);
            if (currentCam == 1)
            {
                overlay1.SetActive(true);
                cam1.gameObject.SetActive(true);
            }
            else if (currentCam == 2)
            {
                overlay2.SetActive(true);
                cam2.gameObject.SetActive(true);
            }
            else if (currentCam == 3)
            {
                overlay3.SetActive(true);
                cam3.gameObject.SetActive(true);
            }
        }
	}

	void AudioOn() {
		if (audioOn == false) {
            //no sound
            sound.mute = true;
            door.GetComponent<AudioSource>().mute = true;
            audioOverlay.SetActive(false);
        } 

        if (!systemOnline)
        {
            sound.mute = true;
            door.GetComponent<AudioSource>().mute = true;
            audioOverlay.SetActive(false);
        }

        if ((audioOn))
        {
            audioOverlay.SetActive(true);
            sound.mute = false;
            door.GetComponent<AudioSource>().mute = false;
        }
	}

	//temp function to test switching cam and audio
	public void TempSwitch() {
		if (!switched)
        {
            camOn = false;
            audioOn = true;
            CameraOn();
            AudioOn();
            switched = true;
        } else if (switched)
        {
            camOn = true;
            audioOn = false;
            CameraOn();
            AudioOn();
            switched = false;
        }
	}

    IEnumerator StopMove()
    {
        yield return new WaitForSeconds(.0000001f);
        zoomingOut = false;
        zoomingIn = false;
        movingLeft = false;
        movingRight = false;
    }

	public void ClosePort() {
		sp.Close();
	}
}
