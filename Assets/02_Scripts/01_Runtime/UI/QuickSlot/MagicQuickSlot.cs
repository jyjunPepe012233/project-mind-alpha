using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MinD.SO.Item;
using TMPro;

namespace MinD.Runtime.UI
{
    public class MagicQuickSlot : MonoBehaviour
    {
        public TextMeshProUGUI itemName;
        public Image[] slotImages;

        private List<Magic> magicList = new();
        private int currentIndex = 0;

        public void Initialize(List<Magic> magicSlots, int playerCurrentMagicSlot)
        {
            if (magicSlots == null)
            {
                Debug.LogError("Magic slots are null during initialization.");
                magicList = new List<Magic>();
            }
            else
            {
                magicList = new List<Magic>(magicSlots);
                magicList.RemoveAll(magic => magic == null);
            }

            currentIndex = playerCurrentMagicSlot;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (magicList.Count == 0)
            {
                foreach (var image in slotImages)
                {
                    image.enabled = false;
                }
                itemName.text = string.Empty;
                return;
            }

            itemName.text = magicList[currentIndex].itemName;

            for (int i = 0; i < slotImages.Length; i++)
            {
                int offsetIndex = (currentIndex + i - (slotImages.Length / 2) + magicList.Count) % magicList.Count;

                slotImages[i].sprite = magicList[offsetIndex].itemImage;
                slotImages[i].enabled = true;
            }
        }


        public void SetCurrentMagicSlot(int newIndex)
        {
            if (magicList.Count == 0) return;

            currentIndex = newIndex % magicList.Count;
            UpdateUI();
        }

        public Magic GetCurrentMagic()
        {
            if (magicList.Count == 0) return null;
            return magicList[currentIndex];
        }
    }
}
