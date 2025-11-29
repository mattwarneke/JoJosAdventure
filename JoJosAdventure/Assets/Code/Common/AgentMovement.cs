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

        [field: SerializeField]
        public UnityEvent<float> OnVelocityChange { get; set; }

        // Use this for initialization
        private void Awake()
        {
            this.rigidBody2d = this.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            this.OnVelocityChange?.Invoke(this.currentVelocity);

            this.rigidBody2d.velocity = this.currentVelocity * this.movementDirection.normalized;
        }

        // called from UnityEvent
        public void Move(MoveEvent moveEvent)
        {
            if (moveEvent.Input.magnitude > 0)
            {
                // reset velocity if the player is changing direction
                if (Vector2.Dot(moveEvent.Input.normalized, this.movementDirection) < 0)
                {
                    this.currentVelocity = 0;
                }
                this.movementDirection = moveEvent.Input.normalized;
            }
            this.currentVelocity = this.CalculateSpeed(moveEvent);
        }

        private float CalculateSpeed(MoveEvent moveEvent)
        {
            if (moveEvent.Input.magnitude > 0)
            { // has any value
                this.currentVelocity += this.movementData.acceleration * Time.deltaTime;
            }
            else
            {
                this.currentVelocity -= this.movementData.deacceleration * Time.deltaTime;
            }
            return Mathf.Clamp(this.currentVelocity, 0, this.movementData.maxSpeed * moveEvent.SpeedModifer);
        }
    }

    public struct MoveEvent
    {
        public MoveEvent(Vector2 movementInput, float movementSpeedModifer = 1)
        {
            this.Input = movementInput;
            this.SpeedModifer = movementSpeedModifer;
        }

        public Vector2 Input { get; set; }

        public float SpeedModifer { get; set; }
    }
}