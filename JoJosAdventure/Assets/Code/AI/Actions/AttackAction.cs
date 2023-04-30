using UnityEngine;

namespace JoJosAdventure
{
    public class AttackAction : AIAction
    {
        public override void TakeAction()
        {
            this.AIMovementData.Direction = Vector2.zero;
            this.AIMovementData.PointOfInterest = this.EnemyBrain.Target.transform.position;
            this.EnemyBrain.Move(this.AIMovementData.Direction, this.AIMovementData.PointOfInterest);
            this.AIActionData.Attack = true;
            this.EnemyBrain.Attack();
        }
    }
}