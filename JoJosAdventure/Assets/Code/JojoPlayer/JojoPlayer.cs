using JoJosAdventure.Common;
using JoJosAdventure.Common.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure
{
    public class JojoPlayer : MonoBehaviour, IAgent, IHittable, IStunnable
    {
        [field: SerializeField]
        public int Health { get; private set; }

        private bool dead = false;

        [field: SerializeField]
        public UnityEvent OnGetHit { get; set; }

        [field: SerializeField]
        public UnityEvent OnDie { get; set; }

        private AgentMovement agentMovement;

        private void Awake()
        {
            this.agentMovement = this.GetComponent<AgentMovement>();
        }

        public void GetHit(int damage, GameObject damageDealer)
        {
            if (this.dead) return;

            this.Health--;
            this.OnGetHit?.Invoke();
            if (this.Health <= 0)
            {
                this.OnDie?.Invoke();
                this.dead = true;
                this.StartCoroutine(this.WaitToDie());
            }
        }

        public void GetStunned(float duration)
        {
            this.agentMovement.GetStunned(duration);
        }

        private IEnumerator WaitToDie()
        {
            yield return new WaitForSeconds(0.54f);
            Destroy(this.gameObject);
        }
    }
}