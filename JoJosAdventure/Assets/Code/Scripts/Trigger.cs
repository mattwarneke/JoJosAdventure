using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Logic;

public class Trigger : MonoBehaviour 
{
    public EventEnum eventToTrigger;
    public bool CanBeRepeated;
    
	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.name == "PlayerCharacter")
		{
            GameService.Instance().HandleEvent(eventToTrigger);
            if (!CanBeRepeated)
                Destroy(this.gameObject);
		}
	}
}
