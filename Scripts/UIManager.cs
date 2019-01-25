using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    float letterTime = .03f;
    string n1, n2, n3, n4, n5, n6, n7, n8, n9, n10;
    string ai1, ai2;
    List<string> nStrings;
    public Text narration, aiText;
    public GameObject startElements, narrativeElements;
    int i = 0;

	// Use this for initialization
	void Start () {
        narration.GetComponent<Text>();
        nStrings = new List<string>();
        AddLines();
	}

    void AddLines()
    {
        ai1 = "<color=lime>AI#1:</color>\n";
        ai2 = "<color=fuchsia>AI#2:</color>\n";

        n1 = "Greetings, disposable Mobile Tech Support Unit 293, and thank you for offering some of your unemployed time to actually make a difference for once. As your title indicates, you are a Mobile Tech Support Unit that fixes big problems. Not for angry old ladies. But for ships. Spaceships. In Space. That means \"Really-really-far-away-from-here\".";
        n2 = "As the previous support unit miraculously detonated, you are currently stuck with me: Your AI Guide. Some call me Codename Agony, others refer to me-";
        n3 = "ERNIE DID YOU INFORM THE UNIT ABOUT THE CONTROLLERS YET!?";
        n4 = "Y-yes, dear…\n\nIgnore that. That’s my partner.";
        n5 = "AS I was saying. Because your predecessors destroyed all the proper equipment in a sea of fire and screams, you are now equipped with the controllers in front of you. These are older models that demand a lot of power. Their circuits aren’t even fully covered. Bunch of tramps if you ask me.";
        n6 = "Anyway, you’ll need them to insert yourself into a spaceship’s surveillance and security systems. You’ll be powerless without them.";
        n7 = "You see, these controllers are used for camera feeds and camera movement, plus color, number, and voice input necessities. Yes, some ships are equipped with voice recognition. Don’t ask me why. You humans always want these nifty things";
        n8 = "ERNIE!?";
        n9 = "But enough talk. You are here to work. And this Unit has just received an emergency request for assistance. Some dumb pilot captain person got hit by something and is now trapped in his cockpit. Wounded and probably dying.";
        n10 = "I will connect you to the ship’s surveillance and security system. Your assignment is to un-trap the captain from his cockpit, so he can be extracted afterwards by an escape pod. I must warn you that the ship’s power is fading and you have limited time. Fading power also means that receiving visual and audio outputs at the same time isn’t possible. You’ll have to switch between the two. Don’t worry too much, I’ll be here in the meantime. Supporting you. Yay.";

        nStrings.Add(n1); nStrings.Add(n2); nStrings.Add(n3); nStrings.Add(n4); nStrings.Add(n5); nStrings.Add(n6); nStrings.Add(n7); nStrings.Add(n8); nStrings.Add(n9); nStrings.Add(n10);
    }

    IEnumerator NarrativeText(int e)
    {
        if ((e == 2) || (e == 7))
        {
            aiText.text = ai2;
        } else
        {
            aiText.text = ai1;
        }

        foreach(char letter in nStrings[e].ToCharArray())
        {
            narration.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterTime);
        }

        yield return new WaitForSeconds(4f);
        narration.text = "";
        i++;
        if (i < 10)
            StartCoroutine(NarrativeText(i));
    }
	
	// Update is called once per frame
	void Update () {
		if (i == 10)
        {
            SceneManager.LoadScene("060618", LoadSceneMode.Single);
        }
	}
    
    public void SkipNarrative()
    {
        i = 10;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        startElements.SetActive(false);
        narrativeElements.SetActive(true);
        StartCoroutine(NarrativeText(i));
    }
}
