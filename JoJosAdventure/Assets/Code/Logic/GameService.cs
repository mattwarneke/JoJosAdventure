namespace JoJosAdventure.Logic
{
    using JoJosAdventure.GUI;
    using UnityEngine;

    public class GameService : MonoBehaviour
    {
        private static GameService instance;
        public static GameService Instance => instance;

        public static bool IsRunning => instance != null;

        /// <summary>
        /// StartGame
        /// When the GamseService Object Awakes prepare the game
        /// </summary>
        private void Awake()
        {
            if (instance != null && (object)instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }

            this.CollectableCount = 0;
        }

        public int CollectableCount { get; private set; }
        public GameManager GameManager;

        public void HandleEvent(EventEnum eventTriggered)
        {
            if (this.GameManager == null)// game unload
                return;

            switch (eventTriggered)
            {
                case EventEnum.MattFollowJoJo:
                    this.MattFollowJoJo();
                    break;

                case EventEnum.ExitLounge:
                    this.ExitLounge();
                    break;

                case EventEnum.DemonKilled:
                    this.DemonKilled();
                    break;

                case EventEnum.EnterBedroom:
                    this.EnterBedroom();
                    break;

                case EventEnum.NearJarTrigger:
                    this.JoJoNearJar();
                    break;

                default:
                    return;
            }
        }

        public void MattFollowJoJo()
        {
            this.GameManager.SetMattFollowJojo();
            // should prob split speech in 2 so can do after speech callback
            this.GameManager.MattSpeak(SpeechRepository.GetMattFollowJoJoSpeech());
            this.GameManager.DoActionAfterXTime(3.5f, () =>
            {
                this.GameManager.PanToPaigeWithCallBack(() =>
                {
                    //this.GuiController.RestartJoJoMovement()
                });
            });
        }

        public void ExitLounge()
        {
            //this.GuiController.PauseJojoMovement();
            this.GameManager.MattSpeakWithCallBack(SpeechRepository.GetExitLoungeSpeechNoneCollected(), () =>
            {
                this.GameManager.PanToDemons();
                this.GameManager.DoActionAfterPanFinished(() =>
                {
                    //this.GuiController.RestartJoJoMovement()
                });
            });
        }

        public void DemonKilled()
        {
            this.RemoveCollectable();

            if (this.CollectableCount > 0)
            {
                this.GameManager.MattSpeak(SpeechRepository.GetDemonDied());
                return;
            }

            //this.GuiController.PauseJojoMovement();
            this.GameManager.RemoveBedroomDoor();
            this.GameManager.PanToBedroomDoor();
            this.GameManager.DoActionAfterPanFinished(() =>
            {
                this.GameManager.MattSpeak(SpeechRepository.GetNoMoreDemons());
                //this.GuiController.RestartJoJoMovement();
            });
        }

        public void EnterBedroom()
        {
            //this.GuiController.PauseJojoMovement();
            this.GameManager.PanToPaigeWithCallBack(() =>
            {
                this.GameManager.MattSpeakWithCallBack(
                    SpeechRepository.GetEnterBedroomSpeech(),
                    () =>
                    {
                        //this.GuiController.RestartJoJoMovement()
                    });
            });
        }

        public void JoJoNearJar()
        {
            //this.GuiController.PauseJojoMovement();

            // double swip
            // swip over do speech and ring
            this.GameManager.DoActionAfterXTime(1.5f, () =>
            {
                this.GameManager.MattSpeakWithCallBack(SpeechRepository.GetJoJoBreakJarFailedSpeech(), () =>
                {
                    this.GameManager.ShowRingAnimation();
                    this.GameManager.DoActionAfterXTime(4, () => this.GameManager.ShowMarryMeCanvasDialog());
                    // another chained call paige is free! and Huh is that a ring?
                    // will you marry me paige - speech.
                    // could probably do something more elegant callbacks on animations or something.. but w.e
                });
            });
        }

        public void SheSaidYesFuckYeah()
        {
            this.GameManager.RemoveJar();
            this.GameManager.MattSpeak(SpeechRepository.SheSaidYesFuckYeah());
        }

        public void AddCollectable()
        {
            this.CollectableCount++;
        }

        public void RemoveCollectable()
        {
            this.CollectableCount--;
        }
    }
}