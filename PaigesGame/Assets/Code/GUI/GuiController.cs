using Assets.Code.Logic;
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

        public PlayerJojo PlayerJojo;
        public MattScript MattScript;
        public PaigeScript PaigeScript;

        public CameraFollow CameraScript;

        public GameObject DialogCanvasGO;
        
        void Start()
        {
            PlayerJojo = Jojo.GetComponent<PlayerJojo>();
            MattScript = Matt.GetComponent<MattScript>();
            PaigeScript = Paige.GetComponent<PaigeScript>();
        }

        public void SetMattFollowJojo()
        {
            MattScript.ActivateFollow(Jojo.transform);
        }

        public void MattSpeakWithCallBack(List<Speech> speech, Action callback)
        {
            MattSpeak(speech);
            DoActionAfterSpeech(callback);
        }

        public void MattSpeak(List<Speech> speech)
        {
            MattScript.Speak(speech);
        }
        
        public void PauseJojoMovement(float timePaused)
        {
            PlayerJojo.PauseWalking(timePaused);
        }

        public void PauseJojoMovement()
        {
            PlayerJojo.PauseWalking();
        }

        public void RestartJoJoMovement()
        {
            PlayerJojo.RestartWalking();
        }
        
        public Transform[] demonPositions;
        public void PanToDemons()
        {
            if (CameraScript == null)
                return;

            if (demonPositions.Length > 0)
                CameraScript.SetCustomPanTarget(demonPositions[0].position);

            CameraScript.RunActionOnCustomPanFinished(() =>
            {
                CameraScript.SetCustomPanTarget(demonPositions[1].position);
            });
        }

        public void PanToBedroomDoor()
        {
            if (BedroomDoor != null)
                CameraScript.SetCustomPanTarget(BedroomDoor.transform.position);
        }

        public void PanToPaigeWithCallBack(Action callback)
        {
            PanToPaige();
            DoActionAfterPanFinished(callback);
        }

        public void PanToPaige()
        {
            CameraScript.SetCustomPanTarget(Paige.transform.position);
        }

        public void JoJoSwipAnimation()
        {
            PlayerJojo.PlaySwipAnimation();
        }
        
        public GameObject BedroomDoor;
        public void RemoveBedroomDoor()
        {
            if (BedroomDoor != null)
                BedroomDoor.SetActive(false);
        }
        
        public void NearJar()
        {
            PlayerJojo.PlaySwipAnimation();
        }

        public void ShowRingAnimation()
        {
            MattScript.ShowRing();
        }

        public void ShowMarryMeCanvasDialog()
        {
            DialogCanvasGO.SetActive(true);
        }

        public void MarryMeYes()
        {
            DialogCanvasGO.SetActive(false);
            GameService.Instance().SheSaidYesFuckYeah();
        }

        public GameObject JarContainer;
        public void RemoveJar()
        {
            JarContainer.SetActive(false);
            PaigeScript.Speak(SpeechRepository.PaigeFreedom());
            // stop paige animating and kiss???
        }

        public void DoActionAfterXTime(float waitTime, Action callback)
        {
            StartCoroutine(RunCallbackAfterWait(waitTime, callback));
        }

        private IEnumerator RunCallbackAfterWait(float waitTime, Action callback)
        {
            //yield return new WaitUntil(waitTime);
            yield return new WaitForSecondsRealtime(waitTime);
            callback();
        }

        public void DoActionAfterSpeech(Action callback)
        {
            MattScript.speechBubble.RunActionOnSpeechFinished(callback);
        }

        public void DoActionAfterPanFinished(Action callback)
        {
            if (CameraScript == null)
                return;
            CameraScript.RunActionOnCustomPanFinished(callback);
        }
    }
}
