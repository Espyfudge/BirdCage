using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ANALYTICS : MonoBehaviour {

	public GameObject kp;
	public GameObject cm;

    public GameObject _analytics;

    public Text wrongCodes, resetsUsed, camSwitched;
    public Text cam1Data, cam2Data, cam3Data;
    public Text timePlayed, timePlayed2;
    public Text p1time;
    public Text p2time;

	private KeypadCode _kp;
	private CameraMover _cm;

    private float p1t;
    private float p2t;
    private float playTime;
    private string minutes;
    private string seconds;
    private string hours;
    bool isActive;
	// Use this for initialization
	void Start () {
		_kp = kp.GetComponent<KeypadCode> ();
		_cm = cm.GetComponent<CameraMover> ();
	}
	
	// Update is called once per frame
	void Update () {

        if ( (Input.GetKey(KeyCode.LeftShift)) && (Input.GetKey(KeyCode.K)) && (!isActive))
        {
            _analytics.SetActive(true);
            isActive = true;
        } else if ((Input.GetKey(KeyCode.LeftShift)) && (Input.GetKey(KeyCode.K)) && (isActive))
        {
            _analytics.SetActive(false);
            isActive = false;
        }

        if (_kp.puzzle != 6)
            playTime += Time.deltaTime;

        if (_kp.puzzle == 2)
            p1t += Time.deltaTime;

        if (_kp.puzzle == 3)
            p2t += Time.deltaTime;

        hours = Mathf.Floor(((playTime/3600)%24)).ToString("00");
        minutes = Mathf.Floor(((playTime / 60)%60)).ToString("00");
        seconds = Mathf.Floor((playTime % 60)).ToString("00");
        

        timePlayed.text = hours + ":" + minutes + ":" + seconds;
        timePlayed2.text = timePlayed.text;
        p1time.text = "Puzzle 1 time = " + Mathf.Floor((p1t / 60)).ToString("00") + ":" + Mathf.Floor((p1t % 60)).ToString("00"); 
        p2time.text = "Puzzle 2 time = " + Mathf.Floor((p2t / 60)).ToString("00") + ":" + Mathf.Floor((p2t % 60)).ToString("00"); 

        wrongCodes.text = "Wrong Codes = " + _kp.timesWrong;
		resetsUsed.text = "Reset Used = " + _kp.timesReset;
		camSwitched.text = "Camera Switched = " + _cm.switchData;
		cam1Data.text = "CAM 1 Moved left = " + _cm.c1left + " Moved right = " + _cm.c1right + " Time: " + Mathf.Floor((_cm.c1time/60)).ToString("00") + ":" + Mathf.Floor((_cm.c1time%60)).ToString("00");
		cam2Data.text = "CAM 2 Moved left = " + _cm.c2left + " Moved right = " + _cm.c2right + " Time: " + Mathf.Floor((_cm.c2time / 60)).ToString("00") + ":" + Mathf.Floor((_cm.c2time % 60)).ToString("00");
        cam3Data.text = "CAM 3 Moved left = " + _cm.c3left + " Moved right = " + _cm.c3right + " Time: " + Mathf.Floor((_cm.c3time / 60)).ToString("00") + ":" + Mathf.Floor((_cm.c3time % 60)).ToString("00");
    }
}
