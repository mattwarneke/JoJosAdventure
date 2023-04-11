using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure
{
    public interface IHittable
    {
        UnityEvent OnGetHit { get; set; }

        void GetHit(int damage, GameObject damageDealer);
    }
}