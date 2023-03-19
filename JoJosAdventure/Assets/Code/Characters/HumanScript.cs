using Assets.Code.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HumanScript : MonoBehaviour
{
    public HumanAnimation humanAnimation;
    public FieldOfView fieldOfView;
    public SpeechBubble speechBubble;

    private bool IsFollowing = false;
    private Transform transformFollowing;

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
    private float minDistanceY = 1.75f;
    private float minDistanceX = 2f;

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
            && Math.Abs(this.transform.position.x - this.currentTarget.Value.x) <= this.minDistanceX + 0.25f
            && Math.Abs(this.transform.position.y - this.currentTarget.Value.y) <= this.minDistanceY + 0.25f;

        // close enough to current target, look for next target or stop.
        if (isAtCurrentTarget && this.followingPositions.Count > 0)
            this.currentTarget = this.followingPositions.Dequeue();
        else if (isAtCurrentTarget && this.followingPositions.Count == 0)
            this.currentTarget = null;

        if (this.currentTarget.HasValue)
        {
            this.humanAnimation.SetAnimation(Acting.Walk, 75f);

            if (isMoreThanMinDistanceFromFollowing)
                this.transform.position = Vector2.Lerp(this.transform.position, this.currentTarget.Value, Time.deltaTime * 2);
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

        this.humanAnimation.SetAnimation(Acting.Walk, 75f);
    }

    private void FollowTransform(Transform transformToFollow)
    {
        this.IsFollowing = true;
        this.transformFollowing = transformToFollow;
        this.currentTarget = transformToFollow.position;
    }

    private void Speak(Speech speech)
    {
        this.speechBubble.AddToSpeechQueue(speech);
    }
}