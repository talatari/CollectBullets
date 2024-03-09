using System;

namespace Source.Codebase.Players.PlayerModels
{
    [Serializable]
    public class CommonStats
    {
        private int _defaultMagnet;
        private int _magnet;
        private int _defaultSpeed;
        private int _speed;
        private int _defaultFreeze;
        private int _freeze;
        private int _defaultRadiusAttack;
        private int _radiusAttack;
        
        public CommonStats(int magnet, int speed, int freeze, int radiusAttack)
        {
            if (magnet <= 0)
                throw new ArgumentOutOfRangeException(nameof(magnet));
            if (speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(speed));
            if (freeze < 0) 
                throw new ArgumentOutOfRangeException(nameof(freeze));
            if (radiusAttack <= 0)
                throw new ArgumentOutOfRangeException(nameof(radiusAttack));

            _defaultMagnet = magnet;
            _defaultSpeed = speed;
            _defaultFreeze = freeze;
            _defaultRadiusAttack = radiusAttack;

            SetDefaultValues();
        }
        
        public event Action<int> MagnetChanged;
        public event Action<int> SpeedChanged;
        public event Action<int> FreezeChanged;
        public event Action<int> RadiusAttackChanged; 

        public int Magnet => _magnet;
        public int Speed => _speed;
        public int Freeze => _freeze;
        public int RadiusAttack => _radiusAttack;

        public void SetDefaultValues()
        {
            _magnet = _defaultMagnet;
            MagnetChanged?.Invoke(_magnet);
            
            _speed = _defaultSpeed;
            SpeedChanged?.Invoke(_speed);

            _freeze = _defaultFreeze;
            FreezeChanged?.Invoke(_freeze);
            
            _radiusAttack = _defaultRadiusAttack;
            RadiusAttackChanged?.Invoke(_radiusAttack);
        }
        
        public int AddMagnet(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _magnet += value;
            MagnetChanged?.Invoke(_magnet);

            return _magnet;
        }
        
        public int AddSpeed(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _speed += value;
            SpeedChanged?.Invoke(_speed);

            return _speed;
        }
        
        public int AddFreeze(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _freeze += value;
            FreezeChanged?.Invoke(_freeze);

            return _freeze;
        }
        
        public int AddRadiusAttack(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _radiusAttack += value;
            RadiusAttackChanged?.Invoke(_radiusAttack);
            
            return _radiusAttack;
        }
    }
}