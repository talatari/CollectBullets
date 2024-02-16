using System;

namespace Source.Scripts.Players.PlayerModels
{
    public class CommonStats
    {
        private float _magnet;
        private float _speed;
        private float _freeze;
        
        public CommonStats(float magnet, float speed, float freeze)
        {
            if (magnet <= 0)
                throw new ArgumentOutOfRangeException(nameof(magnet));
            if (speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(speed));
            if (freeze < 0) 
                throw new ArgumentOutOfRangeException(nameof(freeze));

            _magnet = magnet;
            _speed = speed;
            _freeze = freeze;
        }
        
        public event Action<float> MagnetChanged;
        public event Action<float> SpeedChanged;
        public event Action<float> FreezeChanged;

        public float Magnet => _magnet;
        public float Speed => _speed;
        public float Freeze => _freeze;

        public void AddMagnet(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _magnet += value;
            MagnetChanged?.Invoke(_magnet);
        }
        
        public void AddSpeed(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _speed += value;
            SpeedChanged?.Invoke(_speed);
        }
        
        public void AddFreeze(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _freeze += value;
            FreezeChanged?.Invoke(_freeze);
        }
    }
}