using UnityEngine;

namespace JoJosAdventure
{
    public class IdleAction : AIAction
    {
        public override void TakeAction()
        {
            this.AIMovementData.Direction = Vector2.zero;
            this.AIMovementData.PointOfInterest = this.transform.position;
            this.EnemyBrain.Move(this.AIMovementData.Direction, this.AIMovementData.PointOfInterest);
        }
    }
}