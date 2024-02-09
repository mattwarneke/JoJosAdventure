using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure.Common.Interfaces
{
    public interface IAgentInput
    {
        UnityEvent OnFireButtonPressed { get; set; }
        UnityEvent OnFireButtonReleased { get; set; }
        UnityEvent<MoveEvent> OnMovementPressed { get; set; }
        UnityEvent<Vector2> OnPointerPositionChanged { get; set; }
    }
}