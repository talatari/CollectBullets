using UnityEngine;

namespace Source.Scripts.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Player", fileName = "Player", order = 2)]
    public class PlayerScriptableObject : ScriptableObject
    {
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float Burning { get; private set; }
        [field: SerializeField] public float Vampirism { get; private set; }
        [field: SerializeField] public int ClipCapacity { get; private set; }
        [field: SerializeField] public float ShootingDelay { get; private set; }

        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float Regeneration { get; private set; }

        [field: SerializeField] public float Magnet { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Freeze { get; private set; }
    }
}