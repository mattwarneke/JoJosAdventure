using UnityEngine;

namespace JoJosAdventure
{
    public class AIMovementData : MonoBehaviour
    {
        [field: SerializeField]
        public Vector2 Direction { get; set; }

        [field: SerializeField]
        public Vector2 PointOfInterest { get; set; }
    }
}