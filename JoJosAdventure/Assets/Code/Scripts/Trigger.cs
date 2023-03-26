using JoJosAdventure.Logic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public EventEnum eventToTrigger;
    public bool CanBeRepeated;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "PlayerCharacter")
        {
            GameService.Instance.HandleEvent(this.eventToTrigger);
            if (!this.CanBeRepeated)
                Destroy(this.gameObject);
        }
    }
}