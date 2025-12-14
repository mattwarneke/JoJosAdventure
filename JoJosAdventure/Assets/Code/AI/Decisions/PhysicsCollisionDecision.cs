using UnityEngine;
using JoJosAdventure.Common;

namespace JoJosAdventure
{
    public class PhysicsCollisionDecision : AIDecision
    {
        public override bool MakeADecision()
        {
            // Enemy has touched Jojo
            if (this.EnemyBrain.EnemyHitBox.HitBoxCollider.IsTouching(this.EnemyBrain.TargetHitBox.HitBoxCollider))
            {
                this.AIActionData.Attack = true;
                return true;
            }
            this.AIActionData.Attack = false;
            return false;
        }
    }
}