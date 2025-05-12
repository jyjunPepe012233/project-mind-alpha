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
    public List<Image> magicQuickSlots;
    public TextMeshProUGUI magicNameTMP;
    public Image magicEffectImage;

    [Header("Tool QuickSlot UI")]
    public List<Image> toolQuickSlots;
    public TextMeshProUGUI toolNameTMP;
    public Image cooldownImage;
    public Image toolEffectImage;

    [SerializeField] private Item[] magicSlots;
    [SerializeField] private Item[] toolSlots;

    [SerializeField] private int currentMagicIndex;
    [SerializeField] private int currentToolIndex;

    private Dictionary<int, (float startTime, float endTime)> toolCooldownTimers = new();


    private void Update()
    {
        UpdateCooldownUI();
    }

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
                slot.color = new Color(1, 1, 1, 0);
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
                slot.color = new Color(1, 1, 1, 0);
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
        PlaySwapEffect(magicEffectImage);
    }

    public void HandleToolSlotSwapping(int direction)
    {
        var equippedTools = toolSlots.Where(t => t != null && t.isEquipped).ToList();
        if (equippedTools.Count == 0) return;

        var previousCenterItem = GetCircularTriple(equippedTools, currentToolIndex)[1];
        int previousTypeNumber = previousCenterItem.itemTypeNumber;

        currentToolIndex = (currentToolIndex + direction + equippedTools.Count) % equippedTools.Count;

        var newCenterItem = GetCircularTriple(equippedTools, currentToolIndex)[1];
        int newTypeNumber = newCenterItem.itemTypeNumber;

        UpdateToolQuickSlot();
        PlaySwapEffect(toolEffectImage);
    }

    public void StartCooldownOnToolSlot(float cooldownDuration)
    {
        var equippedTools = toolSlots.Where(t => t != null && t.isEquipped).ToList();
        if (equippedTools.Count == 0) return;

        var currentItem = GetCircularTriple(equippedTools, currentToolIndex)[1];
        int type = currentItem.itemTypeNumber;

        float startTime = Time.time;
        float endTime = Time.time + cooldownDuration;

        toolCooldownTimers[type] = (startTime, endTime);
    }

    public void UpdateCooldownUI()
    {
        var equippedTools = toolSlots.Where(t => t != null && t.isEquipped).ToList();
        if (equippedTools.Count == 0) return;

        var currentItem = GetCircularTriple(equippedTools, currentToolIndex)[1];
        int type = currentItem.itemTypeNumber;

        if (toolCooldownTimers.TryGetValue(type, out var timer))
        {
            float now = Time.time;
            float duration = timer.endTime - timer.startTime;
            float remaining = timer.endTime - now;

            if (remaining <= 0f)
            {
                cooldownImage.fillAmount = 0f;
                toolCooldownTimers.Remove(type);
            }
            else
            {
                cooldownImage.fillAmount = Mathf.Clamp01(remaining / duration);
            }
        }
        else
        {
            cooldownImage.fillAmount = 0f;
        }
    }


    private void PlaySwapEffect(Image effectImage)
    {
        if (effectImage == null) return;
        StartCoroutine(FadeEffect(effectImage));
    }

    private IEnumerator FadeEffect(Image image, float fadeTime = 0.2f)
    {
        float elapsed = 0f;

        Color color = image.color;
        color.a = 0f;
        image.color = color;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            image.color = color;
            yield return null;
        }
    }
}
