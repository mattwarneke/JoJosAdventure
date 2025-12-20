using UnityEngine;
using UnityEngine.UI;

namespace JoJosAdventure
{
    public class FearMeterUI : MonoBehaviour
    {
        private Slider fearSlider { get; set; }

        protected void Awake()
        {
            this.fearSlider = this.GetComponent<Slider>();
        }

        // called from UnityEvent ->
        public void UpdateFearMeter(FearEvent fearEvent)
        {
            this.fearSlider.value = fearEvent.Fear/ (float)fearEvent.MaxFear;
        }
    }
}