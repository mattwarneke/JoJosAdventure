using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{

	#region Member Variables
	/// <summary>
	/// The distance the player can move before the camera follows
	/// </summary>
	public Vector2 Margins;	

	/// <summary>
	/// The maximum x and y coordinates the camera can have
	/// </summary>
	public Vector2 MAXBounds;

	/// <summary>
	/// The minimum x and y coordinates the camera can have
	/// </summary>
	public Vector2 MINBounds;

	/// <summary>
	/// The player character
	/// </summary>
	public GameObject PlayerCharacter;

	/// <summary>
	///  Reference to the users current view transform.
	/// </summary>
	private Transform PlayerTransform;
	#endregion

	void Start()
	{
		// get the players transform
		PlayerTransform = PlayerCharacter.transform;
        transform.position = new Vector3(PlayerCharacter.transform.position.x, PlayerCharacter.transform.position.y, transform.position.z);
        followingTarget = PlayerTransform.position;
    }

    public void SetCustomPanTarget(Vector3 target)
    {
        this.CustomTarget = true;
        this.followingTarget = target;
    }

    float cameraLerpSpeed = 3f;
    Vector2 target;
    public bool CustomTarget = false;
    Vector3 followingTarget;
    void Update ()
	{
        if (CustomTarget)
        {
            cameraLerpSpeed = 2.5f;
        }
        else
        {
            followingTarget = PlayerCharacter.transform.position;
            cameraLerpSpeed = 4f;
        }

        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        target = new Vector2(transform.position.x, transform.position.y);
		
		// If the player has moved beyond the x margin
		if(CheckXMargin())
		{
			// the target X-coordinate should be a Lerp between the camera's current x position and the player's current x position.
			target.x = Mathf.Lerp(transform.position.x, followingTarget.x , cameraLerpSpeed * Time.deltaTime);
		}
		
		// If the player has moved beyond the y margin
		if(CheckYMargin())
		{
			// The target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			target.y = Mathf.Lerp(transform.position.y, followingTarget.y , cameraLerpSpeed * Time.deltaTime);
		}
        
        // Clamp the camera within the bounds
        target.x = Mathf.Clamp(target.x, MINBounds.x, MAXBounds.x);
		target.y = Mathf.Clamp(target.y, MINBounds.y, MAXBounds.y);

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(target.x, target.y, transform.position.z);
        if (CustomTarget)
        {
            if (Math.Abs(transform.position.x - followingTarget.x) < 0.25f
                && Math.Abs(transform.position.y - followingTarget.y) < 0.25f)
            {
                CustomTarget = false;
            }
        }
    }

	/// <summary>
	/// Checks if the distance between the camera and player (on X axis) is greater than X margin
	/// </summary>
	/// <returns><c>true</c>, is distance is greater, <c>false</c> otherwise.</returns>
	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - followingTarget.x) > Margins.x;
	}
	
	/// <summary>
	/// Checks if the distance between the camera and player (on Y axis) is greater than Y margin
	/// </summary>
	/// <returns><c>true</c>, is distance is greater, <c>false</c> otherwise.</returns>
	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - followingTarget.y) > Margins.y;
	}

    public void RunActionOnCustomPanFinished(Action callback)
    {
        StartCoroutine(RunActionOnCustomPanFinishedCoroutine(callback));
    }

    IEnumerator RunActionOnCustomPanFinishedCoroutine(Action callback)
    {
        yield return new WaitWhile(() => CustomTarget == true);
        callback();
    }
}
