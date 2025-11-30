using JoJosAdventure.Common.Interfaces;

namespace JoJosAdventure
{
    public class EnemyAttackHug : EnemyAttack
    {
        public override void Attack(int damage)
        {
            if (this.waitBeforeNextAttack == false)
            {
                var hittable = this.GetTarget().GetComponent<IHittable>();
                hittable?.GetHit(damage, this.gameObject);
                this.StartCoroutine(this.WaitBeforeAttackCoroutine());
            }
        }
    }
}