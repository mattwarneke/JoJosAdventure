using UnityEngine;

namespace JoJosAdventure.JojoPlayer
{
    public class JojoRenderer : MonoBehaviour
    {
        protected SpriteRenderer spriteRenderer;

        private void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        private bool isFacingRight = true;

        // called from UnityEvent
        public void FaceDirection(Vector2 pointerInput)
        {
            var direction = (Vector3)pointerInput - this.transform.position;
            // if angle is positive we are facing right, negative left
            var result = Vector3.Cross(Vector2.up, direction);
            Debug.Log(result);

            bool needsToFlip =
                (this.isFacingRight && result.z > 0)
                || (!this.isFacingRight && result.z < 0);
            if (needsToFlip)
            {
                this.isFacingRight = !this.isFacingRight;

                // reverse the scale. This prevents z index changing in the case we rotated it 180 degrees
                Vector3 currentScale = this.gameObject.transform.localScale;
                currentScale.x *= -1;
                this.gameObject.transform.localScale = currentScale;
            }
            //this.transform.parent.transform.Rotate(0, 180, 0);
        }
    }
}