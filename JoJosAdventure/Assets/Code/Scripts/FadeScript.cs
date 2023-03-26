using UnityEngine;

public class FadeScript : MonoBehaviour
{
    #region Member Variables

    /// <summary>
    /// The sprite that represents the fade on screen
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// The alpha value of the fade
    /// </summary>
    private float AlphaValue;

    /// <summary>
    /// A toggle for turning this tiles functionality on or off
    /// </summary>
    public enum FADETYPE
    {
        IN = 0,
        OUT = 1,
        NONE = 2,
        RESPAWN = 3,
    }

    public FADETYPE FadeType;

    #endregion Member Variables

    // Use this for initialization
    private void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.FadeType = FADETYPE.IN;
        this.AlphaValue = 1.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        // fade in or fade out based on the objects state
        if (this.FadeType == FADETYPE.IN)
        {
            this.AlphaValue -= 0.25f * Time.deltaTime;
            // limit the possible alpha value
            if (this.AlphaValue < 0.0f)
            {
                this.AlphaValue = 0.0f;
                this.FadeType = FADETYPE.NONE;
            }
        }
        else if (this.FadeType == FADETYPE.OUT)
        {
            this.AlphaValue += Time.deltaTime;

            // limit the possible alpha value
            if (this.AlphaValue > 1.0f)
            {
                this.AlphaValue = 1.0f;
                this.FadeType = FADETYPE.NONE;
                this.ChangeLevel();
            }
        }
        else if (this.FadeType == FADETYPE.RESPAWN)
        {
            this.AlphaValue += 2.0f * Time.deltaTime;
            // limit the possible alpha value
            if (this.AlphaValue > 1.0f)
            {
                this.AlphaValue = 1.0f;
                this.FadeType = FADETYPE.IN;
                //GameObject.Find("PlayerCharacter").GetComponent<PlayerJojo>().RespawnPlayerAtCheckpoint;
            }
        }

        // set the objects new colour
        this.spriteRenderer.color = new Color(this.spriteRenderer.color.r, this.spriteRenderer.color.g, this.spriteRenderer.color.b, this.AlphaValue);
    }

    /// <summary>
    /// Set the fade out state
    /// </summary>
    public void FadeOut()
    {
        this.FadeType = FADETYPE.OUT;
    }

    /// <summary>
    /// Respawns the fade
    /// </summary>
    public void RespawnFade()
    {
        // set the respawn state
        this.FadeType = FADETYPE.RESPAWN;
    }

    /// <summary>
    /// Changes the level to the next level in the list
    /// </summary>
    private void ChangeLevel()
    {
        int levelID = Application.loadedLevel + 1;
        if (levelID > Application.levelCount - 1) { levelID = 0; }
        Application.LoadLevel(levelID);
    }
}