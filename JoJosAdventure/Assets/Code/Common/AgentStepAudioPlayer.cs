using JoJosAdventure.Common;
using UnityEngine;

namespace JoJosAdventure.JojoPlayer
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