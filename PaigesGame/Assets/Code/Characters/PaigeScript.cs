using Assets.Code.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaigeScript : MonoBehaviour
{
    public Animator animator;
    public SpeechBubble speechBubble;

    public void Speak(List<Speech> speech)
    {
        animator.SetBool("Finished", true);
        speechBubble.enabled = true;
        speechBubble.AddToSpeechQueue(speech);
    }
}
