using System.Collections;
using UnityEngine;

namespace JoJosAdventure
{
    public abstract class EnemyAttack : MonoBehaviour
    {
        protected EnemyAIBrain enemyBrain;

        [field: SerializeField]
        public float AttackDelay { get; private set; }

        protected bool waitBeforeNextAttack;

        private void Awake()
        {
            this.enemyBrain = this.GetComponent<EnemyAIBrain>();
        }

        public abstract void Attack(int damage);

        protected IEnumerator WaitBeforeAttackCoroutine()
        {
            this.waitBeforeNextAttack = true;
            yield return new WaitForSeconds(this.AttackDelay);
            this.waitBeforeNextAttack = false;
        }

        protected GameObject GetTarget()
        {
            return this.enemyBrain.Target;
        }
    }
}