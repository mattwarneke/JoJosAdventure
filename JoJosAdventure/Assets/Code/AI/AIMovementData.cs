using JoJosAdventure.Common;
using UnityEngine;

namespace JoJosAdventure
{
    public class AIMovementData : MonoBehaviour
    {
        [field: SerializeField]
        public GameObject[] PatrolPoints { get; set; }

        public int CurrentPatrolIndex = 0;

        [field: SerializeField]
        public float PatrolSpeedMultiplier { get; set; } = 0.5f;

        [field: SerializeField]
        public AgentMovement AgentMovement { get; set; }
    }
}