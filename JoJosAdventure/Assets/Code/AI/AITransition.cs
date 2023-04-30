using System.Collections.Generic;
using UnityEngine;

namespace JoJosAdventure
{
    /// <summary>
    /// Represents when AI should change state
    /// Contains decisions, return a result based on if all decisions are met or not to decide a transition to a new state
    /// </summary>
    public class AITransition : MonoBehaviour
    {
        [field: SerializeField]
        public List<AIDecision> Decisions { get; set; }

        /// <summary>
        /// If all decision is positive transition to this state
        /// </summary>
        [field: SerializeField]
        public AIState PositiveResult { get; set; }

        /// <summary>
        /// If any decision fails transition to this state
        /// </summary>
        [field: SerializeField]
        public AIState NegativeResult { get; set; }

        private void Awake()
        {
            this.Decisions.Clear();
            this.Decisions = new List<AIDecision>(this.GetComponents<AIDecision>());
        }
    }
}