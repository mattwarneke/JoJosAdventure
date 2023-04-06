using UnityEngine;

namespace JoJosAdventure.Weapons
{
    public class AgentWeapon : MonoBehaviour
    {
        protected float desiredAngle;

        [SerializeField]
        protected WeaponRenderer weaponRenderer;

        private void Awake()
        {
            this.weaponRenderer = this.GetComponentInChildren<WeaponRenderer>();
        }

        public virtual void AimWeapon(Vector2 pointerPosition)
        {
            var aimDirection = (Vector3)pointerPosition - this.transform.position;
            // calculate the tangent Aten, Rad 2 degrees gives degrees
            this.desiredAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            this.AdjustWeaponRendering();
            // creates a rotation of angle degrees around an axis
            this.transform.rotation = Quaternion.AngleAxis(this.desiredAngle, Vector3.forward);
        }

        private void AdjustWeaponRendering()
        {
            this.weaponRenderer?.FlipSprite(this.desiredAngle > 90 || this.desiredAngle < -90);
            this.weaponRenderer?.RenderBehindHead(this.desiredAngle < 180 && this.desiredAngle > 0);
        }
    }
}