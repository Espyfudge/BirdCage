using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GHCollision : MonoBehaviour {

	public GameObject lightManager;
	LightSwitch lm;

	[HideInInspector]public int correct = 0;

	public Text yourScore;

	bool clickedCorrect;
	int u = 0;
	
	// Use this for initialization
	void Start () {
		lm = lightManager.GetComponent<LightSwitch>();
	}
	
	// Update is called once per frame
	void Update () {

		if (clickedCorrect) {
			u++;
		}

		if ( u == 1 ) {
			StartCoroutine(pause());
		}

	}

	void OnTriggerStay(Collider other) {

		if ( other.gameObject.name.Contains("White") ) {
			if (lm.GHmatch == 1 ) {
				clickedCorrect = true;
			}
		}

		if ( other.gameObject.name.Contains("Red") ) {
			if (lm.GHmatch == 2 ) {
				clickedCorrect = true;
			}
		}

		if ( other.gameObject.name.Contains("Green") ) {
			if (lm.GHmatch == 3 ) {
				clickedCorrect = true;
			}
		}

		if ( other.gameObject.name.Contains("Yellow") ) {
			if (lm.GHmatch == 4 ) {
				clickedCorrect = true;
			}
		}
	}

	IEnumerator pause() {
		correct++;
        yourScore.text = correct.ToString() ;
		yield return new WaitForSeconds(.5f);
		clickedCorrect = false;
		u = 0;
	}

}