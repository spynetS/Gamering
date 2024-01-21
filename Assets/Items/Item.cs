using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Items
{
    public class Item: MonoBehaviour
    {
        public string id;
        private ItemRecipe myRecipe;
        public GameObject player { get; set; }

        private void Start()
        {
            this.gameObject.tag = "item";
            BoxCollider asd = this.AddComponent<BoxCollider>();
            asd.size = new Vector3(1, 1, 1);
            asd.isTrigger = true;
        }

        public void drop(float amount = 100)
        {
            setItemToBeDroped();
            if (player != null)
            {
                var forward = player.GetComponentInChildren<Camera>().transform.forward;
                this.transform.position = this.player.transform.position + (forward*2);
                this.GetComponent<Rigidbody>().AddForce((forward)*amount+Vector3.up*amount/4);

                var inventory = player.GetComponent<Inventory.Inventory>();
                inventory.stacks[inventory.selectedStack].items.RemoveAt(0);
            }
        }

        /**
         * This method sets the item to be hold by the player
         */
        public void setItemToBeHold()
        {
            transform.SetParent(player.GetComponent<Inventory.Inventory>().itemHolder.transform,true);
            transform.GetComponent<Rigidbody>().isKinematic = true;

            transform.GetComponent<BoxCollider>().enabled = false;
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.position = Vector3.zero;
            transform.localPosition = Vector3.zero;
            transform.gameObject.SetActive(true); 
        }

        /**
         * This method sets the item to be droped by the player
         */
        public void setItemToBeDroped()
        {
            transform.GetComponent<BoxCollider>().enabled = true;
            transform.SetParent(null);
            transform.GetComponent<Rigidbody>().isKinematic = false;
            transform.gameObject.SetActive(true); 
            
        }

        public virtual void update()
        {
            
        }
        
    }
}