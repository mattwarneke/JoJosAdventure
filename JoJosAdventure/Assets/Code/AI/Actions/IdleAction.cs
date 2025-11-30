using UnityEngine;

namespace JoJosAdventure
{
    public class IdleAction : AIAction
    {
        public override void TakeAction()
        {
            // TODO: Have a start location and return the AI to that location?

            // This stops movement to prevent it continuing to point of interest
            this.AIMovementData.Direction = Vector2.zero;
            this.AIMovementData.PointOfInterest = this.transform.position;
            this.EnemyBrain.Move(this.AIMovementData.PointOfInterest);
        }
    }
}