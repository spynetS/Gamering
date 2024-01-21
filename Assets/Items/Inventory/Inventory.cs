using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Items.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private int slots = 5;
        [SerializeField] public List<Stack> stacks = new List<Stack>();
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] public GameObject itemHolder;
        [SerializeField] private int _selectedStack;

        [SerializeField]
        public int selectedStack
        {
            get => _selectedStack;
            private set
            {
                _selectedStack = value;
                updateItemHolder();
            }
        }

        public void updateItemHolder() {
            for (int i = 0; i < itemHolder.transform.childCount; i++) {
                itemHolder.transform.GetChild(i).gameObject.SetActive(false);
            }
            stacks[selectedStack].items[0].setItemToBeHold();
        }

        public Transform lookingAt;
         
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                selectedStack = (selectedStack + 1) % slots;
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                stacks[selectedStack].items[0].drop();
                stacks[selectedStack].items.RemoveAt(0);
                updateItemHolder();
            }
            try{
                
                ((UseableItem)stacks[selectedStack].items[0]).update();
                updateItemHolder();
            }catch(Exception e){}

            int count = 0;
            foreach(Stack stack in stacks)
            {
                if (count == selectedStack)
                {
                    try
                    {
                        inventoryPanel.transform.GetChild(count).GetComponent<TMP_Text>().text =
                        stack.items.Count + "---" + stack.items[0].id;
                    }catch(Exception e){
                        inventoryPanel.transform.GetChild(count).GetComponent<TMP_Text>().text = "0";
                    }

                    inventoryPanel.transform.GetChild(count).GetComponent<TMP_Text>().fontSize = 24;
                }
                else
                {
                    try
                    {
                        inventoryPanel.transform.GetChild(count).GetComponent<TMP_Text>().text =
                            stack.items.Count + " " + stack.items[0].id;
                    }
                    catch (Exception e)
                    {
                        inventoryPanel.transform.GetChild(count).GetComponent<TMP_Text>().text = "0";
                    }
                    inventoryPanel.transform.GetChild(count).GetComponent<TMP_Text>().fontSize = 12;
                }
                count++;
            }


            RaycastHit hit;
            Transform forward = transform.GetComponentInChildren<Camera>().transform;
            if (Physics.Raycast(forward.position, forward.forward, out hit, 4)) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    FetchItem(hit.transform);
                }

                if (hit.transform.tag == "item" && hit.transform.GetComponentInChildren<TMP_Text>() != null) {
                    if(lookingAt != hit.transform) {
                        if (lookingAt != null) {
                            lookingAt.GetComponentInChildren<TMP_Text>().enabled = false;
                        }
                        lookingAt = hit.transform;
                        lookingAt.GetComponentInChildren<TMP_Text>().enabled = true;
                    }
                }
                else {
                    lookingAt.GetComponentInChildren<TMP_Text>().enabled = false;
                    lookingAt = null;
                }

            }
            else {
                lookingAt.GetComponentInChildren<TMP_Text>().enabled = false;
                lookingAt = null;
            }
        }

        private void Start()
        {
            for (int i = 0; i < slots; i ++)
            {
                Stack stack = new Stack();
                stacks.Add(stack);
                GameObject g = Instantiate(textPrefab,inventoryPanel.transform);
                g.GetComponent<TMP_Text>().text = "0 ";
                g.GetComponent<TMP_Text>().fontSize = 12;

            }
        }

        public void AddItem(Item item)
        {
            foreach (Stack stack in stacks)
            {
                if (stack.items.Count == 0 || stack.items[0].id == item.id) 
                {
                    stack.AddItem(item);
                    return;
                }
            }
        }

        private void FetchItem(Transform other)
        {
            if (other.gameObject.CompareTag("item") && other.transform.parent != itemHolder.transform)
            {
                if (other.GetComponent<Item>() != null)
                {
                    other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    other.GetComponent<Item>().player = this.gameObject;
                    AddItem(other.GetComponent<Item>());
                }
                other.gameObject.SetActive(false);
                updateItemHolder();
            }
        }
    }
}