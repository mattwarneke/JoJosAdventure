using UnityEngine;

namespace JoJosAdventure
{
    [CreateAssetMenu(menuName = "ScriptableObj/EnemyData")]
    public class EnemyDataSO : ScriptableObject
    {
        [field: SerializeField]
        public int MaxHealth { get; set; } = 3;

        [field: SerializeField]
        public int Damage { get; set; } = 1;
    }
}