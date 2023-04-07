namespace JoJosAdventure.Animation
{
    public class HumanAnimations : CharacterAnimations
    {
        public override void SetAnimation(Acting acting, float speedOverride = 1f)
        {
            if (this.IsDead)
            {
                return;
            }

            this.Acting = acting;
            switch (acting)
            {
                case Acting.Idle:
                    this.Animator.Play("Idle");
                    break;

                case Acting.Walk:
                    this.Animator.SetBool("IsWalking", true);
                    this.Animator.SetFloat("WalkSpeed", 0.75f);
                    break;

                case Acting.Attack:
                    this.Animator.Play("walk");
                    break;

                case Acting.Grab:
                    this.Animator.Play("Grab");
                    break;
            }
        }
    }
}