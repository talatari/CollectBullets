using System;
using Source.Codebase.SO;

namespace Source.Codebase.Players.PlayerModels
{
    [Serializable]
    public class CommonStats
    {
        private readonly PlayerScriptableObject _playerConfig;
        private int _magnet;
        private int _speed;
        private int _freeze;
        private int _radiusAttack;

        public CommonStats(PlayerScriptableObject playerConfig)
        {
            _playerConfig = playerConfig ? playerConfig : throw new ArgumentNullException(nameof(playerConfig));
            if (_playerConfig.Magnet <= 0)
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.Magnet));
            if (_playerConfig.Speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.Speed));
            if (_playerConfig.Freeze < 0) 
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.Freeze));
            if (_playerConfig.RadiusAttack <= 0)
                throw new ArgumentOutOfRangeException(nameof(_playerConfig.RadiusAttack));

            _magnet = _playerConfig.Magnet;
            _speed = _playerConfig.Speed;
            _freeze = _playerConfig.Freeze;
            _radiusAttack = _playerConfig.RadiusAttack;
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
            _magnet = _playerConfig.Magnet;
            MagnetChanged?.Invoke(_magnet);
            
            _speed = _playerConfig.Speed;
            SpeedChanged?.Invoke(_speed);

            _freeze = _playerConfig.Freeze;
            FreezeChanged?.Invoke(_freeze);
            
            _radiusAttack = _playerConfig.RadiusAttack;
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