using UnityEngine;

namespace Source.Scripts.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Ability", fileName = "Ability", order = 0)]
    public class AbilitySriptableObject : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public bool IsUpgradable { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int MaxLevel { get; private set; }
        [field: SerializeField] public int DefaultValue { get; private set; }
        [field: SerializeField] public int IncrementValue { get; private set; }
    }
}