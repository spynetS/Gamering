

using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemRecipe : MonoBehaviour
    {
        private Item output;
        private Dictionary<int, Item> itemRecipe = new Dictionary<int, Item>();
    }
}