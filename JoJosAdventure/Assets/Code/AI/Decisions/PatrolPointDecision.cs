using UnityEngine;

namespace JoJosAdventure
{
    public class PatrolPointDecision : AIDecision
    {
        public override bool MakeADecision()
        {
            Vector2 toTarget = this.AIMovementData.PointOfInterest - (Vector2)this.transform.position;
            if (toTarget.magnitude < 0.25f)
            {
                this.AIMovementData.CurrentPatrolIndex = this.AIMovementData.CurrentPatrolIndex + 1 >= this.AIMovementData.PatrolPoints.Length
                    ? 0
                    : this.AIMovementData.CurrentPatrolIndex + 1;
                // At patrol point
                return true;
            }
            return false;
        }
    }
}