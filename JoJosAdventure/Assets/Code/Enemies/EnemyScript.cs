using JoJosAdventure.Common.Interfaces;
using JoJosAdventure.Logic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure.Enemies
{
    public class EnemyScript : MonoBehaviour, IHittable, IAgent
    {
        public FieldOfView fieldOfView;
        public SpeechBubble speechBubble;

        [field: SerializeField]
        public EnemyDataSO EnemyData { get; set; }

        [field: SerializeField]
        public int Health { get; private set; }

        public GameObject FlipContainer;
        private bool isFacingRight => this.FlipContainer.transform.eulerAngles.y < 180;

        [field: SerializeField]
        public UnityEvent OnGetHit { get; set; }

        [field: SerializeField]
        public UnityEvent OnDie { get; set; }

        // Use this for initialization
        private void Start()
        {
            this.Health = this.EnemyData.MaxHealth;

            if (this.fieldOfView == null) throw new ExpectedInspectorReferenceException();

            string repeatedSpeechText = "Here kitty, kitty, kitty!";
            this.Speak(new Speech(repeatedSpeechText, 2));
            this.Speak(new Speech(null, 1));
            this.Speak(new Speech(repeatedSpeechText, 2));
        }

        private void Update()
        {
        }

        public void GetHit(int damage, GameObject damageDealer)
        {
            this.Health--;
            this.OnGetHit?.Invoke();
            if (this.Health <= 0)
            {
                this.OnDie?.Invoke();
                this.StartCoroutine(this.WaitToDie());
            }
        }

        private IEnumerator WaitToDie()
        {
            yield return new WaitForSeconds(0.54f);
            Destroy(this.gameObject);
        }

        //private void ActivateFollow(Transform transformToFollow)
        //{
        //    this.Speak(new Speech("Hug the kitty!!!", 2));
        //}

        //private void PlayerOutOfSite()
        //{
        //    this.Speak(new Speech("Where did the kitty go?", 2));
        //}

        //private void GrabJojo()
        //{
        //    this.Speak(new Speech("Has kitty!!!", 2));
        //}

        private void Speak(Speech speech)
        {
            this.speechBubble.AddToSpeechQueue(speech);
        }
    }
}