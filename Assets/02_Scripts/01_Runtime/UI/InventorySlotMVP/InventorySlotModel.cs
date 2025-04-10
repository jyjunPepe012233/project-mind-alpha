using MinD.SO.Item;
using UnityEngine;

public class InventorySlotModel : MonoBehaviour
{
    public Item Item { get; private set; }
    public int Count { get; private set; }
    public bool IsEquipped { get; private set; }

    public InventorySlotModel(Item item, int count, bool isEquipped)
    {
        Item = item;
        Count = count;
        IsEquipped = isEquipped;
    }

    public void SetItem(Item item, int count, bool isEquipped)
    {
        Item = item;
        Count = count;
        IsEquipped = isEquipped;
    }

    public void Clear()
    {
        Item = null;
        Count = 0;
        IsEquipped = false;
    }
}
