using UnityEngine;

namespace Source.Scripts.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Projectile", fileName = "Projectile", order = 3)]
    public class ProjectileScriptableObject : ScriptableObject
    {
        [field: SerializeField] public GameObject BulletPrefab { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }
        [field: SerializeField] public int LifeTime { get; private set; }
    }
}