using UnityEngine;

namespace Source.Scripts.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Player", fileName = "Player", order = 2)]
    public class PlayerScriptableObject : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
    }
}