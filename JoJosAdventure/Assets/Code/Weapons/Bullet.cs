using JoJosAdventure.ScriptableObjects;
using UnityEngine;

namespace JoJosAdventure.Weapons
{
    public abstract class Bullet : MonoBehaviour
    {
        [field: SerializeField]
        public virtual BulletDataSO BulletData { get; set; }
    }
}