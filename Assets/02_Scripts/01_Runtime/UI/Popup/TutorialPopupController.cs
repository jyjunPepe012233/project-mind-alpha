using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopupController : MonoBehaviour
{
    [SerializeField] private Transform popupParent;
    [SerializeField] private TutorialPopupView popupPrefab;

    [SerializeField] private int maxPopupCount = 3;
    [SerializeField] private float spacing = 280f;

    private readonly LinkedList<TutorialPopupView> activePopups = new LinkedList<TutorialPopupView>();

    public void ShowPopup(string title, string content, float displayTime)
    {
        if (activePopups.Count >= maxPopupCount)
        {
            TutorialPopupView oldest = activePopups.Last.Value;
            activePopups.RemoveLast();
            Destroy(oldest.gameObject);
        }

        TutorialPopupView newPopup = Instantiate(popupPrefab, popupParent);
        newPopup.transform.SetAsFirstSibling();

        // 팝업이 닫힐 때 호출될 액션 등록
        newPopup.OnPopupClosed += HandlePopupClosed;
        activePopups.AddFirst(newPopup);

        newPopup.ShowWithTimer(title, content, displayTime);

        RefreshPopupPositions();
    }

    private void HandlePopupClosed(TutorialPopupView closedPopup)
    {
        if (activePopups.Contains(closedPopup))
        {
            activePopups.Remove(closedPopup);
            RefreshPopupPositions();
        }
    }

    private void RefreshPopupPositions()
    {
        int count = activePopups.Count;
        int index = 0;

        float centerOffset = (count - 1) * 0.5f;

        foreach (var popup in activePopups)
        {
            float offsetY = (index - centerOffset) * spacing;
            RectTransform rt = popup.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, offsetY);
            index++;
        }
    }
}