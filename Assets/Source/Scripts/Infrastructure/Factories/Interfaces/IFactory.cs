using Source.Scripts.Infrastructure.Pools.Interfaces;

namespace Source.Scripts.Infrastructure.Factories.Interfaces
{
    public interface IFactory<T> where T : IPoolable
    {
        T Create();

        // TODO: перенести в спавнер
        // float _distanceRange;
        
        // protected Vector3 GetPositionX(Vector3 position) => 
        //     new(Random.Range(-1 * _distanceRange, _distanceRange), position.y, position.z);
        //
        // protected Vector3 GetPositionY(Vector3 position) => 
        //     new(position.x, Random.Range(0, _distanceRange), position.z);
        //
        // protected Vector3 GetPositionZ(Vector3 position) => 
        //     new(position.x, position.y, Random.Range(-1 * _distanceRange, _distanceRange));
    }
}