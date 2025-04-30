using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.Runtime.Object.Interactables
{
    public class Lever : Interactable
    {
        [Header("Lever Settings")] 
        [SerializeField] private Transform lever;
        [SerializeField] private float rotationSpeed = 60f; // 1초에 60도 회전
        [SerializeField] private bool oneTimeUse = true; // 한 번만 사용할 수 있는지
        
        [Header("Connected Objects")]
        [SerializeField] private Door targetDoor; // 연동할 Door 참조
        [SerializeField] private Lift targetLift; // 연동할 Lift 참조
        [SerializeField] private bool controlsDoor = true; // Door를 제어하는지 여부
        [SerializeField] private bool controlsLift = false; // Lift를 제어하는지 여부
        
        private bool isUsed = false;
        private bool isRotating = false;
        private Quaternion targetRotation;
        private bool isLeverUp = false; // 레버가 올라가 있는지 확인하는 변수

        private const string DEFAULT_INTERACTION_TEXT = "레버를 당긴다";
        private const string USED_INTERACTION_TEXT = "이미 사용된 레버다";

        private void Awake()
        {
            UpdateInteractionText();
            if (lever != null)
                targetRotation = lever.localRotation;
        }

        private void Update()
        {
            if (isRotating && lever != null)
            {
                lever.localRotation = Quaternion.RotateTowards(
                    lever.localRotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );

                if (Quaternion.Angle(lever.localRotation, targetRotation) < 0.1f)
                {
                    lever.localRotation = targetRotation;
                    isRotating = false;
                    
                    // 레버 회전이 끝나면 다시 상호작용 가능하도록 설정 (oneTimeUse가 아닌 경우)
                    if (!oneTimeUse)
                    {
                        canInteraction = true;
                    }
                }
            }
        }

        public override void Interact(Player interactor)
        {
            // 상호작용이 불가능하거나 사용된 레버이거나 회전 중인 경우 상호작용 방지
            if (!canInteraction || isUsed || isRotating || targetLift.IsMoving())
            {
                return;
            }

            // 플레이어 애니메이션 재생
            interactor.animation.PlayTargetAction("Anchor_Discover", true, true, false, false);

            // Door 제어 (설정된 경우)
            if (controlsDoor && targetDoor != null)
            {
                targetDoor.Unlock();
            }

            // Lift 제어 (설정된 경우)
            if (controlsLift && targetLift != null)
            {
                targetLift.TogglePosition();
            }

            // 레버 회전 애니메이션
            if (lever != null)
            {
                // 레버 회전 중에는 상호작용 비활성화
                isRotating = true;
                canInteraction = false;
                
                // 레버 상태에 따라 회전 방향 결정
                float rotationAngle = isLeverUp ? -60f : 60f;
                targetRotation = lever.localRotation * Quaternion.Euler(rotationAngle, 0f, 0f);
                
                // 레버 상태 토글
                isLeverUp = !isLeverUp;
            }

            // 한 번만 사용하는 레버인 경우
            if (oneTimeUse)
            {
                isUsed = true;
                // oneTimeUse가 true인 경우 상호작용 비활성화 상태 유지
                canInteraction = false;
                UpdateInteractionText();
            }
        }

        private void UpdateInteractionText()
        {
            interactionText = isUsed ? USED_INTERACTION_TEXT : DEFAULT_INTERACTION_TEXT;
        }
    }
}