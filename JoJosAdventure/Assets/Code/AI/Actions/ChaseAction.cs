namespace JoJosAdventure
{
    public class ChaseAction : AIAction
    {
        public override void TakeAction()
        {
            var direction = this.EnemyBrain.Target.transform.position - this.transform.position;
            this.AIMovementData.Direction = direction.normalized;
            this.AIMovementData.PointOfInterest = this.EnemyBrain.Target.transform.position;
            this.EnemyBrain.Move(this.AIMovementData.Direction, this.AIMovementData.PointOfInterest);
        }
    }
}