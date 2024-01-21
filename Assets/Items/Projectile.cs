using System;
using UnityEngine;

namespace Items
{
    public class Projectile : MonoBehaviour
    {
        public float speed;
        private float damage;

        public void onHit()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "Respawn")
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
                onHit();
            }
        }
    }
}