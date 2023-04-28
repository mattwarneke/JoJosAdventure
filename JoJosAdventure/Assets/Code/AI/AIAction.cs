using UnityEngine;

namespace JoJosAdventure
{
    /// <summary>
    /// Represents what should happen when we enter an AIState
    /// An action to perform based on the current state
    /// </summary>
    public abstract class AIAction : MonoBehaviour
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

        public abstract void TakeAction();
    }
}