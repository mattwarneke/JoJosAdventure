using JoJosAdventure.Common;
using JoJosAdventure.Common.Interfaces;
using JoJosAdventure.Logic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure.Enemies
{
    public class EnemyScript : MonoBehaviour, IHittable, IAgent
    {
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

        private AgentMovement agentMovement;

        private void Awake()
        {
            if (this.EnemyAttack == null)
            {
                this.EnemyAttack = this.GetComponent<EnemyAttack>();
            }
            this.agentMovement = this.GetComponent<AgentMovement>();
        }

        private void Start()
        {
            this.Health = this.EnemyData.MaxHealth;

            this.StartUpSpeech();
        }

        private void Update()
        {
        }

        public void GetHit(int damage, float? stunDuration)
        {
            if (this.dead) return;

            this.Health--;
            if (stunDuration.HasValue)
                this.agentMovement.GetStunned(stunDuration.Value);

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
            this.agentMovement.GetStunned(this.EnemyAttack.AttackDelay);
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