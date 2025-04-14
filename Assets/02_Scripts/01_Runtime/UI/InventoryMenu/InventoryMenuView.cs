using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryMenuView : MonoBehaviour
{
    [Header("인벤토리 프리펩")] 
    [SerializeField] private GameObject inventoryMenu;
    
    [Header("카테고리 컨텐츠들")]
    [SerializeField] private GameObject magicContents;
    [SerializeField] private GameObject staffContents;
    [SerializeField] private GameObject toolContents;
    [SerializeField] private GameObject protectionContents;

    [Header("카테고리 UI 표시")]
    [SerializeField] private TextMeshProUGUI categoryTMP;
    [SerializeField] private GameObject magicCategoryIcon;
    [SerializeField] private GameObject staffCategoryIcon;
    [SerializeField] private GameObject toolCategoryIcon;
    [SerializeField] private GameObject protectionCategoryIcon;

    private Dictionary<int, GameObject> contentMap;
    private Dictionary<int, List<InventorySlot>> slotMap;
    private Dictionary<int, GameObject> iconMap;

    private void Awake()
    {
        contentMap = new Dictionary<int, GameObject>()
        {
            { 0, magicContents },
            { 1, staffContents },
            { 2, toolContents },
            { 3, protectionContents }
        };

        slotMap = new Dictionary<int, List<InventorySlot>>();

        // 자동 슬롯 수집
        foreach (var kvp in contentMap)
        {
            var slotList = new List<InventorySlot>();
            foreach (Transform child in kvp.Value.transform)
            {
                var slot = child.GetComponent<InventorySlot>();
                if (slot != null)
                    slotList.Add(slot);
            }
            slotMap[kvp.Key] = slotList;
        }

        iconMap = new Dictionary<int, GameObject>()
        {
            { 0, magicCategoryIcon },
            { 1, staffCategoryIcon },
            { 2, toolCategoryIcon },
            { 3, protectionCategoryIcon }
        };
    }

    public void SetActiveCategory(int categoryId)
    {
        // 콘텐츠 활성화 / 비활성화
        foreach (var kvp in contentMap)
            kvp.Value.SetActive(kvp.Key == categoryId);

        // 카테고리 아이콘 하이라이트
        foreach (var kvp in iconMap)
            kvp.Value.SetActive(kvp.Key == categoryId);

        // 카테고리 텍스트 변경
        string categoryName = categoryId switch
        {
            0 => "마법",
            1 => "스태프",
            2 => "소모품",
            3 => "수호",
            _ => ""
        };
        categoryTMP.text = categoryName;
    }

    public List<InventorySlot> GetCurrentSlots(int categoryId)
    {
        return slotMap.ContainsKey(categoryId) ? slotMap[categoryId] : null;
    }

    public void SetActiveInventoryMenu(bool isOpen)
    {
        inventoryMenu.SetActive(isOpen);
    }
}
