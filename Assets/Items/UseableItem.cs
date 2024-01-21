using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Items
{
    public class UseableItem : Item
    {
        public virtual void use()
        {
            
        }

        public override void update()
        {
            base.update();
            if (Input.GetMouseButtonDown(0)) use();
        }
    }
}