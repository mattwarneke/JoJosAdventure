using Assets.Code.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public GameObject speechBubbleContainer;
    public Text speechBubbleText;
	// Use this for initialization
	void Start () {
    }
	

	// Update is called once per frame
	void Update ()
    {
        if (currentlyPlayingSpeech == null && this.SpeechQueue.Count == 0)
            return;

        if (HasSpeechTimePassed())
        {
            if (this.SpeechQueue.Count > 0)
                PlayNextSpeech();
            else
                FinishSpeech();
        }

	}

    Speech currentlyPlayingSpeech { get; set; }
    float lastSpeechTime;
    private bool HasSpeechTimePassed()
    {
        float timeSpeechPlaying = Time.time - lastSpeechTime;
        return timeSpeechPlaying > currentlyPlayingSpeech.SpeechTimeSeconds;
    }

    Queue<Speech> SpeechQueue = new Queue<Speech>();
    public void AddToSpeechQueue(List<Speech> speechs)
    {
        foreach (Speech speech in speechs)
            SpeechQueue.Enqueue(speech);

        PlayNextSpeech();
    }
    
    void PlayNextSpeech()
    {
        Speech speech = SpeechQueue.Dequeue();
        if (speech == null)
            return;

        currentlyPlayingSpeech = speech;
        lastSpeechTime = Time.time;

        if (this.speechBubbleContainer == null)
            return;

        if (!string.IsNullOrEmpty(speech.SpeechText))
        {
            this.speechBubbleContainer.SetActive(true);
            speechBubbleText.text = speech.SpeechText;
        }
        else
        {   // empty speech is a pause.
            this.speechBubbleContainer.SetActive(false);
        }
    }

    void FinishSpeech()
    {
        this.speechBubbleContainer.SetActive(false);
        currentlyPlayingSpeech = null;
    }

    public void EmptySpeechQueue()
    {
        currentlyPlayingSpeech = null;
        speechBubbleContainer.SetActive(false);
        SpeechQueue.Clear();
    }

    public void RunActionOnSpeechFinished(Action callback)
    {
        StartCoroutine(RunActionOnSpeechFinishedCoroutine(callback));
    }

    IEnumerator RunActionOnSpeechFinishedCoroutine(Action callback)
    {
        yield return new WaitWhile(() => currentlyPlayingSpeech != null);
        callback();
    }
}
