using JoJosAdventure.Common.Interfaces;
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
                this._moveEventCached.InputWorldPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                this.OnMovementPressed?.Invoke(this._moveEventCached);
            }
            else if (Input.GetMouseButton(0))
            {
                this._moveEventCached.InputWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.OnMovementPressed?.Invoke(this._moveEventCached);
            }
        }
    }
}