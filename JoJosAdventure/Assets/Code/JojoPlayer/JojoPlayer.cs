using JoJosAdventure.Common;
using JoJosAdventure.Common.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure
{
    public class JojoPlayer : MonoBehaviour, IAgent, IHittable
    {
        [field: SerializeField]
        public UnityEvent OnGetHit { get; set; }

        [field: SerializeField]
        public UnityEvent OnDie { get; set; }

        [field: SerializeField]
        private LivesController LivesController { get; set; }

        [field: SerializeField]
        private FearController FearController { get; set; }

        private AgentMovement agentMovement;

        private float fearOnHit = 25f;

        private void Awake()
        {
            this.agentMovement = this.GetComponent<AgentMovement>();
        }

        public void GetHit(int damage, float? stunDuration)
        {
            if (!this.LivesController.IsAlive()) return;

            this.LivesController.TakeDamage(damage);
            this.FearController.AddFear(this.fearOnHit);
            if (stunDuration.HasValue)
                this.agentMovement.GetStunned(stunDuration.Value);

            this.OnGetHit?.Invoke();

            if (!this.LivesController.IsAlive())
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
    }
}