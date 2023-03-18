﻿using Assets.Code.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.GUI
{
    public class GuiController : MonoBehaviour
    {
        public GameObject Jojo;
        public GameObject Matt;
        public GameObject Paige;
        public CameraFollow CameraScript;
        public GameObject DialogCanvasGO;

        private PlayerJojo PlayerJojo;
        private MattScript MattScript;
        private PaigeScript PaigeScript;

        private void Start()
        {
            this.PlayerJojo = this.Jojo.GetComponent<PlayerJojo>();
            this.MattScript = this.Matt.GetComponent<MattScript>();
            this.PaigeScript = this.Paige.GetComponent<PaigeScript>();
        }

        public void SetMattFollowJojo()
        {
            this.MattScript.ActivateFollow(this.Jojo.transform);
        }

        public void MattSpeakWithCallBack(List<Speech> speech, Action callback)
        {
            this.MattSpeak(speech);
            this.DoActionAfterSpeech(callback);
        }

        public void MattSpeak(List<Speech> speech)
        {
            this.MattScript.Speak(speech);
        }

        public void PauseJojoMovement(float timePaused)
        {
            this.PlayerJojo.PauseWalking(timePaused);
        }

        public void PauseJojoMovement()
        {
            this.PlayerJojo.PauseWalking();
        }

        public void RestartJoJoMovement()
        {
            this.PlayerJojo.RestartWalking();
        }

        public Transform[] demonPositions;

        public void PanToDemons()
        {
            if (this.CameraScript == null)
                return;

            if (this.demonPositions.Length > 0)
                this.CameraScript.SetCustomPanTarget(this.demonPositions[0].position);

            this.CameraScript.RunActionOnCustomPanFinished(() =>
            {
                this.CameraScript.SetCustomPanTarget(this.demonPositions[1].position);
            });
        }

        public void PanToBedroomDoor()
        {
            if (this.BedroomDoor != null)
                this.CameraScript.SetCustomPanTarget(this.BedroomDoor.transform.position);
        }

        public void PanToPaigeWithCallBack(Action callback)
        {
            this.PanToPaige();
            this.DoActionAfterPanFinished(callback);
        }

        public void PanToPaige()
        {
            this.CameraScript.SetCustomPanTarget(this.Paige.transform.position);
        }

        public void JoJoSwipAnimation()
        {
            this.PlayerJojo.PlaySwipAnimation();
        }

        public GameObject BedroomDoor;

        public void RemoveBedroomDoor()
        {
            if (this.BedroomDoor != null)
                this.BedroomDoor.SetActive(false);
        }

        public void NearJar()
        {
            this.PlayerJojo.PlaySwipAnimation();
        }

        public void ShowRingAnimation()
        {
            this.MattScript.ShowRing();
        }

        public void ShowMarryMeCanvasDialog()
        {
            this.DialogCanvasGO.SetActive(true);
        }

        public void MarryMeYes()
        {
            this.DialogCanvasGO.SetActive(false);
            GameService.Instance().SheSaidYesFuckYeah();
        }

        public GameObject JarContainer;

        public void RemoveJar()
        {
            this.JarContainer.SetActive(false);
            this.PaigeScript.Speak(SpeechRepository.PaigeFreedom());
            // stop paige animating and kiss???
        }

        public void DoActionAfterXTime(float waitTime, Action callback)
        {
            this.StartCoroutine(this.RunCallbackAfterWait(waitTime, callback));
        }

        private IEnumerator RunCallbackAfterWait(float waitTime, Action callback)
        {
            //yield return new WaitUntil(waitTime);
            yield return new WaitForSecondsRealtime(waitTime);
            callback();
        }

        public void DoActionAfterSpeech(Action callback)
        {
            this.MattScript.speechBubble.RunActionOnSpeechFinished(callback);
        }

        public void DoActionAfterPanFinished(Action callback)
        {
            if (this.CameraScript == null)
                return;
            this.CameraScript.RunActionOnCustomPanFinished(callback);
        }
    }
}