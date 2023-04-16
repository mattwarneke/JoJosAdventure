using UnityEngine;

namespace JoJosAdventure
{
    /// <summary>
    /// Represent decision making for AI State
    /// </summary>
    public abstract class AIDecision : MonoBehaviour
    {
        protected AIActionData AIActionData;
        protected AIMovementData AIMovementData;
        protected EnemyAIBrain EnemyBrain;

        private void Awake()
        {
            // transform.root get the top most transform in heirarchy then search i.e Enemy
            this.AIActionData = this.transform.root.GetComponentInChildren<AIActionData>();
            this.AIMovementData = this.transform.root.GetComponentInChildren<AIMovementData>();
            // transform.root get the top most transform in heirarchy brain is there
            this.EnemyBrain = this.transform.root.GetComponent<EnemyAIBrain>();
        }

        public abstract bool MakeADecision();
    }
}