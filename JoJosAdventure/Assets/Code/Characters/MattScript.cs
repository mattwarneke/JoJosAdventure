using Assets.Code.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattScript : MonoBehaviour
{
    private Animator animator;
    public SpeechBubble speechBubble;
    public bool IsFollowing { get; private set; }
    public Transform transformFollowing;
	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();

        string repeatedSpeechText = "Jojo, Jojo!" + Environment.NewLine + "Where are you ?";
        List<Speech> startSpeech = new List<Speech>();
        startSpeech.Add(new Speech(repeatedSpeechText, 2));
        startSpeech.Add(new Speech(null, 1));
        startSpeech.Add(new Speech(repeatedSpeechText, 2));
        Speak(startSpeech);
    }

    Queue<Vector3> followingPositions = new Queue<Vector3>();
    Vector3? currentTarget;
    float minDistanceY = 1.75f;
    float minDistanceX = 2f;
    void Update ()
    {
        if (!IsFollowing || this.transformFollowing == null)
            return;
        // will follow the queue always even when close.
        bool IsExactFollow = false;// cant get it working smooth

        bool isMoreThanMinDistanceFromFollowing =// object to follow is far enough away, add waypoint to follow path.
            Math.Abs(this.transform.position.x - this.transformFollowing.position.x) > minDistanceX
            || Math.Abs(this.transform.position.y - this.transformFollowing.position.y) > minDistanceY;
       
        if (IsExactFollow || isMoreThanMinDistanceFromFollowing)
        {
            followingPositions.Enqueue(this.transformFollowing.position);
            if (currentTarget == null)
                currentTarget = followingPositions.Dequeue();
        }

        bool isAtCurrentTarget = 
            currentTarget.HasValue
            && Math.Abs(this.transform.position.x - this.currentTarget.Value.x) <= minDistanceX + 0.25f
            && Math.Abs(this.transform.position.y - this.currentTarget.Value.y) <= minDistanceY + 0.25f;
    
        // close enough to current target, look for next target or stop.
        if (isAtCurrentTarget && followingPositions.Count > 0)
            currentTarget = followingPositions.Dequeue();
        else if (isAtCurrentTarget && followingPositions.Count == 0)
            currentTarget = null;

        if (currentTarget.HasValue)
        {
            animator.SetBool("IsWalking", true);
            animator.SetFloat("WalkSpeed", 0.75f);
            if (isMoreThanMinDistanceFromFollowing)
                this.transform.position = Vector2.Lerp(this.transform.position, this.currentTarget.Value, Time.deltaTime * 2);
        }
        else
        {
            animator.SetFloat("WalkSpeed", 0);
            animator.SetBool("IsWalking", false);
        }
    }
    
    public void ActivateFollow(Transform transformToFollow)
    {
        FollowTransform(transformToFollow);
        speechBubble.EmptySpeechQueue();
        animator.SetFloat("WalkSpeed", 0.75f);
        animator.SetBool("IsWalking", true);
    }

    private void FollowTransform(Transform transformToFollow)
    {
        this.IsFollowing = true;
        this.transformFollowing = transformToFollow;
        this.currentTarget = transformToFollow.position;
    }
    
    public void Speak(List<Speech> speech)
    {
        speechBubble.AddToSpeechQueue(speech);
    }

    public void ShowRing()
    {
        animator.SetTrigger("Ring");
    }
}
