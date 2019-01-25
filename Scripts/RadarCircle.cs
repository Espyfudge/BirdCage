using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarCircle : MonoBehaviour {

    public bool ping;
    public bool radarStart;
    public GameObject cameraManager;
    public GameObject face;

    CameraMover cm;
    private Transform circle;
    private Material circleImage;
    private float scale;
    private Color fade;
    private float alpha = 1f;
    bool soundActive;
    bool startFace;

    float speed = 40f;
    AudioSource sound;
	// Use this for initialization
	void Start () {
        cm = cameraManager.GetComponent<CameraMover>();
        sound = GetComponent<AudioSource>();
        circle = gameObject.transform;
        circle.localScale = new Vector3(5f, 5f, 1f);
        circleImage = GetComponent<Renderer>().material;
        fade = new Color(1f, 1f, 1f, 1f);
        circleImage.color = fade;
	}
	
	// Update is called once per frame
	void Update () {

        soundActive = cm.audioOn;

        if (!soundActive)
        {
            face.GetComponent<AudioSource>().mute = true;
            sound.mute = true;
        } else
        {
            face.GetComponent<AudioSource>().mute = false;
            sound.mute = false;
        }

        if (ping)
        {
            sound.Play();
            ping = false;
        }

		if (radarStart)
        {
            if (!startFace)
            {
                face.GetComponent<AudioSource>().Play();
                startFace = true;
            }
            scale += Time.deltaTime * speed;
            circle.localScale = new Vector3(scale, scale, 0f);
            fade = new Color(1f, 1f, 1f, alpha);

        }
        
        if (scale >= 320f)
        {
            if (alpha > 0f)
                alpha -= Time.deltaTime;
            circleImage.color = fade;
        }

        if (scale >= 360f)
        {
            radarStart = false;
            StartCoroutine(RadarReset());
        }
    }

    IEnumerator RadarReset()
    {
        alpha = 1f;
        scale = 5f;
        circleImage.color = new Color(1f, 1f, 1f, 1f);
        circle.localScale = new Vector3(5f, 5f, 1f);
        yield return new WaitForSeconds(1f);
        radarStart = true;
        ping = true;
    }

    public void StopRadar()
    {
        radarStart = false;
        ping = false;
        alpha = 1f;
        scale = 5f;
        circleImage.color = new Color(1f, 1f, 1f, 1f);
        circle.localScale = new Vector3(5f, 5f, 1f);
        face.GetComponent<AudioSource>().Stop();
    }
}
