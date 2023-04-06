using UnityEngine;

namespace JoJosAdventure.Weapons
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponRenderer : MonoBehaviour
    {
        [SerializeField]
        protected int playerSortingOrder = 0;

        protected SpriteRenderer weaponRenderer;

        private void Awake()
        {
            this.weaponRenderer = this.GetComponent<SpriteRenderer>();
        }

        private bool _val = false;

        public void FlipSprite(bool val)
        {
            //this.weaponRenderer.flipY = val;
            if (this._val != val)
            {
                this.transform.localScale = new Vector2(this.transform.localScale.x * -1, this.transform.localScale.y * -1);
                this._val = val;
            }
        }

        public void RenderBehindHead(bool val)
        {
            if (val) // set the weapon behind the player
                this.weaponRenderer.sortingOrder = this.playerSortingOrder - 1;
            else
                this.weaponRenderer.sortingOrder = this.playerSortingOrder + 1;
        }
    }
}