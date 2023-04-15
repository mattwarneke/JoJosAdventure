using JoJosAdventure.Logic;
using JoJosAdventure.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure.Enemies
{
    public class EnemyScript : MonoBehaviour, IHittable
    {
        public FieldOfView fieldOfView;
        public SpeechBubble speechBubble;

        private bool IsFollowing = false;
        private Transform transformFollowing;

        // Use this for initialization
        private void Start()
        {
            this.Health = this.EnemyData.MaxHealth;

            if (this.fieldOfView == null) throw new ExpectedInspectorReferenceException();

            this.fieldOfView.OnPlayerSpotted += this.playerSpotted;

            string repeatedSpeechText = "Here kitty, kitty, kitty!";
            this.Speak(new Speech(repeatedSpeechText, 2));
            this.Speak(new Speech(null, 1));
            this.Speak(new Speech(repeatedSpeechText, 2));
        }

        private void Update()
        {
            if (this.IsFollowing)
            {
                this.FollowPlayerInSight();
            }
            else
            {
                // Patrol logic
            }
        }

        [field: SerializeField]
        public EnemyDataSO EnemyData { get; set; }

        [field: SerializeField]
        public int Health { get; private set; }

        public GameObject FlipContainer;
        private bool isFacingRight => this.FlipContainer.transform.eulerAngles.y < 180;

        [field: SerializeField]
        public UnityEvent OnGetHit { get; set; }

        [field: SerializeField]
        public UnityEvent OnDie { get; set; }

        public void GetHit(int damage, GameObject damageDealer)
        {
            this.Health--;
            this.OnGetHit?.Invoke();
            if (this.Health <= 0)
            {
                this.OnDie?.Invoke();
                this.StartCoroutine(this.WaitToDie());
            }
        }

        private IEnumerator WaitToDie()
        {
            yield return new WaitForSeconds(0.54f);
            Destroy(this.gameObject);
        }

        private void playerSpotted(Transform transform)
        {
            if (!this.IsFollowing)
            {
                this.ActivateFollow(transform);
            }
        }

        private void ActivateFollow(Transform transformToFollow)
        {
            this.FollowTransform(transformToFollow);

            this.Speak(new Speech("Hug the kitty!!!", 2));
        }

        private void FollowTransform(Transform transformToFollow)
        {
            this.IsFollowing = true;
            this.transformFollowing = transformToFollow;
        }

        private void PlayerOutOfSite()
        {
            this.Speak(new Speech("Where did the kitty go?", 2));
            // Navigate back to patrol
            this.IsFollowing = false;
        }

        private void FollowPlayerInSight()
        {
            // smarter routing with a queue? But then could stand still if player does or run wrong way
            //    this.walkingToSpot = this.followingPositions.Dequeue();

            //this.transform.position = Vector2.Lerp(this.transform.position, this.transformFollowing.position, Time.deltaTime * this.MoveSpeed);

            this.flipToFacePlayer();

            if (!this.fieldOfView.PlayerInSight)
            {
                this.PlayerOutOfSite();
            }
        }

        private void flipToFacePlayer()
        {
            if (this.transformFollowing.position.x >= this.transform.position.x
                && !this.isFacingRight)
            {
                this.Flip();
            }
            else if (this.transformFollowing.position.x < this.transform.position.x
                && this.isFacingRight)
            {
                this.Flip();
            }
        }

        private void Flip()
        {
            this.FlipContainer.transform.Rotate(0, 180, 0);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            // This means bumping into the back would capture
            // An alternative is to check contact with player collider in follow
            if (!LayersUtil.IsColliderPlayer(col.collider)) return;

            this.GrabJojo();
        }

        private void GrabJojo()
        {
            this.Speak(new Speech("Has kitty!!!", 2));
        }

        private void Speak(Speech speech)
        {
            this.speechBubble.AddToSpeechQueue(speech);
        }
    }
}