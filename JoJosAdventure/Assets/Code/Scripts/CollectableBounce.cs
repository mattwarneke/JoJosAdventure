using JoJosAdventure.Utils;
using UnityEngine;

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

    #endregion Member Variables

    private void Start()
    {
        this.StartingScale = this.transform.localScale.x;
    }

    // Update is called once per frame
    private void Update()
    {
        // change the scale factor the object based on the current state
        if (this.ScaleDirection == SCALEDIRECTION.UP)
        {
            this.ScaleXY += 0.5f * Time.deltaTime;
        }
        else if (this.ScaleDirection == SCALEDIRECTION.DOWN)
        {
            this.ScaleXY -= 0.5f * Time.deltaTime;
        }

        // limit the scale in both directions
        if (this.ScaleXY > 1.2f)
        {
            this.ScaleDirection = SCALEDIRECTION.DOWN;
            this.ScaleXY = 1.2f;
        }

        if (this.ScaleXY < 0.8f)
        {
            this.ScaleDirection = SCALEDIRECTION.UP;
            this.ScaleXY = 0.8f;
        }

        // apply the scale factor
        this.transform.localScale = new Vector3(this.StartingScale * this.ScaleXY, this.StartingScale * this.ScaleXY, this.transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (LayersUtil.IsColliderPlayer(collider))
        {
            //GameService.Instance.JoJoSwip();
            Destroy(this.gameObject, 0.5f);
        }
    }
}