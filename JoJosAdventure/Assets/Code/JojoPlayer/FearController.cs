using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JoJosAdventure
{
    public class FearController : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEvent<FearEvent> OnFearChanged { get; set; }

        private float fear = 0f;
        private int maxFear = 100;

        protected void Start()
        {
            this.setFear(this.fear);
        }

        private void setFear(float fear)
        {
            this.fear = fear;
            this.OnFearChanged.Invoke(new FearEvent(this.fear, this.maxFear));
        }
    }

    public struct FearEvent
    {
        public FearEvent(float fear, int maxFear)
        {
            this.Fear = fear;
            this.MaxFear = maxFear;
        }

        public float Fear { get; set; }

        public float MaxFear { get; set; }
    }
}