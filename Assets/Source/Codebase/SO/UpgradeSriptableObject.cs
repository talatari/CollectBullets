using Source.Codebase.Upgrades;
using UnityEngine;

namespace Source.Codebase.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Upgrade", fileName = "Upgrade", order = 3)]
    public class UpgradeSriptableObject : ScriptableObject
    {
        [field: SerializeField] public StatType StatType { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public int CurrentValue { get; private set; }
        [field: SerializeField] public int IncrementValue { get; private set; }
        [field: SerializeField] public int CurrentLevel { get; private set; }
        [field: SerializeField] public int MaxLevel { get; private set; }
    }
}