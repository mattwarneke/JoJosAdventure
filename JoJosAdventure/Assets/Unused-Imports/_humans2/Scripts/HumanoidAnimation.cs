using System;
using UnityEngine;

public class HumanoidAnimation : CharacterAnimation
{

    protected override void SetAnimation(Acting acting)
    {
        if (IsDead)
        {
            return;
        }

        Acting = acting;
        switch (acting)
        {
            case Acting.Bow:
                Animator.Play("attack-bow");
                break;
            case Acting.Smash:
                Animator.Play("attack-smash");
                break;
            case Acting.Stab:
                Animator.Play("attack-stab");
                break;
            case Acting.Swing:
                Animator.Play("attack-swing");
                break;
            case Acting.Buffdown:
                Animator.Play("buff-down");
                break;
            case Acting.Buffup:
                Animator.Play("buff-up");
                break;
            case Acting.Dance1:
                Animator.Play("dance1");
                break;
            case Acting.Dance2:
                Animator.Play("dance2");
                break;
            case Acting.Dance3:
                Animator.Play("dance3");
                break;
            case Acting.Die:
                Animator.Play("die");
                break;
            case Acting.Duck:
                Animator.Play("duck");
                break;
            case Acting.Fall:
                Animator.Play("fall");
                break;
            case Acting.Ground:
                Animator.Play("ground");
                break;
            case Acting.Hurt:
                Animator.Play("hurt");
                break;
            case Acting.Idle:
                Animator.Play("idle");
                break;
            case Acting.Jump:
                Animator.Play("jump");
                break;
            case Acting.Leap:
                Animator.Play("leap");
                break;
            case Acting.Pull:
                Animator.Play("pull");
                break;
            case Acting.Push:
                Animator.Play("push");
                break;
            case Acting.Roll:
                Animator.Play("roll");
                break;
            case Acting.Run:
                Animator.Play("run");
                break;
            case Acting.Sad:
                Animator.Play("sad");
                break;
            case Acting.Sit:
                Animator.Play("sit");
                break;
            case Acting.Slide:
                Animator.Play("slide-down");
                break;
            case Acting.Spring:
                Animator.Play("spring");
                break;
            case Acting.Walk:
                Animator.Play("walk");
                break;
        }
    }

}
