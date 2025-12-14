using UnityEngine;

namespace JoJosAdventure
{
    public class FieldOfViewDecision : AIDecision
    {
        [field: SerializeField]
        [field: Range(0.1f, 10)]
        public float Distance { get; set; } = 5f;

        public override bool MakeADecision()
        {
            bool playerSpotted = this.EnemyBrain.EnemyFOV.LookForPlayer(this.EnemyBrain.TargetHitBox.Collider2D);
            if (playerSpotted)
            {
                this.AIActionData.TargetSpotted = true;
            }
            else
            {
                this.AIActionData.TargetSpotted = false;
            }
            return this.AIActionData.TargetSpotted;
        }
    }
}