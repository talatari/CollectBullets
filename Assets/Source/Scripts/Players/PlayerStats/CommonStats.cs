using System;

namespace Source.Scripts.Players.PlayerStats
{
    public class CommonStats
    {
        private float _magnet;

        public CommonStats(float magnet)
        {
            if (magnet <= 0) 
                throw new ArgumentOutOfRangeException(nameof(magnet));
            
            _magnet = magnet;
        }
        
        public float Magnet => _magnet;
    }
}