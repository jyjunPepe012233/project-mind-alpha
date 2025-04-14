using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlotView : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image itemImage;

    public void SetItem(Sprite sprite, bool isEquipped)
    {
        itemImage.sprite = sprite;
        itemImage.enabled = sprite != null;
    }

    public void ClearSlot()
    {
        itemImage.sprite = null;
        itemImage.enabled = false;
    }
}