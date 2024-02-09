using UnityEngine;

namespace Source.Scripts.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Weapon", fileName = "Weapon", order = 2)]
    public class WeaponScriptableObject : ScriptableObject
    {
        [field: SerializeField] public int ClipCapacityProjectile { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
    }
}