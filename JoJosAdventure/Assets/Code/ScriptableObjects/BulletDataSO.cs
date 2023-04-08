using UnityEngine;

namespace JoJosAdventure.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObj/BulletData")]
    public class BulletDataSO : ScriptableObject
    {
        [field: SerializeField]
        public GameObject BulletPrefab { get; set; }

        [field: SerializeField]
        [field: Range(1, 100)]
        public float BulletSpeed { get; internal set; } = 1;

        [field: SerializeField]
        [field: Range(1, 10)]
        public int Damage { get; set; } = 1;

        [field: SerializeField]
        [field: Range(0, 100)]
        public float Friction { get; internal set; } = 0;

        [field: SerializeField]
        public bool IsBounce { get; set; } = false;

        [field: SerializeField]
        public bool IsPierceEnemies { get; set; } = false;

        [field: SerializeField]
        public bool IsRayCast { get; set; } = false;

        [field: SerializeField]
        public GameObject ImpactObstaclePrefab { get; set; }

        [field: SerializeField]
        public GameObject ImpactEnemyPrefab { get; set; }

        [field: SerializeField]
        [field: Range(1, 20)]
        public float KnockbackPower { get; set; } = 5;

        [field: SerializeField]
        [field: Range(0.01f, 1f)]
        public float KnockbackDelay { get; set; } = 0.1f;
    }
}