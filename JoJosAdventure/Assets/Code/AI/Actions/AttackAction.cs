namespace JoJosAdventure
{
    public class AttackAction : AIAction
    {
        public override void TakeAction()
        {
            if (this.AIActionData.Attack == true)
                return;

            this.AIMovementData.PointOfInterest = this.EnemyBrain.Target.transform.position;
            this.EnemyBrain.Move(this.AIMovementData.PointOfInterest);
            this.AIActionData.Attack = true;
            this.EnemyBrain.Attack();
        }
    }
}