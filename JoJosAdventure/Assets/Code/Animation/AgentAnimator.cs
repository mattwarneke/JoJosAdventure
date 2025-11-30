using UnityEngine;

namespace JoJosAdventure.Animation
{
    [RequireComponent(typeof(Animator))]
    public class AgentAnimator : MonoBehaviour
    {
        protected Animator Animator;

        private void Awake()
        {
            this.Animator = this.GetComponent<Animator>();
        }

        public void SetWalkAnimation(bool val)
        {
            this.Animator.SetBool("Walk", val);
        }

        // called from UnityEvent -> AgentMovement:OnVelocityChange
        public void AnimatePlayer(float velocity)
        {
            this.SetWalkAnimation(velocity > 0);
        }
    }
}