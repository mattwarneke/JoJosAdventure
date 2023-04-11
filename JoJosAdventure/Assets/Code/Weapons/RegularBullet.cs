using JoJosAdventure.ScriptableObjects;
using JoJosAdventure.Utils;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var hittable = collision.GetComponent<IHittable>();
            hittable?.GetHit(this.BulletData.Damage, this.gameObject);
            // todo replace with impact input of layers
            if (collision.gameObject.layer == LayerMask.NameToLayer(LayersUtil.ObstacleLayer))
            {
                this.HitObstacle();
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer(LayersUtil.EnemyLayer))
            {
                this.HitEnemy();
            }
            Destroy(this.gameObject);
        }

        private void HitEnemy()
        {
            Debug.Log("Hitting Enemy");
        }

        private void HitObstacle()
        {
            Debug.Log("Hitting Obstacle");
        }
    }
}