using UnityEngine;

namespace JoJosAdventure.Weapons
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponRenderer : MonoBehaviour
    {
        private bool _val = false;

        public void FlipSprite(bool val)
        {
            int flipModifier = val ? -1 : 1;
            if (this._val != val)
            {
                this.transform.localScale = new Vector3(
                    this.transform.localScale.x * flipModifier,
                    Mathf.Abs(this.transform.localScale.y) * flipModifier,
                    this.transform.localScale.z);
                this._val = val;
            }
        }
    }
}