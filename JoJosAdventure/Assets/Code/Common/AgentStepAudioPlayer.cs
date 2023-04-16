using UnityEngine;

namespace JoJosAdventure.Common
{
    public class AgentStepAudioPlayer : AudioPlayer
    {
        [SerializeField]
        protected AudioClip stepClip;

        public void PlayStepSound()
        {
            this.PlayClipWithVariablePitch(this.stepClip);
        }
    }
}