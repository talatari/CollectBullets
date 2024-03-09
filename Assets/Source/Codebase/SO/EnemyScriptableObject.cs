using UnityEngine;

namespace Source.Codebase.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Enemy", fileName = "Enemy", order = 0)]
    public class EnemyScriptableObject : ScriptableObject
    {
        // TODO: переписать все SO с float на int !!!
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float DistanceAttack { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
    }
}