using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GHMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate(Vector3.down * Time.deltaTime);

		// remove at certain pos
		if ( gameObject.transform.localPosition.y <= -190.0f ) {
			Destroy (gameObject);
		}

	}
}
