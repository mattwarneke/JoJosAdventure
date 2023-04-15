using JoJosAdventure.Common;
using UnityEngine;

namespace JoJosAdventure
{
    public class AgentSounds : AudioPlayer
    {
        [SerializeField]
        private AudioClip hitClip = null, deathClip = null, voiceLineClip = null;

        public void PlayHitSound()
        {
            this.PlayClipWithVariablePitch(this.hitClip);
        }

        public void PlayDeathSound()
        {
            this.PlayClip(this.deathClip);
        }

        public void PlayVoiceSound()
        {
            this.PlayClip(this.voiceLineClip);
        }
    }
}