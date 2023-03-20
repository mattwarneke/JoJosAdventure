using Assets.Code.Enums;
using Assets.Code.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
    public class PlayerJojo : MonoBehaviour
    {
        #region Member Variables

        /// <summary>
        /// Animation state machine local reference
        /// </summary>
        private Animator animator;

        /// <summary>
        /// The last checkpoint position that we have saved
        /// </summary>
        private Vector3 CheckPointPosition;

        /// <summary>
        /// Is the player dead?
        /// </summary>
        private bool isDead = false;

        private List<SpriteRenderer> AllChildSprites;
        //public GameObject CharacterBody;

        private bool isFacingRight = true;

        #endregion Member Variables

        // Use this for initialization
        private void Start()
        {
            // get the local reference
            this.animator = this.GetComponentInChildren<Animator>();

            this.CheckPointPosition = this.transform.position;

            this.AllChildSprites = this.GetComponentsInChildren<SpriteRenderer>().ToList();
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.paused)
            {
                this.movement = new Vector2(0, 0);
                this.body.velocity = this.movement;
                return;
            }

            if (this.IsScriptedActionPlaying)
            {
                this.animator.SetFloat("WalkSpeed", 2f);
                this.MoveToForActionScripted();
                return;
            }
            MoveInputDirection directionX;
            MoveInputDirection directionY;
            Vector3? inputPosition = this.GetInputPosition();
            if (inputPosition.HasValue)
            {
                List<MoveInputDirection> inputDirections = this.getDirectionToInput(inputPosition.Value);
                directionX = inputDirections[0];
                directionY = inputDirections[1];
            }
            else
            {
                directionX = InputCalculator.MovementInputX(Input.GetAxis("Horizontal"));
                directionY = InputCalculator.MovementInputY(Input.GetAxis("Vertical"));
            }

            float xMovement = this.speed.x * (float)directionX * 5f;
            float yMovement = this.speed.y * (float)directionY * 5f;

            // 4 - Movement per direction
            this.movement = new Vector2(xMovement, yMovement);

            this.body.velocity = this.movement;

            if (directionX == MoveInputDirection.WalkRight
                && !this.isFacingRight)
            {
                this.Flip();
            }
            else if (directionX == MoveInputDirection.WalkLeft
                && this.isFacingRight)
            {
                this.Flip();
            }

            if (directionX == MoveInputDirection.NoMovement
                && directionY == MoveInputDirection.NoMovement)
            {
                // we aren't moving so make sure we dont animate
                //animator.speed = 0.0f;
                this.animator.SetFloat("WalkSpeed", 0f);
            }
            else
            {
                //animator.speed = 2f;
                this.animator.SetFloat("WalkSpeed", 2f);
            }

            // if we are dead do not move anymore
            if (this.isDead == true)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                this.animator.speed = 0.0f;
            }
        }

        /// <summary>
        /// 1 - The speed of the ship
        /// </summary>
        public Vector2 speed = new Vector2(0.8f, 0.8f);

        // 2 - Store the movement
        private Vector2 movement;

        public Rigidbody2D body;

        public bool IsScriptedActionPlaying { get; set; }

        private void OnStart()
        {
            this.IsScriptedActionPlaying = false;
        }

        private Vector3? lastInputPosition;

        private Vector3? GetInputPosition()
        {
            if (Input.touchSupported
                && Input.touchCount > 0
                && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                Touch touch = Input.GetTouch(0);
                this.lastInputPosition = Camera.main.ScreenToWorldPoint(touch.position);
            }
            else if (Input.GetMouseButton(0))
            {
                this.lastInputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            return this.lastInputPosition;
        }

        private List<MoveInputDirection> getDirectionToInput(Vector3 inputPosition)
        {
            List<MoveInputDirection> inputDirections = new List<MoveInputDirection>();
            // is input a tiny bit away from character - stops jittering.
            if (Math.Abs(inputPosition.x - this.transform.position.x) > 0.25f)
            {   // x increases to the right
                if (inputPosition.x < this.transform.position.x)
                    inputDirections.Add(MoveInputDirection.WalkLeft);
                else if (inputPosition.x > this.transform.position.x)
                    inputDirections.Add(MoveInputDirection.WalkRight);
            }
            if (inputDirections.Count == 0)
                inputDirections.Add(MoveInputDirection.NoMovement);

            if (Math.Abs(inputPosition.y - this.transform.position.y) > 0.25f)
            {
                if (inputPosition.y > this.transform.position.y)
                    inputDirections.Add(MoveInputDirection.WalkUp);
                else if (inputPosition.y < this.transform.position.y)
                    inputDirections.Add(MoveInputDirection.WalkDown);
            }

            if (inputDirections.Count <= 1)
                inputDirections.Add(MoveInputDirection.NoMovement);

            return inputDirections;
        }

        public void Flip()
        {
            this.isFacingRight = !this.isFacingRight;
            //this.transform.parent.transform.Rotate(0, 180, 0);

            Vector3 currentScale = this.gameObject.transform.localScale;
            currentScale.x *= -1;
            this.gameObject.transform.localScale = currentScale;
        }

        private void FixedUpdate()
        {
            // 5 - Move the game object
            this.body.velocity = this.movement;
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
            this.lastInputPosition = null;
        }

        public void RestartWalking()
        {
            this.paused = false;
        }

        private bool paused = false;

        private IEnumerator PauseMovement(float pausedTime)
        {
            this.paused = true;
            this.lastInputPosition = null;
            yield return new WaitForSeconds(pausedTime);
            this.paused = false;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "DangerousTile")
            {
                GameObject.Find("FadePanel").GetComponent<FadeScript>().RespawnFade();
                this.isDead = true;
            }
            else if (collider.gameObject.tag == "LevelChanger")
            {
                GameObject.Find("FadePanel").GetComponent<FadeScript>().FadeOut();
                this.isDead = true;
            }
        }

        /// <summary>
        /// Respawns the player at checkpoint.
        /// </summary>
        public void RespawnPlayerAtCheckpoint()
        {
            // if we hit a dangerous tile then we are dead so go to the checkpoint position that was last saved
            this.transform.position = this.CheckPointPosition;
            this.isDead = false;
        }

        public void PlaySwipAnimation()
        {
            //StartCoroutine(PauseMovement(0.25f));
            this.animator.SetTrigger("attack");
        }
    }
}