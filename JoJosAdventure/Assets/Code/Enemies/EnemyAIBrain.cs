using JoJosAdventure.Common.Interfaces;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure
{
    public class EnemyAIBrain : MonoBehaviour, IAgentInput
    {
        [field: SerializeField]
        public GameObject Target { get; set; }

        [field: SerializeField]
        public UnityEvent OnFireButtonPressed { get; set; }

        [field: SerializeField]
        public UnityEvent OnFireButtonReleased { get; set; }

        [field: SerializeField]
        public UnityEvent<Vector2> OnMovementPressed { get; set; }

        [field: SerializeField]
        public UnityEvent<Vector2> OnPointerPositionChanged { get; set; }

        internal void ChangeToState(AIState positiveResult)
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            this.Target = FindObjectOfType<JojoPlayer>().gameObject;
        }
    }
}