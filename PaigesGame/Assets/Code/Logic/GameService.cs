﻿namespace Assets.Code.Logic
{
    using Assets.Code.GUI;
    using System;
    using System.Collections;
    using UnityEngine;

    public class GameService
    {
        private static GameService _instance;
        public static GameService Instance()
        {
            if (_instance == null)
                _instance = new GameService();
            return _instance;
        }

        private GameService()
        {
            CollectableCount = 0;
            GuiController = GameObject.Find("GuiController").GetComponent<GuiController>();
        }

        public int CollectableCount { get; private set; }
        public GuiController GuiController { get; private set; }

        public void AddCollectable()
        {
            CollectableCount++;
        }

        public void JoJoSwip()
        {
            GuiController.JoJoSwipAnimation();
        }

        public void RemoveCollectable()
        {
            CollectableCount--;
            if (CollectableCount == 0)
            {
                //GuiController.RemoveJar();
                GuiController.RemoveBedroomDoor();
            }
        }
        
        public void HandleEvent(EventEnum eventTriggered)
        {
            switch (eventTriggered)
            {
                case (EventEnum.MattFollowJoJo):
                    GuiController.PauseJojoMovement(8f);
                    GuiController.SetMattFollowJojo();
                    GuiController.MattSpeak(SpeechRepository.GetMattFollowJoJoSpeech());
                    GuiController.DoActionAfterXTime(3.5f, () =>
                    {
                        GuiController.PanToPaige();
                    });
                    break;
                case (EventEnum.ExitLounge):
                    GuiController.PauseJojoMovement(6.5f);
                    GuiController.MattSpeak(SpeechRepository.GetExitLoungeSpeechNoneCollected());
                    GuiController.DoActionAfterSpeech(() =>
                    {
                        GuiController.PanToDemons();
                    });
                    break;
                case (EventEnum.DemonKilled):
                    if (CollectableCount > 0)
                        GuiController.MattSpeak(SpeechRepository.GetDemonDied(CollectableCount));
                    else
                    {
                        GuiController.PauseJojoMovement();
                        GuiController.PanToBedroomDoor();
                        GuiController.DoActionAfterPanFinished(() =>
                        {
                            GuiController.MattSpeak(SpeechRepository.GetNoMoreDemons());
                            GuiController.RestartJoJoMovement();
                        });
                    }
                    break;
                case (EventEnum.EnterBedroom):
                    GuiController.PauseJojoMovement(5f);
                    GuiController.MattSpeak(SpeechRepository.GetEnterBedroomSpeech());
                    GuiController.DoActionAfterXTime(3f, () =>
                    {
                        GuiController.PanToPaige();
                    });
                    break;
                case (EventEnum.NearJarTrigger):
                    JoJoSwip();
                    GuiController.DoActionAfterXTime(0.5f, () =>
                    {
                        JoJoSwip();
                    });
                    GuiController.PauseJojoMovement();
                    GuiController.DoActionAfterXTime(1.5f, () =>
                    {
                        GuiController.MattSpeak(SpeechRepository.GetJoJoBreakJarFailedSpeech());
                        GuiController.DoActionAfterXTime(4, () =>
                        {
                            GuiController.ShowRingAnimation();
                            GuiController.DoActionAfterXTime(4, () =>
                            {
                                GuiController.ShowMarryMeCanvasDialog();
                            });
                            // another chained call paige is free! and Huh is that a ring?
                            // will you marry me paige - speech.
                            // could probably do something more elegant callbacks on animations or something.. but w.e
                        });
                    });
                    break;
                default:
                    return;
            }
        }

        public void SheSaidYesFuckYeah()
        {
            GuiController.RemoveJar();
            GuiController.MattSpeak(SpeechRepository.SheSaidYesFuckYeah());
        }
    }
}
