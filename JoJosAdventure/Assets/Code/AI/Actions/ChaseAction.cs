namespace JoJosAdventure
{
    public class ChaseAction : AIAction
    {
        public override void TakeAction()
        {
            this.EnemyBrain.Move(this.EnemyBrain.Target.transform.position);
            this.EnemyBrain.EnemyFOV.RotateFOVTowardsTarget(this.EnemyBrain.Target.transform.position);
        }
    }
}