using UnityEngine;
using UnityEngine.UI;

namespace JoJosAdventure
{
    public class FearMeter : MonoBehaviour
    {
        protected Slider fearSlider;

        protected void Awake()
        {
            this.fearSlider = this.GetComponent<Slider>();
        }

        // called from UnityEvent ->
        protected void UpdateFearMeter(float fear)
        {
            this.fearSlider.value = fear;
        }
    }
}