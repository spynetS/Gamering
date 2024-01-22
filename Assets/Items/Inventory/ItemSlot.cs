using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Items.Inventory {
    public class ItemSlot : MonoBehaviour {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image image;
        [SerializeField] private Sprite emptySlot;
        public Stack stackPointer;
        private void Start() {
            text = GetComponentInChildren<TMP_Text>();
            image = transform.GetChild(0).GetComponent<Image>();
        }

        public void OnClick() {
            text.text = "asd";
        }

        private void Update() {
            if (stackPointer != null) {
                text.text = stackPointer.items.Count.ToString();
                if (stackPointer.items.Count > 0)
                    image.sprite = stackPointer.items[0].image;
            }
            else if (image.sprite != emptySlot)
                image.sprite = emptySlot;
        }

        public void isSelected(bool selected) {
            if (selected) transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            else transform.localScale = new Vector3(1,1,1);
        }
        
    }
}