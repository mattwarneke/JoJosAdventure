using JoJosAdventure.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure.Common
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AgentMovement : MonoBehaviour
    {
        protected Rigidbody2D rigidBody2d;

        [field: SerializeField]
        public MovementDataSO movementData { get; set; }

        protected Vector2 movementDirection;
        protected float currentVelocity = 0;
        protected float arriveBufferDistance = 0.01f;

        private MoveEvent lastMoveEvent { get; set; }
        private Vector2? targetPosition { get; set; }

        [field: SerializeField]
        public UnityEvent<float> OnVelocityChange { get; set; }

        // Use this for initialization
        private void Awake()
        {
            this.rigidBody2d = this.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!this.targetPosition.HasValue)
                return;

            Vector2 toTarget = this.targetPosition.Value - (Vector2)this.transform.position;

            if (toTarget.magnitude < this.arriveBufferDistance)
            {
                // reached target → stop
                this.targetPosition = null;
                this.currentVelocity = 0f;
                this.movementDirection = Vector2.zero;
            }
            else
            {
                this.movementDirection = toTarget.normalized;
            }

            this.currentVelocity = this.CalculateSpeed(this.movementDirection, this.lastMoveEvent.SpeedModifer);

            this.OnVelocityChange?.Invoke(this.currentVelocity);

            this.rigidBody2d.velocity = this.currentVelocity * this.movementDirection.normalized;
        }

        // called from UnityEvent
        public void SetDestination(MoveEvent moveEvent)
        {
            this.lastMoveEvent = moveEvent;
            this.targetPosition = moveEvent.InputWorldPosition;
        }

        private float CalculateSpeed(Vector2 target, float speedMultiplier)
        {
            if (target.sqrMagnitude > 0)
            { // has any value
                this.currentVelocity += this.movementData.acceleration * Time.deltaTime;
            }
            else
            {
                this.currentVelocity -= this.movementData.deacceleration * Time.deltaTime;
            }
            return Mathf.Clamp(this.currentVelocity, 0, this.movementData.maxSpeed * speedMultiplier);
        }
    }

    public struct MoveEvent
    {
        public MoveEvent(Vector2 movementInput, float movementSpeedModifer = 1)
        {
            this.InputWorldPosition = movementInput;
            this.SpeedModifer = movementSpeedModifer;
        }

        public Vector2 InputWorldPosition { get; set; }

        public float SpeedModifer { get; set; }
    }
}