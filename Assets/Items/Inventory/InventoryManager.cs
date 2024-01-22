using System;
using UnityEngine;

namespace Items.Inventory {
    public class InventoryManager : MonoBehaviour {
        [SerializeField] private ItemSlot[] itemSlots;
        
        [SerializeField] private Inventory inventory;

        private void Start() {
            itemSlots = new ItemSlot[7]; 
            for (int i = 0; i < transform.childCount; i++) {
                itemSlots[i] = transform.GetChild(i).GetComponent<ItemSlot>();
            }
        }

        private void Update() {
            for (int i = 0; i < itemSlots.Length; i++) {
                if (i == inventory.selectedStack) {
                    itemSlots[i].isSelected(true);
                }
                else {
                    itemSlots[i].isSelected(false);
                }
                
                itemSlots[i].stackPointer = inventory.stacks[i]; 
            }
        }
    }
}