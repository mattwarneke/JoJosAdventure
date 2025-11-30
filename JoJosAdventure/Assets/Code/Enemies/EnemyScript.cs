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

        [field: SerializeField]
        public EnemyAttack EnemyAttack { get; set; }

        private bool dead = false;

        [field: SerializeField]
        public UnityEvent OnGetHit { get; set; }

        [field: SerializeField]
        public UnityEvent OnDie { get; set; }

        private void Awake()
        {
            if (this.EnemyAttack == null)
            {
                this.EnemyAttack = this.GetComponent<EnemyAttack>();
            }
        }

        private void Start()
        {
            this.Health = this.EnemyData.MaxHealth;

            if (this.fieldOfView == null) throw new ExpectedInspectorReferenceException();
            this.StartUpSpeech();
        }

        private void Update()
        {
        }

        public void GetHit(int damage, GameObject damageDealer)
        {
            if (this.dead) return;

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

        public void PerformAttack()
        {
            if (this.dead) return;

            this.Speak(new Speech("HUG KITTY!!", 1));
            this.EnemyAttack.Attack(this.EnemyData.Damage);
        }

        private void Speak(Speech speech)
        {
            this.speechBubble.AddToSpeechQueue(speech);
        }

        private void StartUpSpeech()
        {
            string repeatedSpeechText = "Here kitty, kitty, kitty!";
            this.Speak(new Speech(repeatedSpeechText, 2));
            this.Speak(new Speech(null, 1));
            this.Speak(new Speech(repeatedSpeechText, 2));
        }
    }
}