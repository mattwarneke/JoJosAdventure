using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure
{
    public class LivesController : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEvent<LivesEvent> OnLivesChanged { get; set; }

        private int lives = 3;
        private int maxLives = 3;

        protected void Start()
        {
            this.setLives(this.lives);
        }

        private void setLives(int lives)
        {
            this.lives = lives;
            this.OnLivesChanged.Invoke(new LivesEvent(this.lives, this.maxLives));
        }
    }

    public struct LivesEvent
    {
        public LivesEvent(int lives, int maxLives)
        {
            this.Lives = lives;
            this.MaxLives = maxLives;
        }

        public float Lives { get; set; }

        public float MaxLives { get; set; }
    }
}