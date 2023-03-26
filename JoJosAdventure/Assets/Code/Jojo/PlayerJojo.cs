using Assets.Code.Logic;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Code
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerJojo : MonoBehaviour
    {
        #region Member Variables

        private bool isFacingRight = true;

        protected Rigidbody2D rigidBody2d;

        [SerializeField]
        protected float speed = 3f;

        private bool IsScriptedActionPlaying { get; set; } = false;

        private Animator animator;

        #endregion Member Variables

        // Use this for initialization
        private void Awake()
        {
            // get the local reference
            this.animator = this.GetComponentInChildren<Animator>();
            this.rigidBody2d = this.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.paused)
            {
                this.rigidBody2d.velocity = new Vector2(0, 0);
                return;
            }

            if (this.IsScriptedActionPlaying)
            {
                this.animator.SetFloat("WalkSpeed", 2f);
                this.MoveToForActionScripted();
                return;
            }

            //MoveInputDirection directionX;
            //MoveInputDirection directionY;
            //Vector3? inputPosition = this.GetInputPosition();
            //if (inputPosition.HasValue)
            //{
            //    List<MoveInputDirection> inputDirections = this.getDirectionToInput(inputPosition.Value);
            //    directionX = inputDirections[0];
            //    directionY = inputDirections[1];
            //}
            //else
            //{
            //    directionX = InputCalculator.MovementInputX(Input.GetAxis("Horizontal"));
            //    directionY = InputCalculator.MovementInputY(Input.GetAxis("Vertical"));
            //}

            //float xMovement = this.speed.x * (float)directionX * 5f;
            //float yMovement = this.speed.y * (float)directionY * 5f;

            //// 4 - Movement per direction
            //this.movement = new Vector2(xMovement, yMovement);

            //this.rigidBody2d.velocity = this.movement;

            //if (directionX == MoveInputDirection.WalkRight
            //    && !this.isFacingRight)
            //{
            //    this.Flip();
            //}
            //else if (directionX == MoveInputDirection.WalkLeft
            //    && this.isFacingRight)
            //{
            //    this.Flip();
            //}

            //if (directionX == MoveInputDirection.NoMovement
            //    && directionY == MoveInputDirection.NoMovement)
            //{
            //    // we aren't moving so make sure we dont animate
            //    //animator.speed = 0.0f;
            //    this.animator.SetFloat("WalkSpeed", 0f);
            //}
            //else
            //{
            //    //animator.speed = 2f;
            //    this.animator.SetFloat("WalkSpeed", 2f);
            //}
        }

        public void Move(Vector2 movementInput)
        {
            this.rigidBody2d.velocity = movementInput * this.speed;
            //this.rigidBody2d.velocity = movementInput.normalized * this.speed;
        }

        public void Flip()
        {
            this.isFacingRight = !this.isFacingRight;
            //this.transform.parent.transform.Rotate(0, 180, 0);

            Vector3 currentScale = this.gameObject.transform.localScale;
            currentScale.x *= -1;
            this.gameObject.transform.localScale = currentScale;
        }

        public void StartPlayBedroomEnter(Transform transform)
        {
            this.moveToTransform = transform;
            //this.IsScriptedActionPlaying = true;
            this.PauseWalking(3);
        }

        private Transform moveToTransform;

        public void MoveToForActionScripted()
        {
            bool isMinDistanceFromTarget =
                Math.Abs(this.transform.position.x - this.moveToTransform.position.x) <= 1
                || Math.Abs(this.transform.position.y - this.moveToTransform.position.y) <= 1;
            if (isMinDistanceFromTarget)
            {
                this.PlaySwipAnimation();
                this.IsScriptedActionPlaying = false;
                GameService.Instance.JoJoNearJar();
                return;
            }

            this.transform.position = Vector2.Lerp(this.transform.position, this.moveToTransform.position, Time.deltaTime / 2);
        }

        public void PauseWalking(float pausedTime)
        {
            if (this.animator == null)
                return;
            this.animator.SetFloat("WalkSpeed", 0f);
            this.StartCoroutine(this.PauseMovement(pausedTime));
        }

        public void PauseWalking()
        {
            if (this.animator == null)
                return;
            this.animator.SetFloat("WalkSpeed", 0f);
            this.paused = true;
        }

        public void RestartWalking()
        {
            this.paused = false;
        }

        private bool paused = false;

        private IEnumerator PauseMovement(float pausedTime)
        {
            this.paused = true;
            yield return new WaitForSeconds(pausedTime);
            this.paused = false;
        }

        public void PlaySwipAnimation()
        {
            //StartCoroutine(PauseMovement(0.25f));
            this.animator.SetTrigger("attack");
        }
    }
}