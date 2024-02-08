using UnityEngine;

namespace Source.Scripts.Players.Weapons
{
    [CreateAssetMenu(menuName = "Create WeaponScriptbleObject", fileName = "WeaponScriptbleObject", order = 0)]
    public class WeaponScriptableObject : ScriptableObject
    {
        [field: SerializeField] public int ClipCapacityProjectile { get; private set; }
        [field: SerializeField] public float ShootingDelay { get; private set; }
    }
}