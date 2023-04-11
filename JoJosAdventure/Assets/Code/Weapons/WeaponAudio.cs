using JoJosAdventure.Common;
using UnityEngine;

namespace JoJosAdventure
{
    public class WeaponAudio : AudioPlayer
    {
        [SerializeField]
        private AudioClip shootBulletClip = null, outOfBulletsClip = null;

        public void PlayShootSound()
        {
            this.PlayClip(this.shootBulletClip);
        }

        public void PlayNoBulletsSound()
        {
            this.PlayClip(this.outOfBulletsClip);
        }
    }
}
