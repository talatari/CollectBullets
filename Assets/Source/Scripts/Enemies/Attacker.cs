using System;
using UnityEngine;

namespace Source.Scripts.Enemies
{
    public class Attacker : MonoBehaviour
    {
        private int _damage;
        
        public void TakeDamage(int damage)
        {
            
        }

        public void SetDamage(int damage)
        {
            if (damage <= 0) 
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _damage = damage;
        }
    }
}