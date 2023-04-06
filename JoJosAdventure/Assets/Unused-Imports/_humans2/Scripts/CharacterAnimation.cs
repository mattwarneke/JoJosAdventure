using System;
using UnityEngine;

public abstract class CharacterAnimation : MonoBehaviour
{
    protected bool IsDead;
    protected Animator Animator;

    public Acting Acting { get; protected set; }

    protected void Start()
    {
        Animator = GetComponent<Animator>();
        SetAnimation(Acting.Idle);
    }

    public void Bow() => SetAnimation(Acting.Bow);
    public void Smash() => SetAnimation(Acting.Smash);
    public void Stab() => SetAnimation(Acting.Stab);
    public void Swing() => SetAnimation(Acting.Swing);
    public void Buffdown() => SetAnimation(Acting.Buffdown);
    public void Buffup() => SetAnimation(Acting.Buffup);
    public void Dance1() => SetAnimation(Acting.Dance1);
    public void Dance2() => SetAnimation(Acting.Dance2);
    public void Dance3() => SetAnimation(Acting.Dance3);
    public void Die() => SetAnimation(Acting.Die);
    public void Duck() => SetAnimation(Acting.Duck);
    public void Fall() => SetAnimation(Acting.Fall);
    public void Ground() => SetAnimation(Acting.Ground);
    public void Hurt() => SetAnimation(Acting.Hurt);
    public void Idle() => SetAnimation(Acting.Idle);
    public void Jump() => SetAnimation(Acting.Jump);
    public void Leap() => SetAnimation(Acting.Leap);
    public void Pull() => SetAnimation(Acting.Pull);
    public void Push() => SetAnimation(Acting.Push);
    public void Roll() => SetAnimation(Acting.Roll);
    public void Run() => SetAnimation(Acting.Run);
    public void Sad() => SetAnimation(Acting.Sad);
    public void Sit() => SetAnimation(Acting.Sit);
    public void Slide() => SetAnimation(Acting.Slide);
    public void Spring() => SetAnimation(Acting.Spring);
    public void Walk() => SetAnimation(Acting.Walk);

    public void PlayAnimationComplete()
    {
        SetAnimation(Acting.Idle);
    }

    protected abstract void SetAnimation(Acting acting);
}
