using JoJosAdventure.ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JoJosAdventure
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        protected GameObject ShotPoint;

        [SerializeField]
        protected WeaponDataSO weaponData;

        [SerializeField]
        protected int ammo = 10;

        public int Ammo
        {
            get { return this.ammo; }
            set
            {
                this.ammo = Mathf.Clamp(value, 0, this.weaponData.AmmoCapacity);
                this.ammo = value;
            }
        }

        public bool AmmoFull { get => this.Ammo >= this.weaponData.AmmoCapacity; }

        protected bool IsShooting = false;

        [SerializeField]
        protected bool reloadCoroutine = false;

        [field: SerializeField]
        public UnityEvent OnShoot { get; set; }

        [field: SerializeField]
        public UnityEvent OnShootNoAmmo { get; set; }

        public void Start()
        {
            this.Ammo = this.weaponData.AmmoCapacity;
        }

        public void Update()
        {
            this.UseWeapon();
        }

        private void UseWeapon()
        {
            if (this.IsShooting && this.reloadCoroutine == false)
            {
                if (this.Ammo > 0)
                {
                    this.Ammo--;
                    this.OnShoot?.Invoke();
                    for (int i = 0; i < this.weaponData.GetBulletCountToSpawn(); i++)
                    {
                        this.ShootBullet();
                    }
                }
                else
                {
                    this.IsShooting = false;
                    this.OnShootNoAmmo.Invoke();
                    return;
                }
                this.FinishShooting();
            }
        }

        private void FinishShooting()
        {
            this.StartCoroutine(this.DelayNextShotCoroutine());
            if (this.weaponData.AutomaticFire == false)
            {
                this.IsShooting = false;
            }
        }

        protected IEnumerator DelayNextShotCoroutine()
        {
            this.reloadCoroutine = true;
            yield return new WaitForSeconds(this.weaponData.WeaponDelay);
            this.reloadCoroutine = false;
        }

        private void ShootBullet()
        {
            Debug.Log("shooting bullet");
        }

        public void TryShooting()
        {
            this.IsShooting = true;
        }

        public void StopShooting()
        {
            this.IsShooting = false;
        }

        public void Reload(int ammo)
        {
            this.Ammo += ammo;
        }
    }
}