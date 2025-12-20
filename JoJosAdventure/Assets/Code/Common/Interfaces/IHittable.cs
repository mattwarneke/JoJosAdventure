using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure.Common.Interfaces
{
    public interface IHittable
    {
        UnityEvent OnGetHit { get; set; }

        void GetHit(int damage, float? stunDuration);
    }
}