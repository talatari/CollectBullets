using UnityEngine;

namespace Source.Scripts.SO
{
    [CreateAssetMenu(menuName = "Create ScriptableObject/Wave", fileName = "Wave", order = 4)]
    public class WaveScriptableObject : ScriptableObject
    {
        [field: SerializeField] public int DefaultCount { get; private set; }
        [field: SerializeField] public int IncrementPercent { get; private set; }
        
        [field: SerializeField] public float DefaultDelay { get; private set; }
        [field: SerializeField] public float DecrementDelay { get; private set; }
        [field: SerializeField] public int DelayBetweenWaves { get; private set; }
    }
}