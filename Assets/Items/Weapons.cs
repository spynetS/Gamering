using System;
using System.Collections.Generic;
using Items.Inventory;
using UnityEngine;

namespace Items
{
    public class Weapons : Tool
    {
        [SerializeField] private float shootSpeed;
        [SerializeField] private int reloadTime;
        [SerializeField] private GameObject projectile;
        [SerializeField] private int magazineSize;
        [SerializeField] private List<string> projectileIdList = new List<string>();
        [SerializeField] private List<GameObject> loaded = new List<GameObject>();
        [SerializeField] private GameObject caseThrower;
        private float shootTicker = 0;
        private float reloadTicker = 0;
        public void reload()
        {
            reloadTicker = 0;
            foreach (Stack stack in player.GetComponent<Inventory.Inventory>().stacks)
            {
                List<Item> toRemove = new List<Item>();
                foreach (Item item in stack.items) {
                    if (projectileIdList.Contains(item.id) && loaded.Count < magazineSize) {
                        loaded.Add(item.gameObject);
                        toRemove.Add(item);
                    }
                }
                foreach (Item item in toRemove) stack.items.Remove(item);
            }
        }
        public override void use()
        {
             
            if (shootTicker < 1/shootSpeed) {
                shootTicker += Time.deltaTime;
                return;
            }
            
            shootTicker = 0;
            
            var forward = player.GetComponentInChildren<Camera>().transform;
            if (loaded.Count > 0) {
                loaded[0].gameObject.SetActive(true);
                
                if (loaded[0].GetComponent<Bullet>() != null)
                {
                    throwCasing(loaded[0].GetComponent<Bullet>().casing);
                }
                
                loaded[0].transform.SetPositionAndRotation(forward.position+forward.forward, player.GetComponentInChildren<Camera>().transform.rotation);
                loaded[0].GetComponent<Rigidbody>().AddForce(forward.forward*loaded[0].GetComponent<Projectile>().speed);

                
                loaded.RemoveAt(0);
            }
            
        }

        public override void update()
        {
            base.update();
            if (Input.GetKeyDown(KeyCode.R))
            {
                reload();
            }
        }


        private void throwCasing(GameObject casing)
        {
            var thrownCasing = Instantiate(casing,caseThrower.transform.position, Quaternion.identity);
            thrownCasing.GetComponent<Rigidbody>().AddForce(transform.right*100);
            
        }
    }
}