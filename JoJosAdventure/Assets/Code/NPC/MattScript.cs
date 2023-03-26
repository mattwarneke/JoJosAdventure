using JoJosAdventure.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MattScript : MonoBehaviour
{
    public Animator animator;
    public SpeechBubble speechBubble;
    public bool IsFollowing { get; private set; }
    public Transform transformFollowing;

    // Use this for initialization
    private void Start()
    {
        this.animator = this.GetComponent<Animator>();

        string repeatedSpeechText = "Jojo, Jojo!" + Environment.NewLine + "Where are you ?";
        List<Speech> startSpeech = new List<Speech>();
        startSpeech.Add(new Speech(repeatedSpeechText, 2));
        startSpeech.Add(new Speech(null, 1));
        startSpeech.Add(new Speech(repeatedSpeechText, 2));
        this.Speak(startSpeech);
    }

    private Queue<Vector3> followingPositions = new Queue<Vector3>();
    private Vector3? currentTarget;
    private float minDistanceY = 1.75f;
    private float minDistanceX = 2f;

    private void Update()
    {
        if (!this.IsFollowing || this.transformFollowing == null)
            return;
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
            this.animator.SetBool("IsWalking", true);
            this.animator.SetFloat("WalkSpeed", 0.75f);
            if (isMoreThanMinDistanceFromFollowing)
                this.transform.position = Vector2.Lerp(this.transform.position, this.currentTarget.Value, Time.deltaTime * 2);
        }
        else
        {
            this.animator.SetFloat("WalkSpeed", 0);
            this.animator.SetBool("IsWalking", false);
        }
    }

    public void ActivateFollow(Transform transformToFollow)
    {
        this.FollowTransform(transformToFollow);
        this.speechBubble.EmptySpeechQueue();
        this.animator.SetFloat("WalkSpeed", 0.75f);
        this.animator.SetBool("IsWalking", true);
    }

    private void FollowTransform(Transform transformToFollow)
    {
        this.IsFollowing = true;
        this.transformFollowing = transformToFollow;
        this.currentTarget = transformToFollow.position;
    }

    public void Speak(List<Speech> speech)
    {
        this.speechBubble.AddToSpeechQueue(speech);
    }

    public void ShowRing()
    {
        this.animator.SetTrigger("Ring");
    }
}