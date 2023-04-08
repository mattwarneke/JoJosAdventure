using JoJosAdventure.ScriptableObjects;
using UnityEngine;

namespace JoJosAdventure.Weapons
{
    public class RegularBullet : Bullet
    {
        protected Rigidbody2D rigidbody2d;

        public override BulletDataSO BulletData
        {
            get => base.BulletData;
            set
            {
                base.BulletData = value;
                this.rigidbody2d = this.GetComponent<Rigidbody2D>();
                this.rigidbody2d.drag = this.BulletData.Friction;
            }
        }

        private void FixedUpdate()
        {
            if (this.rigidbody2d != null && this.BulletData != null)
            {
                // fly in direction facing
                this.rigidbody2d.MovePosition(this.transform.position + (this.BulletData.BulletSpeed * this.transform.right * Time.fixedDeltaTime));
            }
        }
    }
}