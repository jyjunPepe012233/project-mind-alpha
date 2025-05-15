using System;
using System.Collections.Generic;
using MinD.Runtime.Managers;
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
    
    private bool magic = false;
    private bool tool = false;
    private bool staff = false;
    private bool protection = false;
    private bool torch = false;

    public void SetTutorial(int categoryid)
    {
        switch (categoryid)
        {
            case 0:
                if (magic) return;
                PlayerHUDManager.Instance.ShowTutorialPopup("마법", "장착한 후 [ 마우스 왼쪽 클릭 ] 으로 사용가능하다.", 10f);
                magic = true;
                break;
            case 1:
                if (staff) return;
                PlayerHUDManager.Instance.ShowTutorialPopup("스태프", "스태프를 장착해야만 마법이 사용가능하다.", 10f);
                staff = true;
                break;
            case 2:
                if (tool) return;
                PlayerHUDManager.Instance.ShowTutorialPopup("소모품", "장착한 후 [ R ] 로 사용가능하다.", 10f);
                tool = true;
                break;
            case 3:
                if (protection) return;
                PlayerHUDManager.Instance.ShowTutorialPopup("수호", "장착한 후 [ 마우스 오른쪽 클릭 ] 을 길게 눌러 사용가능하다.", 10f);
                protection = true;
                break;
            case 4:
                if (torch) return;
                PlayerHUDManager.Instance.ShowTutorialPopup("락온", "적에게 [ Q 또는 마우스 휠 클릭 ] 을 하면 락온 이 가능하다.", 10f);
                torch = true;
                break;
            default:
                return;
        }
    }
}