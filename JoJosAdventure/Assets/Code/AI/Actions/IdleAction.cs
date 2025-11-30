namespace JoJosAdventure
{
    public class IdleAction : AIAction
    {
        public override void TakeAction()
        {
            // TODO: Have a start location and return the AI to that location?

            this.AIMovementData.PointOfInterest = this.transform.position;
            this.EnemyBrain.Move(this.AIMovementData.PointOfInterest);
        }
    }
}