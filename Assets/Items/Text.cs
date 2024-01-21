using System;
using UnityEngine;

namespace Items
{
    public class Text : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);

        }
    }
}