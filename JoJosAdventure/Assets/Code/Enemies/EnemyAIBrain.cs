using JoJosAdventure.Common;
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
        public UnityEvent<MoveEvent> OnMovementPressed { get; set; }

        [field: SerializeField]
        public UnityEvent<Vector2> OnPointerPositionChanged { get; set; }

        private void Awake()
        {
            this.Target = FindObjectOfType<JojoPlayer>().gameObject;
        }

        private void Update()
        {
            this.CurrentState.UpdateState();
        }

        public void Attack()
        {
            this.OnFireButtonPressed?.Invoke();
        }

        public void Move(Vector2 movementDirection, Vector2 targetPosition, float maxSpeedMultiplier = 1)
        {
            this.OnMovementPressed?.Invoke(new MoveEvent(movementDirection, maxSpeedMultiplier));
            this.OnPointerPositionChanged?.Invoke(targetPosition);
        }

        internal void ChangeToState(AIState state)
        {
            this.CurrentState = state;
        }
    }
}