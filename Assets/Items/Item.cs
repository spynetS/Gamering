using System;
using TMPro;
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
       
        public GameObject text;

        public Sprite image;
        
        private void Start()
        {
            /*
             * Adds items neeeded for items
             * Trigger collider
             * Text for press me
             */
            this.gameObject.tag = "item";
            BoxCollider asd = this.AddComponent<BoxCollider>();
            asd.size = new Vector3(1, 1, 1);
            asd.isTrigger = true;

            var t = Instantiate(text, transform);
            t.GetComponent<TMP_Text>().enabled = false;

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
            transform.GetComponentInChildren<TMP_Text>().enabled = false;

            foreach (Collider col in transform.GetComponents<Collider>()) {
                col.enabled = false;
            }
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
            foreach (Collider col in transform.GetComponents<Collider>()) {
                col.enabled = true;
            }
            transform.SetParent(null);
            transform.GetComponent<Rigidbody>().isKinematic = false;
            transform.gameObject.SetActive(true); 
            
        }
        public virtual void update()
        {
        }

    }
}