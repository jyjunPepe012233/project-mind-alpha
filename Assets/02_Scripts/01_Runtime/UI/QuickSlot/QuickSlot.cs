using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using MinD.SO.Item;

public class QuickSlot : MonoBehaviour
{
    [Header("Magic QuickSlot UI")]
    public List<Image> magicQuickSlots; // 왼, 가운데, 오
    public TextMeshProUGUI magicNameTMP;

    [Header("Tool QuickSlot UI")]
    public List<Image> toolQuickSlots;
    public TextMeshProUGUI toolNameTMP;
    public Image cooldownImage;

    [SerializeField] private Item[] magicSlots;
    [SerializeField] private Item[] toolSlots;

    [SerializeField] private int currentMagicIndex;
    [SerializeField] private int currentToolIndex;

    public void Initialize(Magic[] magicSlots, Tool[] toolSlots)
    {
        this.magicSlots = magicSlots;
        this.toolSlots = toolSlots;

        currentMagicIndex = 0;
        currentToolIndex = 0;

        UpdateMagicQuickSlot();
        UpdateToolQuickSlot();
    }

    public void UpdateMagicQuickSlot()
    {
        if (magicSlots == null || magicSlots.Length == 0) return;

        var equippedMagics = magicSlots.Where(m => m != null && m.isEquipped).ToList();
        if (equippedMagics.Count == 0)
        {
            foreach (var slot in magicQuickSlots)
            {
                slot.sprite = null;
                slot.color = new Color(1, 1, 1, 0); // 투명하게
            }
            magicNameTMP.text = "";
            return;
        }

        currentMagicIndex %= equippedMagics.Count;

        var displayItems = GetCircularTriple(equippedMagics, currentMagicIndex);

        for (int i = 0; i < 3; i++)
        {
            magicQuickSlots[i].sprite = displayItems[i].itemImage;
            magicQuickSlots[i].color = Color.white;
        }

        magicNameTMP.text = displayItems[1].itemName;
    }

    public void UpdateToolQuickSlot()
    {
        if (toolSlots == null || toolSlots.Length == 0) return;

        var equippedTools = toolSlots.Where(t => t != null && t.isEquipped).ToList();
        if (equippedTools.Count == 0)
        {
            foreach (var slot in toolQuickSlots)
            {
                slot.sprite = null;
                slot.color = new Color(1, 1, 1, 0); // 투명하게
            }
            toolNameTMP.text = "";
            return;
        }

        currentToolIndex %= equippedTools.Count;

        var displayItems = GetCircularTriple(equippedTools, currentToolIndex);

        for (int i = 0; i < 3; i++)
        {
            toolQuickSlots[i].sprite = displayItems[i].itemImage;
            toolQuickSlots[i].color = Color.white;
        }

        toolNameTMP.text = displayItems[1].itemName;
    }


    private List<Item> GetCircularTriple(List<Item> items, int centerIndex)
    {
        int count = items.Count;
        Item left = items[(centerIndex - 1 + count) % count];
        Item center = items[centerIndex];
        Item right = items[(centerIndex + 1) % count];

        return new List<Item> { left, center, right };
    }

    public void HandleMagicSlotSwapping(int direction)
    {
        var equippedMagics = magicSlots.Where(m => m != null && m.isEquipped).ToList();
        if (equippedMagics.Count == 0) return;

        currentMagicIndex = (currentMagicIndex + direction + equippedMagics.Count) % equippedMagics.Count;
        UpdateMagicQuickSlot();
    }

    public void HandleToolSlotSwapping(int direction)
    {
        var equippedTools = toolSlots.Where(t => t != null && t.isEquipped).ToList();
        if (equippedTools.Count == 0) return;

        currentToolIndex = (currentToolIndex + direction + equippedTools.Count) % equippedTools.Count;
        UpdateToolQuickSlot();
    }

    // Cooldown method for the Tool slot center image
    public void StartCooldownOnToolSlot(float cooldownDuration)
    {
        if (cooldownImage != null)
        {
            StartCoroutine(CooldownCoroutine(cooldownDuration));
        }
    }

    private IEnumerator CooldownCoroutine(float cooldownDuration)
    {
        float elapsedTime = 0f;
        cooldownImage.fillAmount = 1f; // 시작은 가득 참

        while (elapsedTime < cooldownDuration)
        {
            elapsedTime += Time.deltaTime;
            cooldownImage.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / cooldownDuration);
            yield return null;
        }

        cooldownImage.fillAmount = 0f; // 끝에 확실히 비움
    }
}
