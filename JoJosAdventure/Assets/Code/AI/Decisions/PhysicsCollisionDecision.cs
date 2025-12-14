using UnityEngine;
using JoJosAdventure.Common;

namespace JoJosAdventure
{
    public class PhysicsCollisionDecision : AIDecision
    {
        public override bool MakeADecision()
        {
            // Enemy has touched Jojo
            if (this.EnemyBrain.EnemyHitBox.Collider2D.IsTouching(this.EnemyBrain.TargetHitBox.Collider2D))
            {
                this.AIActionData.Attack = true;
                return true;
            }
            this.AIActionData.Attack = false;
            return false;
        }
    }
}