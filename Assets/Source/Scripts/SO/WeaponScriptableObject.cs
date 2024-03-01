using UnityEngine;

namespace Source.Scripts.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Weapon", fileName = "Weapon", order = 5)]
    public class WeaponScriptableObject : ScriptableObject
    {
        // TODO: переписать все SO с float на int !!!
        [field: SerializeField] public int ClipCapacityProjectile { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
    }
}