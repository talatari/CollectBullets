using System;

namespace Source.Codebase.Common
{
    public class CooldownTimer
    {
        private float _cooldownInSeconds;
        private float _remainingDuration;

        public CooldownTimer(float cooldownInSeconds)
        {
            if (cooldownInSeconds <= 0) 
                throw new ArgumentOutOfRangeException(nameof(cooldownInSeconds));

            _cooldownInSeconds = cooldownInSeconds;
        }
        
        public bool CanShoot => _remainingDuration == 0;

        public void Run() => 
            _remainingDuration = _cooldownInSeconds;

        public void Tick(float deltaTime)
        {
            _remainingDuration -= deltaTime;
            
            if (_remainingDuration < 0)
                _remainingDuration = 0;
        }

        public void SetCooldown(float cooldownInSeconds)
        {
            if (cooldownInSeconds <= 0) 
                throw new ArgumentOutOfRangeException(nameof(cooldownInSeconds));
            
            _cooldownInSeconds = cooldownInSeconds;
        }
    }
}