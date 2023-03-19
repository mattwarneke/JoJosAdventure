public class HumanAnimation : CharacterAnimation
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
                this.Animator.Play("idle");
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