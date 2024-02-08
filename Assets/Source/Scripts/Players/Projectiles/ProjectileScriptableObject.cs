using UnityEngine;

namespace Source.Scripts.Players.Projectiles
{
    [CreateAssetMenu(menuName = "Create ProjectileSO", fileName = "ProjectileSO", order = 0)]
    public class ProjectileScriptableObject : ScriptableObject
    {
        [field: SerializeField] public GameObject BulletPrefab { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
    }
}