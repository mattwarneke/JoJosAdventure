namespace JoJosAdventure
{
    public class ChaseAction : AIAction
    {
        public override void TakeAction()
        {
            this.AIMovementData.PointOfInterest = this.EnemyBrain.Target.transform.position;
            this.EnemyBrain.Move(this.AIMovementData.PointOfInterest);
            this.EnemyBrain.EnemyFOV.RotateFOVToFollowPlayer(this.EnemyBrain.Target.transform.position);
        }
    }
}