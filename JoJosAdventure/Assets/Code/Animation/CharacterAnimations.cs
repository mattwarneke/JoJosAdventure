using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class CharacterAnimations : MonoBehaviour
{
    protected bool IsDead;
    protected Animator Animator;

    public Acting Acting { get; protected set; }

    private void Awake()
    {
        this.Animator = this.GetComponent<Animator>();
        this.SetAnimation(Acting.Idle);
    }

    public void Idle() => this.SetAnimation(Acting.Idle);

    public void Walk() => this.SetAnimation(Acting.Walk);

    public void Attack() => this.SetAnimation(Acting.Attack);

    public void Grab() => this.SetAnimation(Acting.Grab);

    public void PlayAnimationComplete()
    {
        this.SetAnimation(Acting.Idle);
    }

    public void StopWalk()
    {
        this.Animator.SetFloat("WalkSpeed", 0);
        this.Animator.SetBool("IsWalking", false);
    }

    public abstract void SetAnimation(Acting acting, float speedOverride = 1f);
}