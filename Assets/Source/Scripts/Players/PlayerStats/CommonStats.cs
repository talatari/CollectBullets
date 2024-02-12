using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class CommonStats
    {
        private float _magnet;
        private float _speed;

        public CommonStats(float magnet, float speed)
        {
            if (magnet <= 0)
                throw new ArgumentOutOfRangeException(nameof(magnet));
            if (speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(speed));

            _magnet = magnet;
            _speed = speed;
        }
        
        public event Action<float> MagnetChanged;
        public event Action<float> SpeedChanged;

        public float Magnet => _magnet;
        public float Speed => _speed;

        public void ChangeMagnet(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _magnet = value;
            MagnetChanged?.Invoke(_magnet);
        }
        
        public void ChangeSpeed(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _speed = value;
            SpeedChanged?.Invoke(_speed);
        }
    }
}