using Assets.Code.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Code.Inputs
{
    public class AgentInput : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEvent<Vector2> OnMovementPressed { get; set; }

        private void Update()
        {
            this.GetMovementInput();
        }

        private void GetMovementInput()
        {
            if (Input.touchSupported
                && Input.touchCount > 0)
            {
                this.OnMovementPressed?.Invoke(
                    this.getDirectionToInput(Input.GetTouch(0).position));
            }
            else if (Input.GetMouseButton(0))
            {
                this.OnMovementPressed?.Invoke(this.getDirectionToInput(Input.mousePosition));
            }
            else
            {
                this.OnMovementPressed?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            }
        }

        private Vector2 getDirectionToInput(Vector2 inputPosition)
        {
            inputPosition = Camera.main.ScreenToWorldPoint(inputPosition);
            MoveInputDirection[] inputDirections = new MoveInputDirection[2];
            // is input a tiny bit away from character - stops jittering.
            //if (Math.Abs(inputPosition.x - this.transform.position.x) > 0.25f)
            //{   // x increases to the right
            if (inputPosition.x < this.transform.position.x)
                inputDirections[0] = MoveInputDirection.WalkLeft;
            else if (inputPosition.x > this.transform.position.x)
                inputDirections[0] = MoveInputDirection.WalkRight;
            else if (inputDirections.Length == 0)
                inputDirections[0] = MoveInputDirection.NoMovement;
            //}

            //if (Math.Abs(inputPosition.y - this.transform.position.y) > 0.25f)
            //{
            if (inputPosition.y > this.transform.position.y)
                inputDirections[1] = MoveInputDirection.WalkUp;
            else if (inputPosition.y < this.transform.position.y)
                inputDirections[1] = MoveInputDirection.WalkDown;
            else
                inputDirections[1] = MoveInputDirection.NoMovement;
            //}

            //if (inputDirections.Count <= 1)
            //    inputDirections.Add(MoveInputDirection.NoMovement);

            return new Vector2((float)inputDirections[0], (float)inputDirections[1]);
        }
    }
}