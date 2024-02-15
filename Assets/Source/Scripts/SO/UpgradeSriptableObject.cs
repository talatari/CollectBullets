using UnityEngine;

namespace Source.Scripts.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Upgrade", fileName = "Upgrade", order = 0)]
    public class UpgradeSriptableObject : ScriptableObject
    {
        // TODO: QUESTION: если я все параметры продублирую для каждого параметра в PlayerProgress, то как быть с типом Sprite?
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public bool IsUpgradable { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int MaxLevel { get; private set; }
        [field: SerializeField] public int DefaultValue { get; private set; }
        [field: SerializeField] public int IncrementValue { get; private set; }
    }
}