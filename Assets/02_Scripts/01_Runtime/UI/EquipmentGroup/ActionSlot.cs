using UnityEngine;

public class ActionSlot : MonoBehaviour
{
    public GameObject selectionIndicator; // 선택 상태를 나타내는 UI 요소

    public void SetSelected(bool isSelected)
    {
        selectionIndicator.SetActive(isSelected); // 선택 시 활성화, 비선택 시 비활성화
    }
}