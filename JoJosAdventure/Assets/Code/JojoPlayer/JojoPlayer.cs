using JoJosAdventure.Common.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure
{
    public class JojoPlayer : MonoBehaviour, IAgent
    {
        [field: SerializeField]
        public int Health { get; private set; }

        [field: SerializeField]
        public UnityEvent OnGetHit { get; set; }

        [field: SerializeField]
        public UnityEvent OnDie { get; set; }

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
    }
}