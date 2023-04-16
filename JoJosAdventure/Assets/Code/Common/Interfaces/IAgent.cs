using UnityEngine.Events;

namespace JoJosAdventure.Common.Interfaces
{
    public interface IAgent
    {
        int Health { get; }
        UnityEvent OnDie { get; set; }
        UnityEvent OnGetHit { get; set; }
    }
}