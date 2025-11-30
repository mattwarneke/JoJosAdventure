using JoJosAdventure.ScriptableObjects;
using System.Collections;
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
        protected float ArriveBufferDistance { get; set; } = 0.2f;

        private MoveEvent lastMoveEvent { get; set; }
        private Vector2? targetPosition { get; set; }

        [field: SerializeField]
        public UnityEvent<float> OnVelocityChange { get; set; }

        [field: SerializeField]
        public UnityEvent<Vector2> OnDirectionChange { get; set; }

        private bool isStunned { get; set; }

        // Use this for initialization
        private void Awake()
        {
            this.rigidBody2d = this.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!this.targetPosition.HasValue || this.isStunned)
            {
                return;
            }

            Vector2 toTarget = this.targetPosition.Value - (Vector2)this.transform.position;
            if (toTarget.magnitude < this.ArriveBufferDistance)
            {
                // reached target → stop
                this.stopMovement();
            }
            else
            {
                this.movementDirection = toTarget.normalized;
                this.setVelocity();
            }
        }

        // called from UnityEvent -> AgentInput.cs Click/Touch
        public void SetDestination(MoveEvent moveEvent)
        {
            this.lastMoveEvent = moveEvent;
            this.targetPosition = moveEvent.InputWorldPosition;
            this.OnDirectionChange?.Invoke(moveEvent.InputWorldPosition);
        }

        private void setVelocity()
        {
            float tmpVelocity = this.CalculateSpeed(this.movementDirection, this.lastMoveEvent.SpeedModifer);
            if (this.currentVelocity != tmpVelocity)
            {
                this.currentVelocity = tmpVelocity;
                this.OnVelocityChange?.Invoke(this.currentVelocity);
            }

            this.rigidBody2d.velocity = this.currentVelocity * this.movementDirection.normalized;
        }

        private void stopMovement()
        {
            this.targetPosition = null;
            this.currentVelocity = 0f;
            this.movementDirection = Vector2.zero;
            this.setVelocity();
        }

        public void GetStunned(float duration)
        {
            if (this.isStunned == false)
            {
                this.stopMovement();
                this.StartCoroutine(this.StunnedCoroutine(duration));
            }
        }

        private IEnumerator StunnedCoroutine(float duration)
        {
            this.isStunned = true;
            yield return new WaitForSeconds(duration);
            this.isStunned = false;
        }

        private float CalculateSpeed(Vector2 target, float speedMultiplier)
        {
            if (target.sqrMagnitude > 0)
            { // has any value
                this.currentVelocity += this.movementData.acceleration * Time.fixedDeltaTime;
            }
            else
            {
                this.currentVelocity -= this.movementData.deacceleration * Time.fixedDeltaTime;
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