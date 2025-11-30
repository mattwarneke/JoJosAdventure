namespace JoJosAdventure
{
    public class PatrolAction : AIAction
    {
        public override void TakeAction()
        {
            this.AIMovementData.PointOfInterest = this.AIMovementData.PatrolPoints[this.AIMovementData.CurrentPatrolIndex].transform.position;

            this.EnemyBrain.Move(this.AIMovementData.PointOfInterest, this.AIMovementData.PatrolSpeedMultiplier);
            this.EnemyBrain.EnemyFOV.RotateFOVTowardsTarget(this.AIMovementData.PointOfInterest);
        }
    }
}