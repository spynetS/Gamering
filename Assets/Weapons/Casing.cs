using System;
using UnityEngine;

namespace Weapons
{
    public class Casing : MonoBehaviour
    {
        private int ticker = 1000;
        private void Update()
        {
            if (ticker < 0)
            {
                Destroy(gameObject);
            }

            ticker--;
        }
    }
}