using UnityEngine;

namespace Source.Codebase.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Weapon", fileName = "Weapon", order = 5)]
    public class WeaponScriptableObject : ScriptableObject
    {
        [field: SerializeField] public int ClipCapacityProjectile { get; private set; }
        [field: SerializeField] public int AttackCooldown { get; private set; }
    }
}