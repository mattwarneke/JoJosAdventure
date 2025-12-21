using JoJosAdventure.Common;
using System;
using System.Collections;
using UnityEngine;

namespace JoJosAdventure.GUI
{
    public class GameManager : MonoBehaviour
    {
        public GameObject Jojo;
        public GameObject DialogCanvasGO;

        private AgentMovement PlayerJojo;

        private void Start()
        {
            this.PlayerJojo = this.Jojo.GetComponent<AgentMovement>();
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
    }
}