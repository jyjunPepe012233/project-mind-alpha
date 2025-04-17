using MinD.SO.Item;
using UnityEngine;

public class InventorySlotTrashTest : MonoBehaviour
{
    [SerializeField] private InventorySlot inventorySlot; // 인벤토리 슬롯 컴포넌트
    [SerializeField] private Item sampleItem; // 예시 아이템 (Unity Inspector에서 할당)

    private void Start()
    {
        // 슬롯에 아이템을 설정하여 테스트
        inventorySlot.SetItem(sampleItem, 5, false); // 아이템을 설정하고 수량을 5로, 장착 상태는 false로 설정

        // 슬롯을 선택된 상태로 설정
        inventorySlot.SetSelected(true);
        
        // 테스트 시작 시 콘솔에 로그 출력
        Debug.Log("Inventory Slot Test Started");
    }

    private void Update()
    {
        // 테스트: 키 입력을 통해 슬롯 상태를 변경합니다.

        // 'UpArrow' 키를 눌러 아이템 수 증가
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int newCount = inventorySlot.GetType().GetField("count", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(inventorySlot) as int? ?? 0;
            inventorySlot.SetItemCount(newCount + 1);
            Debug.Log("Item count increased to " + (newCount + 1));
        }

        // 'DownArrow' 키를 눌러 아이템 수 감소 (최소 1)
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int newCount = inventorySlot.GetType().GetField("count", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(inventorySlot) as int? ?? 0;
            if (newCount > 1)
            {
                inventorySlot.SetItemCount(newCount - 1);
                Debug.Log("Item count decreased to " + (newCount - 1));
            }
        }

        // 'Space' 키를 눌러 장착 상태 변경 (아이템이 장착되었으면 해제, 그렇지 않으면 장착)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventorySlot.ToggleEquippedState();
            Debug.Log("Item equipped state toggled: " + (inventorySlot.GetType().GetField("isEquipped", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(inventorySlot) as bool? ?? false));
        }

        // 'C' 키를 눌러 슬롯을 비움
        if (Input.GetKeyDown(KeyCode.C))
        {
            inventorySlot.Clear();
            Debug.Log("Inventory Slot cleared");
        }
    }
}
