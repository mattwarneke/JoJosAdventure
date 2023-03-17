using UnityEngine;

public class Door : MonoBehaviour
{
    #region Member Variables

    /// <summary>
    /// The sprite for the door
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// The sprite used for the on toggle setting
    /// </summary>
    public Sprite OpenSprite;

    /// <summary>
    /// The sprite used for the off toggle setting
    /// </summary>
    public Sprite ClosedSprite;

    /// <summary>
    /// Do we alter this objects collision or not?
    /// </summary>
    public bool CollisionToggle;

    /// <summary>
    /// A toggle for turning this tiles functionality on or off
    /// </summary>
    public enum TOGGLE
    {
        OPEN = 0,
        CLOSED = 1,
    }

    public TOGGLE Toggle;

    #endregion Member Variables

    private void Start()
    {
        // use the initial inspector setting to determine the starting phase of this object
        this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        if (this.Toggle == TOGGLE.CLOSED)
        {
            this.spriteRenderer.sprite = this.ClosedSprite;
            if (this.CollisionToggle) { this.gameObject.GetComponent<Collider2D>().enabled = true; }
        }
        else if (this.Toggle == TOGGLE.OPEN)
        {
            this.spriteRenderer.sprite = this.OpenSprite;
            if (this.CollisionToggle) { this.gameObject.GetComponent<Collider2D>().enabled = false; }
        }
    }

    /// <summary>
    /// Used to toggle between object states
    /// </summary>
    public void ToggleObject()
    {
        if (this.Toggle == TOGGLE.OPEN)
        {
            // make it closed
            this.Toggle = TOGGLE.CLOSED;

            // enable this objects collider and change the sprite to a closed door
            this.spriteRenderer.sprite = this.ClosedSprite;
            if (this.CollisionToggle) { this.gameObject.GetComponent<Collider2D>().enabled = true; }
        }
        else if (this.Toggle == TOGGLE.CLOSED)
        {
            // make it open
            this.Toggle = TOGGLE.OPEN;

            // remove this objects collider and change the sprite to an open door
            this.spriteRenderer.sprite = this.OpenSprite;
            if (this.CollisionToggle) { this.gameObject.GetComponent<Collider2D>().enabled = false; }
        }
    }
}