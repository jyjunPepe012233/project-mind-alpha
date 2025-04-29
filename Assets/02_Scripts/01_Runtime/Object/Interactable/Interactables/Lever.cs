using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.Runtime.Object.Interactables
{
    public class Lever : Interactable
    {
        [Header("Lever Settings")] 
        [SerializeField] private Transform lever;
        [SerializeField] private Door targetDoor; // 연동할 Door 참조
        [SerializeField] private bool oneTimeUse = true; // 한 번만 사용할 수 있는지
        [SerializeField] private float rotationSpeed = 60f; // 1초에 180도 회전 (원하는 값으로 조절)
        private bool isUsed = false;
        private bool isRotating = false;
        private Quaternion targetRotation;

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
                }
            }
        }

        public override void Interact(Player interactor)
        {
            if (!canInteraction || isUsed)
                return;

            if (targetDoor != null)
            {
                interactor.animation.PlayTargetAction("Anchor_Discover", true, true, false, false);
                targetDoor.Unlock();
            }

            if (lever != null && !isRotating)
            {
                targetRotation = lever.localRotation * Quaternion.Euler(60f, 0f, 0f);
                isRotating = true;
            }

            if (oneTimeUse)
            {
                isUsed = true;
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
