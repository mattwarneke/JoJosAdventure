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
            CollectableCount = 0;
            GuiController = GameObject.Find("GuiController").GetComponent<GuiController>();
        }

        public int CollectableCount { get; private set; }
        public GuiController GuiController { get; private set; }

        public void HandleEvent(EventEnum eventTriggered)
        {
            if (GuiController == null)// game unload
                return;

            switch (eventTriggered)
            {
                case (EventEnum.MattFollowJoJo):
                    MattFollowJoJo();
                    break;
                case (EventEnum.ExitLounge):
                    ExitLounge();
                    break;
                case (EventEnum.DemonKilled):
                    DemonKilled();
                    break;
                case (EventEnum.EnterBedroom):
                    EnterBedroom();
                    break;
                case (EventEnum.NearJarTrigger):
                    JoJoNearJar();
                    break;
                default:
                    return;
            }
        }
        
        public void MattFollowJoJo()
        {
            GuiController.PauseJojoMovement();
            GuiController.SetMattFollowJojo();
            // should prob split speech in 2 so can do after speech callback
            GuiController.MattSpeak(SpeechRepository.GetMattFollowJoJoSpeech());
            GuiController.DoActionAfterXTime(3.5f, () =>
            {
                GuiController.PanToPaigeWithCallBack(() => GuiController.RestartJoJoMovement());
            });
        }

        public void ExitLounge()
        {
            GuiController.PauseJojoMovement();
            GuiController.MattSpeakWithCallBack(SpeechRepository.GetExitLoungeSpeechNoneCollected(), () => 
            {
                GuiController.PanToDemons();
                GuiController.DoActionAfterPanFinished(() => GuiController.RestartJoJoMovement());
            });
        }

        public void DemonKilled()
        {
            RemoveCollectable();

            if (CollectableCount > 0)
            {
                GuiController.MattSpeak(SpeechRepository.GetDemonDied());
                return;
            }

            GuiController.PauseJojoMovement();
            GuiController.RemoveBedroomDoor();
            GuiController.PanToBedroomDoor();
            GuiController.DoActionAfterPanFinished(() =>
            {
                GuiController.MattSpeak(SpeechRepository.GetNoMoreDemons());
                GuiController.RestartJoJoMovement();
            });
        }

        public void EnterBedroom()
        {
            GuiController.PauseJojoMovement();
            GuiController.PanToPaigeWithCallBack(() =>
            {
                GuiController.MattSpeakWithCallBack(
                    SpeechRepository.GetEnterBedroomSpeech(), 
                    () => GuiController.RestartJoJoMovement());
            });
        }

        public void JoJoNearJar()
        {
            GuiController.PauseJojoMovement();

            // double swip
            JoJoSwip();
            GuiController.DoActionAfterXTime(0.5f, () => JoJoSwip());
            
            // swip over do speech and ring
            GuiController.DoActionAfterXTime(1.5f, () =>
            {
                GuiController.MattSpeakWithCallBack(SpeechRepository.GetJoJoBreakJarFailedSpeech(), () =>
                {
                    GuiController.ShowRingAnimation();
                    GuiController.DoActionAfterXTime(4, () => GuiController.ShowMarryMeCanvasDialog());
                    // another chained call paige is free! and Huh is that a ring?
                    // will you marry me paige - speech.
                    // could probably do something more elegant callbacks on animations or something.. but w.e
                });
            });
        }

        public void SheSaidYesFuckYeah()
        {
            GuiController.RemoveJar();
            GuiController.MattSpeak(SpeechRepository.SheSaidYesFuckYeah());
        }

        public void AddCollectable()
        {
            CollectableCount++;
        }
        
        public void RemoveCollectable()
        {
            CollectableCount--;
        }

        public void JoJoSwip()
        {
            GuiController.JoJoSwipAnimation();
        }
    }
}
