using System.Collections.Generic;
using UnityEngine;

namespace JoJosAdventure
{
    public class LivesMeterUI : MonoBehaviour
    {
        [field: SerializeField]
        public GameObject HeartPrefab { get; set; }

        private List<LifeUI> Hearts { get; set; } = new List<LifeUI>();

        private float heightBetweenHearts = 300f;

        public void UpdateLives(LivesEvent livesEvent)
        {
            if (livesEvent.MaxLives > this.Hearts.Count)
            {   // might not need the if, but whatever
                for (int i = this.Hearts.Count; livesEvent.MaxLives > this.Hearts.Count; i++)
                {
                    GameObject heartGO = Instantiate(HeartPrefab, this.transform);
                    heartGO.transform.localPosition = new Vector2(0, i * this.heightBetweenHearts);
                    this.Hearts.Add(heartGO.GetComponent<LifeUI>());
                }
            }
            else if (livesEvent.MaxLives < this.Hearts.Count)
            {
                throw new UnityException("NOT IMPLEMENTED: Reduce max life");
            }

            for (int i = Hearts.Count-1; i >= 0; i--)
            {
                this.Hearts[i].SetHeart(livesEvent.Lives > i);
            }
        }
    }
}