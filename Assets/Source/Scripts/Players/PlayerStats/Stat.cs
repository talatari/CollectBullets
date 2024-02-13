using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class Stat<T>
    {
        public Stat(T value) => 
            Value = value;

        public event Action<T> Changed;

        public T Value { get; private set; }
        
        public void Change(T value)
        {
            Value = value;
            Changed?.Invoke(Value);
        }
    }
}