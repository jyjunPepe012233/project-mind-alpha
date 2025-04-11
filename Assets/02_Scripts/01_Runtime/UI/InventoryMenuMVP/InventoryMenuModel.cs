using System.Collections.Generic;
using System.Linq;
using MinD.SO.Item;

public class InventoryMenuModel
{
    public int CurrentCategoryId { get; private set; }

    private ItemSoList itemSoList;

    public InventoryMenuModel(ItemSoList itemSoList)
    {
        this.itemSoList = itemSoList;
    }

    public void SetCategory(int categoryId)
    {
        CurrentCategoryId = categoryId;
    }

    // 현재 카테고리에 해당하는 아이템 리스트를 반환
    public List<Item> GetCurrentCategoryItems()
    {
        return CurrentCategoryId switch
        {
            0 => SortItems(itemSoList.magicList.Cast<Item>().ToList()),
            1 => SortItems(itemSoList.weaponList.Cast<Item>().ToList()),
            2 => SortItems(itemSoList.toolList.Cast<Item>().ToList()),
            3 => SortItems(itemSoList.protectionList.Cast<Item>().ToList()),
            _ => new List<Item>()
        };
    }

    private List<Item> SortItems(List<Item> items)
    {
        // 정렬 기준: itemCount > 0인 아이템 먼저, itemId 오름차순
        return items
            .Where(item => item.itemCount > 0) // 보유 중인 아이템만
            .OrderByDescending(item => item.itemCount > 0) // 아이템 있는 것 우선 (사실상 전부 true)
            .ThenBy(item => item.itemId)
            .ToList();
    }
}