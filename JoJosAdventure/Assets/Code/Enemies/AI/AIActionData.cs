using UnityEngine;

namespace JoJosAdventure
{
    public class AIActionData : MonoBehaviour
    {
        [field: SerializeField]
        public bool Attack { get; set; }

        [field: SerializeField]
        public bool TargetSpotted { get; set; }

        [field: SerializeField]
        public bool Arrived { get; set; }
    }
}