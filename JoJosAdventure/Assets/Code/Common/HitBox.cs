using UnityEngine;

namespace JoJosAdventure.Common
{
    [RequireComponent(typeof(Collider2D))]
    public class HitBox : MonoBehaviour
    {
        [field: SerializeField]
        public Collider2D Collider2D { get; set; }
    }
}
