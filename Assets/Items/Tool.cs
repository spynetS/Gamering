using System;
using UnityEngine;

namespace Items
{
    public class Tool : UseableItem
    {
        public override void use()
        {
            if (this.GetComponentInParent<Animator>() != null)
            {
                this.GetComponentInParent<Animator>().SetTrigger("Hiter");
            }
        }

        public override void update()
        {
            if (Input.GetMouseButton(0))
            {
                use();
            }
        }
    }
}