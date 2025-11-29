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

        [field: SerializeField]
        public UnityEvent<MoveEvent> OnMovementPressed { get; set; }

        private void Awake()
        {
            this.mainCamera = Camera.main;
        }

        private void Update()
        {
            this.GetMovementInput();
        }

        private MoveEvent _moveEventCached = new MoveEvent(Vector2.zero);

        private void GetMovementInput()
        {
            if (Input.touchSupported
                && Input.touchCount > 0)
            {
                this._moveEventCached.Input = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButton(0))
            {
                this._moveEventCached.Input = Input.mousePosition;
            }
            else
            {
                // Legacy: Support WASD keyboard if feeling like it on PC
                this._moveEventCached.Input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }
            this.OnMovementPressed?.Invoke(this._moveEventCached);
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