namespace JoJosAdventure
{
    public class AttackAction : AIAction
    {
        public override void TakeAction()
        {
            this.AIMovementData.PointOfInterest = this.EnemyBrain.Target.transform.position;
            this.EnemyBrain.Move(this.AIMovementData.PointOfInterest);
            this.EnemyBrain.Attack();
        }
    }
}