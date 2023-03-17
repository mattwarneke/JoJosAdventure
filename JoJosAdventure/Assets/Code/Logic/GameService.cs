namespace Assets.Code.Logic
{
    using Assets.Code.GUI;
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
            this.CollectableCount = 0;
            this.GuiController = GameObject.Find("GuiController").GetComponent<GuiController>();
        }

        public int CollectableCount { get; private set; }
        public GuiController GuiController { get; private set; }

        public void HandleEvent(EventEnum eventTriggered)
        {
            if (this.GuiController == null)// game unload
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
            this.GuiController.PauseJojoMovement();
            this.GuiController.SetMattFollowJojo();
            // should prob split speech in 2 so can do after speech callback
            this.GuiController.MattSpeak(SpeechRepository.GetMattFollowJoJoSpeech());
            this.GuiController.DoActionAfterXTime(3.5f, () =>
            {
                this.GuiController.PanToPaigeWithCallBack(() => this.GuiController.RestartJoJoMovement());
            });
        }

        public void ExitLounge()
        {
            this.GuiController.PauseJojoMovement();
            this.GuiController.MattSpeakWithCallBack(SpeechRepository.GetExitLoungeSpeechNoneCollected(), () =>
            {
                this.GuiController.PanToDemons();
                this.GuiController.DoActionAfterPanFinished(() => this.GuiController.RestartJoJoMovement());
            });
        }

        public void DemonKilled()
        {
            this.RemoveCollectable();

            if (this.CollectableCount > 0)
            {
                this.GuiController.MattSpeak(SpeechRepository.GetDemonDied());
                return;
            }

            this.GuiController.PauseJojoMovement();
            this.GuiController.RemoveBedroomDoor();
            this.GuiController.PanToBedroomDoor();
            this.GuiController.DoActionAfterPanFinished(() =>
            {
                this.GuiController.MattSpeak(SpeechRepository.GetNoMoreDemons());
                this.GuiController.RestartJoJoMovement();
            });
        }

        public void EnterBedroom()
        {
            this.GuiController.PauseJojoMovement();
            this.GuiController.PanToPaigeWithCallBack(() =>
            {
                this.GuiController.MattSpeakWithCallBack(
                    SpeechRepository.GetEnterBedroomSpeech(),
                    () => this.GuiController.RestartJoJoMovement());
            });
        }

        public void JoJoNearJar()
        {
            this.GuiController.PauseJojoMovement();

            // double swip
            this.JoJoSwip();
            this.GuiController.DoActionAfterXTime(0.5f, () => this.JoJoSwip());

            // swip over do speech and ring
            this.GuiController.DoActionAfterXTime(1.5f, () =>
            {
                this.GuiController.MattSpeakWithCallBack(SpeechRepository.GetJoJoBreakJarFailedSpeech(), () =>
                {
                    this.GuiController.ShowRingAnimation();
                    this.GuiController.DoActionAfterXTime(4, () => this.GuiController.ShowMarryMeCanvasDialog());
                    // another chained call paige is free! and Huh is that a ring?
                    // will you marry me paige - speech.
                    // could probably do something more elegant callbacks on animations or something.. but w.e
                });
            });
        }

        public void SheSaidYesFuckYeah()
        {
            this.GuiController.RemoveJar();
            this.GuiController.MattSpeak(SpeechRepository.SheSaidYesFuckYeah());
        }

        public void AddCollectable()
        {
            this.CollectableCount++;
        }

        public void RemoveCollectable()
        {
            this.CollectableCount--;
        }

        public void JoJoSwip()
        {
            this.GuiController.JoJoSwipAnimation();
        }
    }
}