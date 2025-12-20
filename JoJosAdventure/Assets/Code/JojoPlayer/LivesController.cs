using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure
{
    public class LivesController : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEvent<LivesEvent> OnLivesChanged { get; set; }

        private int lives = 0;
        private int maxLives = 3;

        protected void Start()
        {
            this.lives = this.maxLives;
            this.setLives(this.lives);
        }

        public void TakeDamage(int damage)
        {
            this.setLives(this.lives - damage);
        }

        public bool IsAlive()
        {
            return lives > 0;
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