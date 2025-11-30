namespace JoJosAdventure
{
    public class PatrolPointDecision : AIDecision
    {
        public override bool MakeADecision()
        {
            if (this.AIMovementData.AgentMovement.IsAtDestination(
                this.AIMovementData.PatrolPoints[this.AIMovementData.CurrentPatrolIndex].transform.position))
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