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
        public UnityEvent OnGetHit { get; set; }

        [field: SerializeField]
        public UnityEvent OnDie { get; set; }

        [field: SerializeField]
        private LivesController LivesController { get; set; }

        private AgentMovement agentMovement;

        private void Awake()
        {
            this.agentMovement = this.GetComponent<AgentMovement>();
        }

        public void GetHit(int damage, GameObject damageDealer)
        {
            if (!this.LivesController.IsAlive()) return;

            this.LivesController.TakeDamage(damage);
            this.OnGetHit?.Invoke();

            if (!this.LivesController.IsAlive())
            {
                this.OnDie?.Invoke();
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