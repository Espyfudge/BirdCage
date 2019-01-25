using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarSounds : MonoBehaviour {

    bool soundActive;

    CameraMover cm;
    AudioSource sound;
    SpriteRenderer sr;

    float alpha = 0.3f;
    bool circleHit;
    bool startFade;
    bool unrelated;

    private void Awake()
    {
        cm = GameObject.Find("CameraManager").GetComponent<CameraMover>();
        sound = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
        if ((gameObject.name.Contains("B")) || (gameObject.name.Contains("D")))
        {
            unrelated = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        soundActive = cm.audioOn;

        if (!soundActive)
        {
            sound.mute = true;
        } else
        {
            sound.mute = false;
        }

        if (circleHit)
        {
            alpha += Time.deltaTime;
            sr.color = new Color(1f, 1f, 1f, alpha);
            if ( (alpha >= 1f) && (!unrelated) )
                circleHit = false;
            if ((alpha >= .4f) && (unrelated))
                circleHit = false;
        }

        if (startFade)
        {
            alpha -= Time.deltaTime;
            sr.color = new Color(1f, 1f, 1f, alpha);
            if ((alpha <= .3f) && (!unrelated))
                startFade = false;
            if ((alpha <= .15f) && (unrelated))
                startFade = false;

        }
	}

    private void OnTriggerEnter(Collider other)
    {
        circleHit = true;
        sound.Play();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(6f);
        startFade = true;
    }
}
