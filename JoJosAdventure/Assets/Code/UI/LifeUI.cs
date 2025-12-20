using UnityEngine;

namespace JoJosAdventure
{
    public class LifeUI : MonoBehaviour
    {
        [field: SerializeField]
        public GameObject HeartGO { get; set; }

        public void SetHeart(bool isActive)
        {
            this.HeartGO.SetActive(isActive);
        }
    }
}