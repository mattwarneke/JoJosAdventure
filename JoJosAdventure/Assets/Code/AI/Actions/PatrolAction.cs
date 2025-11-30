namespace JoJosAdventure
{
    public class PatrolAction : AIAction
    {
        public override void TakeAction()
        {
            this.EnemyBrain.Move(this.AIMovementData.PatrolPoints[this.AIMovementData.CurrentPatrolIndex].transform.position, this.AIMovementData.PatrolSpeedMultiplier);
            this.EnemyBrain.EnemyFOV.RotateFOVTowardsTarget(this.AIMovementData.PatrolPoints[this.AIMovementData.CurrentPatrolIndex].transform.position);
        }
    }
}