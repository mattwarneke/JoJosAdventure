using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    #region Member Variables

    /// <summary>
    /// The sprite representing the trap
    /// </summary>
    private SpriteRenderer sprite;

    /// <summary>
    /// The sprite used for the on toggle setting
    /// </summary>
    public Sprite OnSprite;

    /// <summary>
    /// The sprite used for the off toggle setting
    /// </summary>
    public Sprite OffSprite;

    /// <summary>
    /// The time between animation changes
    /// </summary>
    private float Timer = 0.0f;

    /// <summary>
    /// A toggle for turning this tiles functionality on or off
    /// </summary>
    //
    public enum TOGGLE
    {
        ON = 0,
        OFF = 1,
    }

    public TOGGLE Toggle;

    /// <summary>
    /// The time the trap takes to activate
    /// </summary>
    public float TrapTime = 2.0f;

    #endregion Member Variables

    private void Start()
    {
        // use the initial inspector setting to determine the starting phase of this object
        this.sprite = this.gameObject.GetComponent<SpriteRenderer>();

        if (this.Toggle == TOGGLE.OFF)
        {
            this.sprite.sprite = this.OffSprite;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else if (this.Toggle == TOGGLE.ON)
        {
            this.sprite.sprite = this.OnSprite;
            this.gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }

    private void Update()
    {
        // Update the timer with the elapsed time
        this.Timer += Time.deltaTime;

        // Check if the timer has finished
        if (this.Timer > this.TrapTime)
        {
            this.Timer = 0.0f;
            this.ToggleObject();
        }
    }

    /// <summary>
    /// Used to toggle between object states
    /// </summary>
    public void ToggleObject()
    {
        if (this.Toggle == TOGGLE.OFF)
        {
            // turn it on
            this.Toggle = TOGGLE.ON;

            // change the sprite to an on trigger
            this.sprite.sprite = this.OnSprite;
            this.gameObject.GetComponent<Collider2D>().enabled = true;
        }
        else if (this.Toggle == TOGGLE.ON)
        {
            // turn it off
            this.Toggle = TOGGLE.OFF;

            // change the sprite to an off trigger
            this.sprite.sprite = this.OffSprite;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}