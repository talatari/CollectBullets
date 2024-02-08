namespace Source.Scripts.Players.Weapons
{
    public class Pistol
    {
        private int _clipCapacityBullets = 5;
        private int _collectedBullets = 0;
        private float _shootingDelay = 1f;
        
        public int CollectedBullets => _collectedBullets;
        public int ClipCapacityBullets => _clipCapacityBullets;
        public float ShootingDelay => _shootingDelay;
    }
}