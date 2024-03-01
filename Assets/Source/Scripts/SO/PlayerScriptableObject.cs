using UnityEngine;

namespace Source.Scripts.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Player", fileName = "Player", order = 1)]
    public class PlayerScriptableObject : ScriptableObject
    {
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int Burning { get; private set; }
        [field: SerializeField] public int BurningDuration { get; private set; }
        [field: SerializeField] public int Vampirism { get; private set; }
        [field: SerializeField] public int ClipCapacity { get; private set; }
        [field: SerializeField] public int ShootingDelay { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Regeneration { get; private set; }
        [field: SerializeField] public int Magnet { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }
        [field: SerializeField] public int Freeze { get; private set; }
        [field: SerializeField] public int RadiusAttack { get; private set; }
    }
}