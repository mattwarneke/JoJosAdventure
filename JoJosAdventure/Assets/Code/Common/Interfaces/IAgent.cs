using UnityEngine.Events;

namespace JoJosAdventure.Common.Interfaces
{
    public interface IAgent
    {
        UnityEvent OnDie { get; set; }
        UnityEvent OnGetHit { get; set; }
    }
}