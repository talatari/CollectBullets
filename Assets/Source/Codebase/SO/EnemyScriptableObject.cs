using UnityEngine;

namespace Source.Codebase.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Enemy", fileName = "Enemy", order = 0)]
    public class EnemyScriptableObject : ScriptableObject
    {
        [field: SerializeField] public int Speed { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int DistanceAttack { get; private set; }
        [field: SerializeField] public int AttackCooldown { get; private set; }
    }
}