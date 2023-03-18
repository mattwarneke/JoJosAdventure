using UnityEngine;

public class Tile : MonoBehaviour
{
    /// <summary>
    /// The player transform.
    /// </summary>
    private Transform playerTransform;

    /// <summary>
    /// The sprite renderer.
    /// </summary>
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    private void Start()
    {
        // obtain the local references
        this.playerTransform = GameObject.FindGameObjectsWithTag("Jojo")[0].transform;
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        if (this.spriteRenderer == null)
            this.spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.spriteRenderer == null || this.tag != "LargeTile")
            return;
        // to ensure correct positioning of the environment around the player (3D Depth Effect)
        // we need to make the tiles below the player higher than the player in the render layering
        // and the ones above the player be lower than the player in the render layering

        if ((this.playerTransform.position.y - 1.28f) > this.transform.position.y)
        {
            // make all the tiles lower than the player higher than them on render layer
            this.spriteRenderer.sortingLayerName = "TreeLayer";
        }
        else
        {
            // give this tile a normal tile set render order
            this.spriteRenderer.sortingLayerName = "Tileset";
        }
    }
}