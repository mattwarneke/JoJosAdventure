using System.Collections.Generic;
using UnityEngine;

namespace JoJosAdventure
{
    /// <summary>
    /// Represents the current AI state
    /// Current state and drive update state logic
    /// </summary>
    public class AIState : MonoBehaviour
    {
        private EnemyAIBrain enemyBrain = null;

        [SerializeField]
        private List<AIAction> actions = null;

        /// <summary>
        /// Transitions are ordered by decision order
        /// </summary>
        [SerializeField]
        private List<AITransition> transitions = null;

        private void Awake()
        {
            // transform root is enemy
            this.enemyBrain = this.transform.root.GetComponent<EnemyAIBrain>();
        }

        public void UpdateState()
        {
            foreach (var action in this.actions)
            {
                action.TakeAction();
            }

            // Transitions are ordered by decision order
            // Check if all decisions for transition are true
            // When all are true we should transition to that new state
            foreach (var transition in this.transitions)
            {
                bool result = false;
                foreach (var decision in transition.Decisions)
                {
                    result = decision.MakeADecision();
                    if (!result)
                    {
                        break;
                    }
                }
                // if no result was assigned in transition = null then we don't change state
                if (result)
                {
                    if (transition.PositiveResult != null)
                    {
                        this.enemyBrain.ChangeToState(transition.PositiveResult);
                        return; // State has changed
                    }
                }
                else
                {
                    if (transition.NegativeResult != null)
                    {
                        this.enemyBrain.ChangeToState(transition.NegativeResult);
                        return; // State has changed
                    }
                }
            }
        }
    }
}