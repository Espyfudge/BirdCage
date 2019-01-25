using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;

public class LightSwitch : MonoBehaviour {

	SerialPort sp = new SerialPort("COM3",9600);

	public GameObject GHPanel;

	[HideInInspector]public int GHmatch;

	public GameObject GHwhite;
	public GameObject GHred;
	public GameObject GHyellow;
	public GameObject GHgreen;
	public Transform GHSpawn;
	public GameObject GHScore;
    public Text ghClue, inputSS;

    public GameObject keypadManager, radarPuzzle;
    public GameObject ssWhite, ssRed, ssGreen, ssYellow, ssWrong1, ssWrong2, ssWrong3, ssCorrect1, ssCorrect2, ssCorrect3, ssCorrect4;
    public GameObject hatch, door;

    public bool startSS;
    public bool ghStart;

    bool ssTurn;
    bool ghActive;
    bool canCheck;
    bool done;
    bool s1;

    int sq = 1;

    KeypadCode kp;

    int sequence = 1;
    int wrongs;
    int i;
    int e;
    public bool ssFinish;
    bool hs;

    string white, red, green, yellow;

    List<int> combination;
    List<int> solving;
    List<string> solvingStrings;

    // Use this for initialization
    void Start () {
		//sp.Open();
		sp.ReadTimeout = 1;
        kp = keypadManager.GetComponent<KeypadCode>();
        white = "<color=white>o</color>";
        red = "<color=red>o</color>";
        green = "<color=green>o</color>";
        yellow = "<color=yellow>o</color>";
	}
	
	// Update is called once per frame
	void Update () {

		// checks for data (bytes) sent by arduino
		if (sp.IsOpen) {
			try {
				gameState(sp.ReadByte());
			}
			catch (System.Exception) {

			}
		}

        if ( (ghStart) && (!done) )
        {
            GHPanel.SetActive(true);
            ghActive = true;
            done = true;
            
        }

		//time loop for GH puzzle - activate only when 2nd puzzle starts
		if (ghActive) { 
            StartCoroutine(DropGH());
            ghActive = false;
		}

		if ( (GHScore.GetComponent<GHCollision>().correct >= 10) && (!canCheck) ) {
			//game won, show screen, stop puzzle
			
			GHPanel.SetActive(false);
			ghClue.gameObject.SetActive(true);
            ghActive = false;
            canCheck = true;
            StartCoroutine(ShowClue());
        }

        tempLights();
        if (startSS)
            SimonSays();


        

        if ((ssFinish))
        {
            hatch.transform.Rotate(Vector3.forward * Time.deltaTime * 30f);
            if (!hs)
            {
                StartCoroutine(StopHatch());
                hs = true;
            }
        }
	}

    IEnumerator StopHatch()
    {
        yield return new WaitForSeconds(5f);
        ssFinish = false;
    }

    void SimonSays()
    {
        if ((sequence == 1) && (!s1))
        {
            inputSS.gameObject.SetActive(true);
            solvingStrings = new List<string>();
            combination = new List<int>();
            solving = new List<int>();
            combination.Add(2);
            combination.Add(4);
            combination.Add(3);
            StartCoroutine(Sequence1());
            s1 = true;
        }

        if (sq == 1)
        {
            if (solvingStrings.Count == 0)
                inputSS.text = "<color=#00FF09>_ _ _</color>";
            if (solvingStrings.Count == 1)
                inputSS.text = solvingStrings[0] + " _ _";
            if (solvingStrings.Count == 2)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " _";
            if (solvingStrings.Count == 3)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " " + solvingStrings[2];
        }

        if (sq == 2)
        {
            if (solvingStrings.Count == 0)
                inputSS.text = "<color=#00FF09>_ _ _ _</color>";
            if (solvingStrings.Count == 1)
                inputSS.text = solvingStrings[0] + " _ _ _";
            if (solvingStrings.Count == 2)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " _ _";
            if (solvingStrings.Count == 3)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " " + solvingStrings[2] + " _";
            if (solvingStrings.Count == 4)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " " + solvingStrings[2] + " " + solvingStrings[3];
        }

        if (sq == 3)
        {
            if (solvingStrings.Count == 0)
                inputSS.text = "<color=#00FF09>_ _ _ _ _</color>";
            if (solvingStrings.Count == 1)
                inputSS.text = solvingStrings[0] + " _ _ _ _";
            if (solvingStrings.Count == 2)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " _ _ _";
            if (solvingStrings.Count == 3)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " " + solvingStrings[2] + " _ _";
            if (solvingStrings.Count == 4)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " " + solvingStrings[2] + " " + solvingStrings[3] + " _";
            if (solvingStrings.Count == 5)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " " + solvingStrings[2] + " " + solvingStrings[3] + " " + solvingStrings[4];
        }

        if (sq == 4)
        {
            if (solvingStrings.Count == 0)
                inputSS.text = "<color=#00FF09>_ _ _ _ _ _</color>";
            if (solvingStrings.Count == 1)
                inputSS.text = solvingStrings[0] + " _ _ _ _ _";
            if (solvingStrings.Count == 2)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " _ _ _ _";
            if (solvingStrings.Count == 3)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " " + solvingStrings[2] + " _ _ _";
            if (solvingStrings.Count == 4)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " " + solvingStrings[2] + " " + solvingStrings[3] + " _ _";
            if (solvingStrings.Count == 5)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " " + solvingStrings[2] + " " + solvingStrings[3] + " " + solvingStrings[4] + " _";
            if (solvingStrings.Count == 6)
                inputSS.text = solvingStrings[0] + " " + solvingStrings[1] + " " + solvingStrings[2] + " " + solvingStrings[3] + " " + solvingStrings[4] + " " + solvingStrings[5];
        }

    }

    void CheckSequence()
    {
        Debug.Log(i);

        if (solving != null)
        {
            if (solving.Count > 0)
            {
                if ((e < solving.Count) && (i < combination.Count))
                {
                    if (solving[e] != combination[i])
                    {
                        wrongs++;
                        StartCoroutine(AfterWrong());
                        i = 0;
                        e++;
                    } else
                    {
                        i++;
                        e++;

                        if ((sequence == 1) && (i == 3))
                        {
                            ssTurn = false;
                            solving.Clear();
                            StartCoroutine(AfterFill());
                            combination.Clear();
                            ssCorrect1.SetActive(true);
                            combination.Add(3);
                            combination.Add(1);
                            combination.Add(4);
                            combination.Add(2);
                            e = 0;
                            sequence = 2;
                            i = 0;
                            StartCoroutine(Sequence2());
                            // sequence one successful
                        }

                        if ((sequence == 2) && (i == 4))
                        {
                            ssTurn = false;
                            solving.Clear();
                            StartCoroutine(AfterFill());
                            combination.Clear();
                            ssCorrect2.SetActive(true);
                            combination.Add(4);
                            combination.Add(2);
                            combination.Add(1);
                            combination.Add(3);
                            combination.Add(2);
                            e = 0;
                            sequence = 3;
                            i = 0;
                            StartCoroutine(Sequence3());
                            // sequence two successful
                        }
                        if ((sequence == 3) && (i == 5))
                        {
                            ssTurn = false;
                            solving.Clear();
                            StartCoroutine(AfterFill());
                            combination.Clear();
                            ssCorrect3.SetActive(true);
                            combination.Add(1);
                            combination.Add(4);
                            combination.Add(3);
                            combination.Add(2);
                            combination.Add(3);
                            combination.Add(1);
                            e = 0;
                            sequence = 4;
                            i = 0;
                            StartCoroutine(Sequence4());
                            // sequence three successful
                        }
                        if ((sequence == 4) && (i == 6))
                        {
                            ssTurn = false;
                            solving.Clear();
                            StartCoroutine(AfterFill());
                            combination.Clear();
                            ssCorrect4.SetActive(true);
                            e = 0;
                            sequence = 5;
                            i = 0;
                            ssFinish = true;
                            kp.puzzle = 4;
                            startSS = false;
                            door.GetComponent<AudioSource>().Play();
                            StartCoroutine(After4());
                            // sequence four successful - open hatch, activate input ability
                        }
                    }
                }
            }
        }

        if (wrongs >= 1)
            ssWrong1.SetActive(true);

        if (wrongs >= 2)
            ssWrong2.SetActive(true);

        if (wrongs >= 3)
        {
            ssWrong3.SetActive(true);
            StartCoroutine(WrongSequence());
        }

    }

    IEnumerator AfterFill()
    {
        yield return new WaitForSeconds(.2f);
        solvingStrings.Clear();
        sq++;
    }

    IEnumerator AfterWrong()
    {
        yield return new WaitForSeconds(.2f);
        solvingStrings.Clear();
    }
    
    IEnumerator After4()
    {
        yield return new WaitForSeconds(1f);
        CancelInvoke();
        radarPuzzle.GetComponent<RadarCircle>().ping = false;
        radarPuzzle.GetComponent<RadarCircle>().radarStart = false;
        inputSS.gameObject.SetActive(false);
    }

    IEnumerator Sequence1()
    {
        yield return new WaitForSeconds(5f);
        ssRed.SetActive(true);
        StartCoroutine(SimonPressed(ssRed));
        yield return new WaitForSeconds(1f);
        ssYellow.SetActive(true);
        StartCoroutine(SimonPressed(ssYellow));
        yield return new WaitForSeconds(1f);
        ssGreen.SetActive(true);
        StartCoroutine(SimonPressed(ssGreen));
        yield return new WaitForSeconds(1f);
        ssTurn = true;
        Invoke("TimePassed", 5f);
    }

    IEnumerator Sequence2()
    {
        yield return new WaitForSeconds(3f);
        CancelInvoke();
        ssGreen.SetActive(true);
        StartCoroutine(SimonPressed(ssGreen));
        yield return new WaitForSeconds(1f);
        ssWhite.SetActive(true);
        StartCoroutine(SimonPressed(ssWhite));
        yield return new WaitForSeconds(1f);
        ssYellow.SetActive(true);
        StartCoroutine(SimonPressed(ssYellow));
        yield return new WaitForSeconds(1f);
        ssRed.SetActive(true);
        StartCoroutine(SimonPressed(ssRed));
        yield return new WaitForSeconds(1f);
        ssTurn = true;
        Invoke("TimePassed", 5f);
    }

    IEnumerator Sequence3()
    {
        yield return new WaitForSeconds(3f);
        CancelInvoke();
        ssYellow.SetActive(true);
        StartCoroutine(SimonPressed(ssYellow));
        yield return new WaitForSeconds(1f);
        ssRed.SetActive(true);
        StartCoroutine(SimonPressed(ssRed));
        yield return new WaitForSeconds(1f);
        ssWhite.SetActive(true);
        StartCoroutine(SimonPressed(ssWhite));
        yield return new WaitForSeconds(1f);
        ssGreen.SetActive(true);
        StartCoroutine(SimonPressed(ssGreen));
        yield return new WaitForSeconds(1f);
        ssRed.SetActive(true);
        StartCoroutine(SimonPressed(ssRed));
        yield return new WaitForSeconds(1f);
        ssTurn = true;
        Invoke("TimePassed", 5f);
    }

    IEnumerator Sequence4()
    {
        yield return new WaitForSeconds(3f);
        CancelInvoke();
        ssWhite.SetActive(true);
        StartCoroutine(SimonPressed(ssWhite));
        yield return new WaitForSeconds(1f);
        ssYellow.SetActive(true);
        StartCoroutine(SimonPressed(ssYellow));
        yield return new WaitForSeconds(1f);
        ssGreen.SetActive(true);
        StartCoroutine(SimonPressed(ssGreen));
        yield return new WaitForSeconds(1f);
        ssRed.SetActive(true);
        StartCoroutine(SimonPressed(ssRed));
        yield return new WaitForSeconds(1f);
        ssGreen.SetActive(true);
        StartCoroutine(SimonPressed(ssGreen));
        yield return new WaitForSeconds(1f);
        ssWhite.SetActive(true);
        StartCoroutine(SimonPressed(ssWhite));
        yield return new WaitForSeconds(1f);
        ssTurn = true;
        Invoke("TimePassed", 5f);
    }

    void TimePassed()
    {
        Debug.Log("time passsed");
        ssWrong1.SetActive(true);
        ssWrong2.SetActive(true);
        ssWrong3.SetActive(true);
        StartCoroutine(WrongSequence());
    }

    IEnumerator WrongSequence()
    {
        ssCorrect1.SetActive(false);
        ssCorrect2.SetActive(false);
        ssCorrect3.SetActive(false);
        ssCorrect4.SetActive(false);
        ssTurn = false;
        i = 0;
        e = 0;
        wrongs = 0;
        combination.Clear();
        solving.Clear();
        solvingStrings.Clear();
        yield return new WaitForSeconds(.7f);
        CancelInvoke();
        ssWrong1.SetActive(false);
        ssWrong2.SetActive(false);
        ssWrong3.SetActive(false);
        yield return new WaitForSeconds(.7f);
        CancelInvoke();
        ssWrong1.SetActive(true);
        ssWrong2.SetActive(true);
        ssWrong3.SetActive(true);
        yield return new WaitForSeconds(.7f);
        CancelInvoke();
        ssWrong1.SetActive(false);
        ssWrong2.SetActive(false);
        ssWrong3.SetActive(false);
        startSS = false;
        sequence = 1;
        sq = 1;
        s1 = false;
        kp.keypad.text = "_ _ _ _";
        kp.insertCodeText1.gameObject.SetActive(true);
        kp.puzzle = 3;
        ssTurn = false;
        radarPuzzle.GetComponent<RadarCircle>().ping = true;
        radarPuzzle.GetComponent<RadarCircle>().radarStart = true;
        inputSS.gameObject.SetActive(false);
    }

   

    IEnumerator ShowClue()
    {
        yield return new WaitForSeconds(1.5f);
        ghClue.text = "# # # 8";
        yield return new WaitForSeconds(1.5f);
        ghClue.text = "# # 8 6";
        yield return new WaitForSeconds(1.5f);
        ghClue.text = "# 8 6 3";
        yield return new WaitForSeconds(1.5f);
        ghClue.text = "8 6 3 0";
    }

    // spawns GH balls at spawn pos gameobject - randomises which ball spawns
    IEnumerator DropGH()
    {
        float randomDrop = Random.Range(1.3f, 1.7f);
        yield return new WaitForSeconds(randomDrop);
        int randomSpawn = Random.Range(1, 5);
        if (randomSpawn == 1)
        {
            Instantiate(GHwhite, GHSpawn);
        }

        if (randomSpawn == 2)
        {
            Instantiate(GHred, GHSpawn);
        }

        if (randomSpawn == 3)
        {
            Instantiate(GHgreen, GHSpawn);
        }

        if (randomSpawn == 4)
        {
            Instantiate(GHyellow, GHSpawn);
        }
        ghActive = true;
    }

    IEnumerator SimonPressed(GameObject colour)
    {
        yield return new WaitForSeconds(0.5f);
        colour.SetActive(false);
    }

    IEnumerator AfterPress()
    {
        yield return new WaitForSeconds(0.375f);
        GHmatch = 0;
    }

	// runs something depending on what byte was sent by arduino
	void gameState (int state) {
		//print(state);

		if (state == 1) {
			//white
			GHmatch = 1;
            if ((startSS))
            {
                if (ssTurn)
                {
                    CancelInvoke();
                    ssWhite.SetActive(true);
                    StartCoroutine(SimonPressed(ssWhite));
                    solving.Add(1);
                    solvingStrings.Add(white);
                    CheckSequence();
                    Invoke("TimePassed", 5f);
                }
                else
                {
                    wrongs++;
                    CheckSequence();
                }
            }
            StartCoroutine(AfterPress());
            
        }

		if (state == 2) {
			//red
			GHmatch = 2;
            if ((startSS))
            {
                if (ssTurn)
                {
                    CancelInvoke();
                    ssRed.SetActive(true);
                    StartCoroutine(SimonPressed(ssRed));
                    solving.Add(2);
                    solvingStrings.Add(red);
                    CheckSequence();
                    Invoke("TimePassed", 5f);
                }
                else
                {
                    wrongs++;
                    CheckSequence();
                }
            }
            StartCoroutine(AfterPress());
        }

		if (state == 3) {
			//green
			GHmatch = 3;
            if ((startSS))
            {
                if (ssTurn)
                {
                    CancelInvoke();
                    ssGreen.SetActive(true);
                    StartCoroutine(SimonPressed(ssGreen));
                    solving.Add(3);
                    solvingStrings.Add(green);
                    CheckSequence();
                    Invoke("TimePassed", 5f);
                }
                else
                {
                    wrongs++;
                    CheckSequence();
                }
            }
            StartCoroutine(AfterPress());
        }

		if (state == 4) {
			//yellow
			GHmatch = 4;
            if ((startSS))
            {
                if (ssTurn)
                {
                    CancelInvoke();
                    ssYellow.SetActive(true);
                    StartCoroutine(SimonPressed(ssYellow));
                    solving.Add(4);
                    solvingStrings.Add(yellow);
                    CheckSequence();
                    Invoke("TimePassed", 5f);
                }
                else
                {
                    wrongs++;
                    CheckSequence();
                }
            }
            StartCoroutine(AfterPress());
        } 
	} 

	public void closePort() {
		sp.Close();
	}

    void tempLights()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //white
            GHmatch = 1;
            if ((startSS))
            {
                if (ssTurn)
                {
                    CancelInvoke();
                    ssWhite.SetActive(true);
                    StartCoroutine(SimonPressed(ssWhite));
                    solving.Add(1);
                    solvingStrings.Add(white);
                    CheckSequence();
                    Invoke("TimePassed", 5f);
                } else
                {
                    wrongs++;
                    CheckSequence();
                }
            }
            StartCoroutine(AfterPress());
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            //red
            GHmatch = 2;
            if ((startSS))
            {
                if (ssTurn)
                {
                    CancelInvoke();
                    ssRed.SetActive(true);
                    StartCoroutine(SimonPressed(ssRed));
                    solving.Add(2);
                    solvingStrings.Add(red);
                    CheckSequence();
                    Invoke("TimePassed", 5f);
                }
                else
                {
                    wrongs++;
                    CheckSequence();
                }
            }
            StartCoroutine(AfterPress());
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //green
            GHmatch = 3;
            if ((startSS))
            {
                if (ssTurn)
                {
                    CancelInvoke();
                    ssGreen.SetActive(true);
                    StartCoroutine(SimonPressed(ssGreen));
                    solving.Add(3);
                    solvingStrings.Add(green);
                    CheckSequence();
                    Invoke("TimePassed", 5f);
                }
                else
                {
                    wrongs++;
                    CheckSequence();
                }
            }
            StartCoroutine(AfterPress());
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            //yellow
            GHmatch = 4;
            if ((startSS))
            {
                if (ssTurn)
                {
                    CancelInvoke();
                    ssYellow.SetActive(true);
                    StartCoroutine(SimonPressed(ssYellow));
                    solving.Add(4);
                    solvingStrings.Add(yellow);
                    CheckSequence();
                    Invoke("TimePassed", 5f);
                }
                else
                {
                    wrongs++;
                    CheckSequence();
                }
            }
            StartCoroutine(AfterPress());
        }
    }

}
