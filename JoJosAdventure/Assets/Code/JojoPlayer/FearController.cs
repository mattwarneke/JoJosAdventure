using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JoJosAdventure
{
    public class FearController : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEvent<FearEvent> OnFearChanged { get; set; }

        private float Fear = 0f;
        private int MaxFear = 100;

        protected void Start()
        {
            this.setFear(this.Fear);
        }

        public void AddFear(float fear)
        {
            this.setFear(this.Fear + fear);
        }

        private void setFear(float fear)
        {
            this.Fear = fear;
            this.OnFearChanged.Invoke(new FearEvent(this.Fear, this.MaxFear));
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