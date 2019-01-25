using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;

public class KeypadCode : MonoBehaviour {

	SerialPort sp = new SerialPort("COM6",9600);

    public GameObject cameraScript;
    public Text keypad;
    public GameObject wrongText, wrongText2;
    public GameObject correctText, correctText2;
    public GameObject resetText, resetText2;
    public Text feedCode;
    public Text cpt;
    //temp
    public GameObject winPanel;
    public Text insertCodeText1, insertCodeText2;

    public GameObject lightManager;
    public GameObject radarPuzzle;

    [HideInInspector]public int timesWrong, timesReset;

    CameraMover cm;

	[HideInInspector]public int puzzle = 1;
    [HideInInspector]public bool p1, p2, p3, p4, p5;

	List<int> combination;
	List<int> solving;

	// Use this for initialization
	void Start () {
        //sp.Open();
        sp.ReadTimeout = 1;
        cm = cameraScript.GetComponent<CameraMover>();
	}

	// Update is called once per frame
	void Update () {

		// checks for data (bytes) sent by arduino
		if (sp.IsOpen) {
			try {
				gameState(sp.ReadChar());
   			}
			catch (System.Exception) {

			}
		}

		if ( (puzzle == 1) && (!p1) ) {
			combination = new List<int> ();
            solving = new List<int>();
			combination.Add (1);
			combination.Add (2);
			combination.Add (3);
			combination.Add (4);
            p1 = true;
		}

        if ((puzzle == 2) && (!p2) )
        {
            combination.Add(9);
            combination.Add(5);
            combination.Add(4);
            combination.Add(4);
            p2 = true;
        }

        if ((puzzle == 3) && (!p3) )
        {
            combination.Add(3);
            combination.Add(1);
            combination.Add(2);
            combination.Add(5);
            p3 = true;
        }

        if ((puzzle == 4) && (!p4))
        {
            combination.Add(8);
            combination.Add(6);
            combination.Add(3);
            combination.Add(0);
            p4 = true;
        }

        if ((puzzle == 5) && (!p5))
        {
            combination.Add(2);
            combination.Add(4);
            combination.Add(1);
            combination.Add(8);
            p5 = true;
        }

        TempInput();
        KeypadText();
        //Debug.Log(solving.Count);

    }

    void CheckCode()
    {
        if (solving.Count >= 4)
        {
            if ((p1) && (solving[0] == combination[0]) && (solving[1] == combination[1]) && (solving[2] == combination[2]) && (solving[3] == combination[3]))
            {
                cm.offOverlay.SetActive(false);
                cm.offStartOverlay.SetActive(false);
                cm.systemOnline = true;
                solving.Clear();
                combination.Clear();
                puzzle = 2;
                p1 = false;
                StartCoroutine(CorrectCode());
                return;
            }

            if ((p2) && (solving[0] == combination[0]) && (solving[1] == combination[1]) && (solving[2] == combination[2]) && (solving[3] == combination[3]))
            {
                solving.Clear();
                combination.Clear();
                puzzle = 3;
                lightManager.GetComponent<LightSwitch>().ghStart = true;
                radarPuzzle.GetComponent<RadarCircle>().ping = true;
                radarPuzzle.GetComponent<RadarCircle>().radarStart = true;
                p2 = false;
                StartCoroutine(CorrectCode());
                insertCodeText1.text = "INSERT CODE #2";
                return;
            }

            if ( (p3) && (solving[0] == combination[0]) && (solving[1] == combination[1]) && (solving[2] == combination[2]) && (solving[3] == combination[3]) )
            {
                solving.Clear();
                combination.Clear();
                puzzle = 0;
                p3 = false;
                StartCoroutine(CorrectCode());
                keypad.text = "DOOR";
                insertCodeText1.gameObject.SetActive(false);
                lightManager.GetComponent<LightSwitch>().startSS = true;
                radarPuzzle.GetComponent<RadarCircle>().StopRadar();
                return;
            }

            if ((p4) && (solving[0] == combination[0]) && (solving[1] == combination[1]) && (solving[2] == combination[2]) && (solving[3] == combination[3]))
            {
                solving.Clear();
                combination.Clear();
                puzzle = 5;
                p4 = false;
                StartCoroutine(CorrectCode());
                insertCodeText2.text = "INSERT CODE #4";
                return;
            }

            if ((p5) && (solving[0] == combination[0]) && (solving[1] == combination[1]) && (solving[2] == combination[2]) && (solving[3] == combination[3]))
            {
                solving.Clear();
                combination.Clear();
                puzzle = 6;
                p5 = false;
                StartCoroutine(CorrectCode());
                winPanel.SetActive(true);
                return;
            }

            if ((solving[0] != combination[0]) || (solving[1] != combination[1]) || (solving[2] != combination[2]) || (solving[3] != combination[3]))
            {
                solving.Clear();
                timesWrong++;
                StartCoroutine(WrongCode());
                return;
            }
        }
    }

    IEnumerator WrongCode()
    {
        if ((puzzle != 0) && (puzzle < 4))
        {
            wrongText.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            wrongText.SetActive(false);
        }

        if ((puzzle != 0) && (puzzle >= 4))
        {
            wrongText2.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            wrongText2.SetActive(false);
        }
    }

    IEnumerator CorrectCode()
    {
        if ((puzzle != 0) && (puzzle < 4))
        {
            correctText.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            correctText.SetActive(false);
        }

        if ((puzzle != 0) && (puzzle >= 4))
        {
            correctText2.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            correctText2.SetActive(false);
        }
    }

    IEnumerator ResetCode()
    {
        if ((puzzle != 0) && (puzzle < 4))
        {
            resetText.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            resetText.SetActive(false);
        }

        if ((puzzle != 0) && (puzzle >= 4))
        {
            resetText2.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            resetText2.SetActive(false);
        }
    }

    void KeypadText()
    {
        if ( (puzzle > 0) && (puzzle < 4) )
        {
            if (solving.Count == 0)
            {
                keypad.text = "_ _ _ _";
                feedCode.text = "_ _ _ _";
            }
            if (solving.Count == 1)
            {
                keypad.text = solving[0] + " _ _ _";
                feedCode.text = solving[0] + " _ _ _";
            }
            if (solving.Count == 2)
            {
                keypad.text = solving[0] + " " + solving[1] + " _ _";
                feedCode.text = solving[0] + " " + solving[1] + " _ _";
            }
            if (solving.Count == 3)
            {
                keypad.text = solving[0] + " " + solving[1] + " " + solving[2] + " _";
                feedCode.text = solving[0] + " " + solving[1] + " " + solving[2] + " _";
            }
            if (solving.Count == 4)
            {
                keypad.text = solving[0] + " " + solving[1] + " " + solving[2] + " " + solving[3];
                feedCode.text = solving[0] + " " + solving[1] + " " + solving[2] + " " + solving[3];
            }
        }

        if (puzzle >= 4)
        {
            if (solving.Count == 0)
            {
                cpt.text = "_ _ _ _";
            }
            if (solving.Count == 1)
            {
                cpt.text = solving[0] + " _ _ _";
            }
            if (solving.Count == 2)
            {
                cpt.text = solving[0] + " " + solving[1] + " _ _";
            }
            if (solving.Count == 3)
            {
                cpt.text = solving[0] + " " + solving[1] + " " + solving[2] + " _";
            }
            if (solving.Count == 4)
            {
                cpt.text = solving[0] + " " + solving[1] + " " + solving[2] + " " + solving[3];
            }
        }
    }

	void gameState(int state) {
        Debug.Log(state);

        if (puzzle != 0)
        {
            if (state == 0)
            {
                solving.Add(0);
                Debug.Log("yeet");
            }

            if (state == 1)
            {
                solving.Add(1);
            }

            if (state == 2)
            {
                solving.Add(2);
            }

            if (state == 3)
            {
                solving.Add(3);
            }

            if (state == 4)
            {
                solving.Add(4);
            }

            if (state == 5)
            {
                solving.Add(5);
            }

            if (state == 6)
            {
                solving.Add(6);
            }

            if (state == 7)
            {
                solving.Add(7);
            }

            if (state == 8)
            {
                solving.Add(8);
            }

            if (state == 9)
            {
                solving.Add(9);
            }

            if (state == 11)
            {
                solving.Clear();
                timesReset++;
                StartCoroutine(ResetCode());
            }

            if (state == 12)
            {
                CheckCode();
            }
        }
    }

    void TempInput()
    {
        if (puzzle != 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                solving.Add(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                solving.Add(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                solving.Add(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                solving.Add(3);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                solving.Add(4);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                solving.Add(5);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                solving.Add(6);
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                solving.Add(7);
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                solving.Add(8);
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                solving.Add(9);
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                solving.Clear();
                timesReset++;
                StartCoroutine(ResetCode());
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckCode();
            }
        }
    }
}