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
        public void Move(Vector2 movementInput)
        {
            if (movementInput.magnitude > 0)
            {
                // reset velocity if the player is changing direction
                if (Vector2.Dot(movementInput.normalized, this.movementDirection) < 0)
                {
                    this.currentVelocity = 0;
                }
                this.movementDirection = movementInput.normalized;
            }
            this.currentVelocity = this.CalculateSpeed(movementInput);
        }

        private float CalculateSpeed(Vector2 movementInput)
        {
            if (movementInput.magnitude > 0)
            { // has any value
                this.currentVelocity += this.movementData.acceleration * Time.deltaTime;
            }
            else
            {
                this.currentVelocity -= this.movementData.deacceleration * Time.deltaTime;
            }
            return Mathf.Clamp(this.currentVelocity, 0, this.movementData.maxSpeed);
        }
    }
}