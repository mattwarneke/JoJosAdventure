using UnityEngine;

namespace JoJosAdventure.Common
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AudioPlayer : MonoBehaviour
    {
        protected AudioSource audioSource;

        [SerializeField]
        protected float pitchRandomness = 0.5f;

        protected float basePitch;

        private void Awake()
        {
            this.audioSource = this.GetComponent<AudioSource>();
        }

        private void Start()
        {
            this.basePitch = this.audioSource.pitch;
        }

        protected void PlayClipWithVariablePitch(AudioClip clip)
        {
            var randomPitch = Random.Range(-this.pitchRandomness, this.pitchRandomness);
            this.audioSource.pitch = this.basePitch + randomPitch;
            this.PlayClip(clip);
        }

        protected void PlayClip(AudioClip clip)
        {
            this.audioSource.Stop();
            this.audioSource.clip = clip;
            this.audioSource.Play();
        }
    }
}
