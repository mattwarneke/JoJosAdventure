using Assets.Code.Logic;
using JoJosAdventure.Utils;
using UnityEngine;

namespace JoJosAdventure.Enemies
{
    public class HumanScript : MonoBehaviour
    {
        public HumanAnimation humanAnimation;
        public FieldOfView fieldOfView;
        public SpeechBubble speechBubble;

        private bool IsFollowing = false;
        private Transform transformFollowing;

        public float MoveSpeed = 1f;

        // Use this for initialization
        private void Start()
        {
            if (this.humanAnimation == null) throw new ExpectedInspectorReferenceException();
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

            this.humanAnimation.SetAnimation(Acting.Walk, this.MoveSpeed);
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
            this.humanAnimation.StopWalk();
            this.IsFollowing = false;
        }

        private void FollowPlayerInSight()
        {
            // smarter routing with a queue? But then could stand still if player does or run wrong way
            //    this.walkingToSpot = this.followingPositions.Dequeue();

            this.transform.position = Vector2.Lerp(this.transform.position, this.transformFollowing.position, Time.deltaTime * this.MoveSpeed);

            this.lookAtPlayer();

            if (!this.fieldOfView.PlayerInSight)
            {
                this.PlayerOutOfSite();
            }
        }

        private void lookAtPlayer()
        {
            float rotationSpeed = 0.5f;

            // 1st attempt
            //this.transform.right = Vector3.Lerp(this.transform.right, this.transformFollowing.position - this.transform.position, rotationSpeed);

            // 2nd attempt
            //Vector3 relativeTarget = (this.transformFollowing.position - this.transform.position).normalized;
            ////Vector3.right if you have a sprite rotated in the right direction
            //Quaternion toQuaternion = Quaternion.FromToRotation(Vector3.right, relativeTarget);
            //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, toQuaternion, rotationSpeed * Time.deltaTime);

            // 3rd attempt
            //Vector3 dir = this.transformFollowing.position - this.transform.position;
            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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

            this.humanAnimation.SetAnimation(Acting.Grab);
        }

        private void Speak(Speech speech)
        {
            this.speechBubble.AddToSpeechQueue(speech);
        }
    }
}