using UnityEngine;

namespace Source.Scripts.Players.Projectiles.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Projectile", fileName = "Projectile", order = 1)]
    public class ProjectileScriptableObject : ScriptableObject
    {
        [field: SerializeField] public GameObject BulletPrefab { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }
    }
}