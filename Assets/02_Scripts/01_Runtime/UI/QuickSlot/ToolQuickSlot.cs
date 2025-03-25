using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MinD.SO.Item;
using TMPro;
using UnityEngine.Serialization;

namespace MinD.Runtime.UI
{
    public class ToolQuickSlot : MonoBehaviour
    {
        public TextMeshProUGUI toolName;
        public Image[] slotImages;
        private List<Tool> toolList = new();
        private int currentIndex = 0;

        public void Initialize(List<Tool> toolSlots, int playerCurrentToolSlot)
        {
            if (toolSlots == null)
            {
                Debug.LogError("Tool slots are null during initialization.");
                toolList = new List<Tool>();
            }
            else
            {
                toolList = new List<Tool>(toolSlots);
                toolList.RemoveAll(tool => tool == null);
            }

            currentIndex = playerCurrentToolSlot;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (toolList.Count == 0)
            {
                // 슬롯이 비어 있는 경우 처리
                foreach (var image in slotImages)
                {
                    image.enabled = false;
                }
                toolName.text = string.Empty;
                return;
            }

            // 현재 선택된 툴 이름 표시
            toolName.text = toolList[currentIndex].itemName;

            for (int i = 0; i < slotImages.Length; i++)
            {
                // 중앙(currentIndex)을 기준으로 인덱스 계산
                int offsetIndex = (currentIndex + i - (slotImages.Length / 2) + toolList.Count) % toolList.Count;

                slotImages[i].sprite = toolList[offsetIndex].itemImage;
                slotImages[i].enabled = true;
            }
        }
        public Tool GetCurrentTool()
        {
            if (toolList.Count == 0) return null;

            return toolList[currentIndex];
        }

        public void SetCurrentToolSlot(int newIndex)
        {
            if (toolList.Count == 0) return;

            currentIndex = newIndex % toolList.Count;
            UpdateUI();
        }
    }

}