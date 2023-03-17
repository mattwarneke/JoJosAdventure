using System;
using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Member Variables

    /// <summary>
    /// The distance the player can move before the camera follows
    /// </summary>
    public Vector2 Margins;

    /// <summary>
    /// The player character
    /// </summary>
    public GameObject PlayerCharacter;

    /// <summary>
    ///  Reference to the users current view transform.
    /// </summary>
    private Transform PlayerTransform;

    /// <summary>
    /// Clamp Camera within defined Bounds
    /// </summary>
    public bool UseBounds;

    /// <summary>
    /// The maximum x and y coordinates the camera can have
    /// </summary>
    public Vector2 MAXBounds;

    /// <summary>
    /// The minimum x and y coordinates the camera can have
    /// </summary>
    public Vector2 MINBounds;

    #endregion Member Variables

    private void Start()
    {
        // get the players transform
        this.PlayerTransform = this.PlayerCharacter.transform;
        this.transform.position = new Vector3(this.PlayerCharacter.transform.position.x, this.PlayerCharacter.transform.position.y, this.transform.position.z);
        this.followingTarget = this.PlayerTransform.position;
    }

    public void SetCustomPanTarget(Vector3 target)
    {
        this.CustomTarget = true;
        this.followingTarget = target;
    }

    private float cameraLerpSpeed = 3f;
    private Vector2 target;
    private bool CustomTarget = false;
    private Vector3 followingTarget;

    private void Update()
    {
        if (this.CustomTarget)
        {
            this.cameraLerpSpeed = 2.5f;
        }
        else
        {
            this.followingTarget = this.PlayerCharacter.transform.position;
            this.cameraLerpSpeed = 4f;
        }

        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        this.target = new Vector2(this.transform.position.x, this.transform.position.y);

        // If the player has moved beyond the x margin
        if (this.CheckMovementXMargin())
        {
            // the target X-coordinate should be a Lerp between the camera's current x position and the player's current x position.
            this.target.x = Mathf.Lerp(this.transform.position.x, this.followingTarget.x, this.cameraLerpSpeed * Time.deltaTime);
        }

        // If the player has moved beyond the y margin
        if (this.CheckMovementYMargin())
        {
            // The target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            this.target.y = Mathf.Lerp(this.transform.position.y, this.followingTarget.y, this.cameraLerpSpeed * Time.deltaTime);
        }

        if (this.UseBounds)
        {
            // Clamp the camera within the bounds
            this.target.x = Mathf.Clamp(this.target.x, this.MINBounds.x, this.MAXBounds.x);
            this.target.y = Mathf.Clamp(this.target.y, this.MINBounds.y, this.MAXBounds.y);
        }

        // Set the camera's position to the target position with the same z component.
        this.transform.position = new Vector3(this.target.x, this.target.y, this.transform.position.z);
        if (this.CustomTarget)
        {
            if (Math.Abs(this.transform.position.x - this.followingTarget.x) < 0.25f
                && Math.Abs(this.transform.position.y - this.followingTarget.y) < 0.25f)
            {
                this.CustomTarget = false;
            }
        }
    }

    /// <summary>
    /// Checks if the distance between the camera and player (on X axis) is greater than X margin
    /// </summary>
    /// <returns><c>true</c>, is distance is greater, <c>false</c> otherwise.</returns>
    private bool CheckMovementXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(this.transform.position.x - this.followingTarget.x) > this.Margins.x;
    }

    /// <summary>
    /// Checks if the distance between the camera and player (on Y axis) is greater than Y margin
    /// </summary>
    /// <returns><c>true</c>, is distance is greater, <c>false</c> otherwise.</returns>
    private bool CheckMovementYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(this.transform.position.y - this.followingTarget.y) > this.Margins.y;
    }

    public void RunActionOnCustomPanFinished(Action callback)
    {
        this.StartCoroutine(this.RunActionOnCustomPanFinishedCoroutine(callback));
    }

    private IEnumerator RunActionOnCustomPanFinishedCoroutine(Action callback)
    {
        yield return new WaitWhile(() => this.CustomTarget == true);
        callback();
    }
}