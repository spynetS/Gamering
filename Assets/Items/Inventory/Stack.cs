using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Inventory
{
    public class Stack
    {
        private int max = 10;
        public List<Item> items = new List<Item>();
        public string currentType { get; private set; }

        public void AddItem(Item item)
        {
            if (this.currentType == null)
                this.currentType = item.id;
            
            items.Add(item);
        }

    }
}