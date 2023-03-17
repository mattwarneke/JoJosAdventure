using UnityEngine;
using System.Collections;
using Assets.Code.Logic;

public class CollectableBounce : MonoBehaviour 
{
	#region Member Variables
	/// <summary>
	/// Scaling states to make the object bounce/pulse
	/// </summary>
	private enum SCALEDIRECTION
	{
		UP = 0,
		DOWN = 1,
	}
	private SCALEDIRECTION ScaleDirection;

	/// <summary>
	/// One scale value to retain aspect ratio	
	/// </summary>
	private float ScaleXY = 1.0f;

	/// <summary>
	/// The objects initial scale value 
	/// </summary>
	private float StartingScale = 0.0f;
	#endregion

	void Start ()
	{
		StartingScale = transform.localScale.x;
        GameService.Instance().AddCollectable();
    }

	// Update is called once per frame
	void Update ()
	{
		// change the scale factor the object based on the current state
		if(ScaleDirection == SCALEDIRECTION.UP)
		{
			ScaleXY += 0.5f * Time.deltaTime;
		}
		else if(ScaleDirection == SCALEDIRECTION.DOWN)
		{
			ScaleXY -= 0.5f * Time.deltaTime;
		}

		// limit the scale in both directions
		if(ScaleXY > 1.2f)
		{
			ScaleDirection = SCALEDIRECTION.DOWN;
			ScaleXY = 1.2f;
		}
		
		if(ScaleXY < 0.8f)
		{
			ScaleDirection = SCALEDIRECTION.UP;
			ScaleXY = 0.8f;
		}

		// apply the scale factor
		transform.localScale = new Vector3(StartingScale * ScaleXY, StartingScale * ScaleXY, transform.localScale.z);
	}
    
    public EventEnum eventRaisedOnDestroy;
	void OnTriggerEnter2D(Collider2D collider)
	{
        if (collider.gameObject.name == "PlayerCharacter")
		{
            GameService.Instance().JoJoSwip();
            Destroy(gameObject, 0.5f);
        }
	}

    void OnDestroy()
    {
        GameService.Instance().HandleEvent(eventRaisedOnDestroy);
    }
}
