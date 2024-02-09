using UnityEngine;

namespace JoJosAdventure
{
    public class PatrolAction : AIAction
    {
        public override void TakeAction()
        {
            var direction = this.AIMovementData.PatrolPoints[this.AIMovementData.CurrentPatrolIndex].transform.position - this.transform.position;
            this.AIMovementData.Direction = direction.normalized;
            this.AIMovementData.PointOfInterest = this.AIMovementData.PatrolPoints[this.AIMovementData.CurrentPatrolIndex].transform.position;

            this.EnemyBrain.Move(this.AIMovementData.Direction, this.AIMovementData.PointOfInterest);
            this.EnemyBrain.EnemyFOV.RotateFOVToFollowPlayer(this.AIMovementData.PointOfInterest);

            // This could be moved to a decision but that feels clunky as it would effectively be a Patrol -> Patrol transition
            if (Vector3.Distance(this.transform.position, this.AIMovementData.PointOfInterest) < 0.1f)
            {
                this.AIMovementData.CurrentPatrolIndex = this.AIMovementData.CurrentPatrolIndex + 1 >= this.AIMovementData.PatrolPoints.Length
                    ? 0
                    : this.AIMovementData.CurrentPatrolIndex + 1;
            }
        }
    }
}