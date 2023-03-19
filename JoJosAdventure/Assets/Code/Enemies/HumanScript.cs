using Assets.Code.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
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

            string repeatedSpeechText = "Here kitty, kitty, kitty!";
            this.Speak(new Speech(repeatedSpeechText, 2));
            this.Speak(new Speech(null, 1));
            this.Speak(new Speech(repeatedSpeechText, 2));
        }

        private Queue<Vector3> followingPositions = new Queue<Vector3>();
        private Vector3? currentTarget;
        private float minDistanceY = 1f;
        private float minDistanceX = 1f;

        private void Update()
        {
            if (!this.IsFollowing)
            {
                this.ScanForJojo();
                return;
            }

            // will follow the queue always even when close.
            bool IsExactFollow = false;// cant get it working smooth

            bool isMoreThanMinDistanceFromFollowing =// object to follow is far enough away, add waypoint to follow path.
                Math.Abs(this.transform.position.x - this.transformFollowing.position.x) > this.minDistanceX
                || Math.Abs(this.transform.position.y - this.transformFollowing.position.y) > this.minDistanceY;

            if (IsExactFollow || isMoreThanMinDistanceFromFollowing)
            {
                this.followingPositions.Enqueue(this.transformFollowing.position);
                if (this.currentTarget == null)
                    this.currentTarget = this.followingPositions.Dequeue();
            }

            bool isAtCurrentTarget =
                this.currentTarget.HasValue
                && Math.Abs(this.transform.position.x - this.currentTarget.Value.x) <= this.minDistanceX
                && Math.Abs(this.transform.position.y - this.currentTarget.Value.y) <= this.minDistanceY;

            // close enough to current target, look for next target or stop.
            if (isAtCurrentTarget && this.followingPositions.Count > 0)
                this.currentTarget = this.followingPositions.Dequeue();
            else if (isAtCurrentTarget && this.followingPositions.Count == 0)
                this.currentTarget = null;

            if (this.currentTarget.HasValue)
            {
                this.humanAnimation.SetAnimation(Acting.Walk, this.MoveSpeed);

                if (isMoreThanMinDistanceFromFollowing)
                {
                    this.transform.position = Vector2.Lerp(this.transform.position, this.currentTarget.Value, Time.deltaTime * this.MoveSpeed);
                }
                else
                {
                    this.GrabJojo();
                }

                // TODO something like this
                //this.Position = Vector2.Lerp(
                //  this.StartWaypointPos,
                //  this.HeadingToWayPoint.ToVector(),
                //  (Time.time - this.lastWaypointSwitchTime)// current time on path
                //     /(this.PathLength / this.MoveSpeed));
            }
            else
            {
                this.humanAnimation.StopWalk();
            }
        }

        private void ScanForJojo()
        {
            // TODO: some type of patrol pattern?
            if (!this.fieldOfView.targetAquired) return;

            this.ActivateFollow(this.fieldOfView.visibleTargets.First());
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
            this.currentTarget = transformToFollow.position;
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