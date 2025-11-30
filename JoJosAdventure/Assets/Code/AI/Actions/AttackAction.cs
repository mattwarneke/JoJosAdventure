namespace JoJosAdventure
{
    public class AttackAction : AIAction
    {
        public override void TakeAction()
        {
            this.EnemyBrain.Move(this.EnemyBrain.Target.transform.position);
            this.EnemyBrain.Attack();
        }
    }
}