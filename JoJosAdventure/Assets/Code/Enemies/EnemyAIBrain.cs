using JoJosAdventure.Common.Interfaces;
using JoJosAdventure.Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure
{
    public class EnemyAIBrain : MonoBehaviour, IAgentInput
    {
        [field: SerializeField]
        public GameObject Target { get; set; }

        [field: SerializeField]
        public AIState CurrentState { get; private set; }

        [field: SerializeField]
        public FieldOfView EnemyFOV { get; private set; }

        [field: SerializeField]
        public UnityEvent OnFireButtonPressed { get; set; }

        [field: SerializeField]
        public UnityEvent OnFireButtonReleased { get; set; }

        [field: SerializeField]
        public UnityEvent<Vector2> OnMovementPressed { get; set; }

        [field: SerializeField]
        public UnityEvent<Vector2> OnPointerPositionChanged { get; set; }

        private void Awake()
        {
            this.Target = FindObjectOfType<JojoPlayer>().gameObject;
        }

        private void Update()
        {
            if (this.Target == null)
            {
                // stop movement
                this.OnMovementPressed?.Invoke(Vector2.zero);
            }
            // Run actions each frame
            this.CurrentState.UpdateState();
        }

        public void Attack()
        {
            this.OnFireButtonPressed?.Invoke();
        }

        public void Move(Vector2 movementDirection, Vector2 targetPosition)
        {
            this.OnMovementPressed?.Invoke(movementDirection);
            this.OnPointerPositionChanged?.Invoke(targetPosition);
        }

        internal void ChangeToState(AIState state)
        {
            this.CurrentState = state;
        }
    }
}