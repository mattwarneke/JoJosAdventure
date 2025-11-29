using UnityEngine.Events;

namespace JoJosAdventure.Common.Interfaces
{
    public interface IAgentInput
    {
        UnityEvent<MoveEvent> OnMovementPressed { get; set; }
    }
}