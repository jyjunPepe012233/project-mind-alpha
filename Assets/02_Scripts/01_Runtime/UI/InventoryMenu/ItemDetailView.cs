using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MinD.SO.Item;

public class ItemDetailView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameTMP;
    [SerializeField] private TextMeshProUGUI itemDescriptionTMP;
    [SerializeField] private Image itemImage;

    public void ShowItemDetails(Item item)
    {
        if (item == null)
        {
            Clear();
            return;
        }

        itemNameTMP.text = item.itemName;
        itemDescriptionTMP.text = item.itemDescription;
        itemImage.sprite = item.itemImage;
        itemImage.enabled = true;
    }

    public void Clear()
    {
        itemNameTMP.text = "";
        itemDescriptionTMP.text = "";
        itemImage.sprite = null;
        itemImage.enabled = false;
    }
}