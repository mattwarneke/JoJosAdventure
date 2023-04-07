using UnityEngine;

namespace JoJosAdventure.Weapons
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponRenderer : MonoBehaviour
    {
        private bool isFlipped = false;

        public void FlipSprite(bool val)
        {
            if (this.isFlipped != val)
            {
                this.transform.localScale = new Vector3(
                    this.transform.localScale.x * -1,
                    this.transform.localScale.y * -1,
                    this.transform.localScale.z);
                this.isFlipped = val;
            }
        }
    }
}