using UnityEngine;

namespace JoJosAdventure.Weapons
{
    public class AgentWeapon : MonoBehaviour
    {
        protected float desiredAngle;

        [SerializeField]
        protected WeaponRenderer weaponRenderer;

        [SerializeField]
        protected Weapon weapon;

        private void Awake()
        {
            this.assignWeapon();
        }

        private void assignWeapon()
        {
            this.weaponRenderer = this.GetComponentInChildren<WeaponRenderer>();
            this.weapon = this.GetComponentInChildren<Weapon>();
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

        protected void AdjustWeaponRendering()
        {
            if (this.weaponRenderer != null)
            {
                this.weaponRenderer.FlipSprite(this.desiredAngle > 90 || this.desiredAngle < -90);
            }
        }

        public void Shoot()
        {
            if (this.weapon != null)
            {
                this.weapon.TryShooting();
            }
        }

        public void StopShooting()
        {
            if (this.weapon != null)
            {
                this.weapon.StopShooting();
            }
        }
    }
}