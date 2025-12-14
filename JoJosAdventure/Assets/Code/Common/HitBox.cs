using UnityEngine;

namespace JoJosAdventure.Common
{
    [RequireComponent(typeof(Collider2D))]
    public class HitBox : MonoBehaviour
    {
        [field: SerializeField]
        public Collider2D HitBoxCollider { get; set; }
    }
}
