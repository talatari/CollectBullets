using UnityEngine;

namespace Source.Codebase.Players.Bug
{
    public class Collected : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _key;
        [SerializeField] private MeshRenderer _bullet;
        
        public Types Type { get; private set; }
        
        public void Inactive() => 
            gameObject.SetActive(false);
        
        public void SetKey()
        {
            Type = Types.Key;

            _key.gameObject.SetActive(true);
            _bullet.gameObject.SetActive(false);
            
            gameObject.SetActive(true);
        }

        public void SetBullet()
        {
            Type = Types.Bullet;
            
            _key.gameObject.SetActive(false);
            _bullet.gameObject.SetActive(true);
            
            gameObject.SetActive(true);
        }
    }
}