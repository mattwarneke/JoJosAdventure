using JoJosAdventure.Logic;
using System.Collections.Generic;
using UnityEngine;

public class PaigeScript : MonoBehaviour
{
    public Animator animator;
    public SpeechBubble speechBubble;

    public void Speak(List<Speech> speech)
    {
        this.animator.SetBool("Finished", true);
        this.speechBubble.enabled = true;
        this.speechBubble.AddToSpeechQueue(speech);
    }
}