using UnityEngine;

namespace JoJosAdventure.Animation
{
    [RequireComponent(typeof(Animator))]
    public class JojoAnimations : MonoBehaviour
    {
        protected Animator Animator;

        public Acting Acting { get; protected set; }

        private void Awake()
        {
            this.Animator = this.GetComponent<Animator>();
        }

        public void SetWalkAnimation(bool val)
        {
            this.Animator.SetBool("Walk", val);
        }

        public void AnimatePlayer(float velocity)
        {
            this.SetWalkAnimation(velocity > 0);
        }
    }
}