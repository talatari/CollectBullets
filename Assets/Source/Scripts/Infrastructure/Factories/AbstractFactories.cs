using UnityEngine;

namespace Source.Scripts.Infrastructure.Factories
{
    public abstract class AbstractFactory
    {
        protected Transform _parent;
        protected float _distanceRange;

        protected T CreateObject<T>(T prefab) where T : MonoBehaviour
        {
            T result = Object.Instantiate(prefab, _parent);
            
            return result;
        }
        
        protected Vector3 GetPositionX(Vector3 position) => 
            new(Random.Range(-1 * _distanceRange, _distanceRange), position.y, position.z);

        protected Vector3 GetPositionY(Vector3 position) => 
            new(position.x, Random.Range(0, _distanceRange), position.z);
        
        protected Vector3 GetPositionZ(Vector3 position) => 
            new(position.x, position.y, Random.Range(-1 * _distanceRange, _distanceRange));
    }
}