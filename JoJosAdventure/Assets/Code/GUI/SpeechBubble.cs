using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoJosAdventure.NPC;

namespace JoJosAdventure.GUI
{
    public class SpeechBubble : MonoBehaviour
    {
        public GameObject speechBubbleContainer;
        public Text speechBubbleText;

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.currentlyPlayingSpeech == null && this.SpeechQueue.Count == 0)
                return;

            if (this.HasSpeechTimePassed())
            {
                if (this.SpeechQueue.Count > 0)
                    this.PlayNextSpeech();
                else
                    this.FinishSpeech();
            }
        }

        private Speech currentlyPlayingSpeech { get; set; }
        private float lastSpeechTime;

        private bool HasSpeechTimePassed()
        {
            float timeSpeechPlaying = Time.time - this.lastSpeechTime;
            return timeSpeechPlaying > this.currentlyPlayingSpeech.SpeechTimeSeconds;
        }

        private Queue<Speech> SpeechQueue = new Queue<Speech>();

        public void AddToSpeechQueue(Speech speech)
        {
            this.SpeechQueue.Enqueue(speech);

            this.PlayNextSpeech();
        }

        public void AddToSpeechQueue(List<Speech> speechs)
        {
            foreach (Speech speech in speechs)
                this.SpeechQueue.Enqueue(speech);

            this.PlayNextSpeech();
        }

        private void PlayNextSpeech()
        {
            Speech speech = this.SpeechQueue.Dequeue();
            if (speech == null)
                return;

            this.currentlyPlayingSpeech = speech;
            this.lastSpeechTime = Time.time;

            if (this.speechBubbleContainer == null)
                return;

            if (!string.IsNullOrEmpty(speech.SpeechText))
            {
                this.speechBubbleContainer.SetActive(true);
                this.speechBubbleText.text = speech.SpeechText;
            }
            else
            {   // empty speech is a pause.
                this.speechBubbleContainer.SetActive(false);
            }
        }

        private void FinishSpeech()
        {
            this.speechBubbleContainer.SetActive(false);
            this.currentlyPlayingSpeech = null;
        }

        public void EmptySpeechQueue()
        {
            this.currentlyPlayingSpeech = null;
            this.speechBubbleContainer.SetActive(false);
            this.SpeechQueue.Clear();
        }

        public void RunActionOnSpeechFinished(Action callback)
        {
            this.StartCoroutine(this.RunActionOnSpeechFinishedCoroutine(callback));
        }

        private IEnumerator RunActionOnSpeechFinishedCoroutine(Action callback)
        {
            yield return new WaitWhile(() => this.currentlyPlayingSpeech != null);
            callback();
        }
    }
}