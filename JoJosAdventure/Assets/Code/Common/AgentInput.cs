using Assets.Code.Enums;
using JoJosAdventure.Common.Interfaces;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure.Common
{
    public class AgentInput : MonoBehaviour, IAgentInput
    {
        private Camera mainCamera;
        private bool fireButtonDown = false;

        [field: SerializeField]
        public UnityEvent<Vector2> OnMovementPressed { get; set; }

        [field: SerializeField]
        public UnityEvent<Vector2> OnPointerPositionChanged { get; set; }

        [field: SerializeField]
        public UnityEvent OnFireButtonPressed { get; set; }

        [field: SerializeField]
        public UnityEvent OnFireButtonReleased { get; set; }

        private void Awake()
        {
            this.mainCamera = Camera.main;
        }

        private void Update()
        {
            this.GetMovementInput();
            this.GetPointerInput();
            this.GetFireInput();
        }

        private void GetFireInput()
        {
            if (Input.GetAxisRaw("Fire1") > 0)
            {
                if (this.fireButtonDown == false)
                {
                    this.fireButtonDown = true;
                    this.OnFireButtonPressed?.Invoke();
                }
            }
            else
            {
                if (this.fireButtonDown == true)
                {
                    this.fireButtonDown = false;
                    this.OnFireButtonReleased?.Invoke();
                }
            }
        }

        private void GetPointerInput()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = this.mainCamera.nearClipPlane;
            var mouseInWorldSpace = this.mainCamera.ScreenToWorldPoint(mousePos);
            this.OnPointerPositionChanged?.Invoke(mouseInWorldSpace);
        }

        private void GetMovementInput()
        {
            //if (Input.touchSupported
            //    && Input.touchCount > 0)
            //{
            //    this.OnMovementPressed?.Invoke(
            //        this.getDirectionToInput(Input.GetTouch(0).position));
            //}
            //else if (Input.GetMouseButton(0))
            //{
            //    this.OnMovementPressed?.Invoke(this.getDirectionToInput(Input.mousePosition));
            //}
            //else
            //{
            this.OnMovementPressed?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            //}
        }

        private Vector2 getDirectionToInput(Vector2 inputPosition)
        {
            inputPosition = this.mainCamera.ScreenToWorldPoint(inputPosition);
            MoveInputDirection[] inputDirections = new MoveInputDirection[2];
            // is input a tiny bit away from character - stops jittering.
            if (Math.Abs(inputPosition.x - this.transform.position.x) > 0.25f)
            {   // x increases to the right
                if (inputPosition.x < this.transform.position.x)
                    inputDirections[0] = MoveInputDirection.WalkLeft;
                else if (inputPosition.x > this.transform.position.x)
                    inputDirections[0] = MoveInputDirection.WalkRight;
            }

            if (Math.Abs(inputPosition.y - this.transform.position.y) > 0.25f)
            {
                if (inputPosition.y > this.transform.position.y)
                    inputDirections[1] = MoveInputDirection.WalkUp;
                else if (inputPosition.y < this.transform.position.y)
                    inputDirections[1] = MoveInputDirection.WalkDown;
            }

            return new Vector2((float)inputDirections[0], (float)inputDirections[1]);
        }
    }
}