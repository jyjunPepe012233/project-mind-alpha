using MinD.Runtime.Entity;
using System.Collections.Generic;
using UnityEngine;

namespace MinD.Runtime.Object.Interactables {
    public class Door : Interactable {

        public enum DoorType { Single, Double }

        [System.Serializable]
        public class DoorLeaf {
            public Transform doorTransform;
            public bool isLeft = true;
            public float openAngle = 60f;
            [HideInInspector] public Quaternion initialRotation;
            [HideInInspector] public Quaternion targetRotation;
        }

        [Header("Door Settings")]
        [SerializeField] private DoorType doorType = DoorType.Single;
        [SerializeField] private float openSpeed = 10f;
        [SerializeField] private bool isLocked = false;
        
        [Header("Door Configuration")]
        [SerializeField] private DoorLeaf primaryDoor;
        [SerializeField] private DoorLeaf secondaryDoor;

        private bool isOpened = false;
        private bool isRotating = false;
        private List<DoorLeaf> activeDoors = new List<DoorLeaf>();
        
        // Default interaction text
        private const string DEFAULT_INTERACTION_TEXT = "문을 연다";
        private const string LOCKED_INTERACTION_TEXT = "단단히 잠겨있다";

        private void Awake() {
            activeDoors.Add(primaryDoor);
            primaryDoor.initialRotation = primaryDoor.doorTransform.rotation;
            
            if (doorType == DoorType.Double && secondaryDoor != null && secondaryDoor.doorTransform != null) {
                activeDoors.Add(secondaryDoor);
                secondaryDoor.initialRotation = secondaryDoor.doorTransform.rotation;
            }
            
            foreach (var door in activeDoors) {
                float angle = door.isLeft ? -door.openAngle : door.openAngle;
                door.targetRotation = Quaternion.Euler(
                    door.doorTransform.eulerAngles.x,
                    door.doorTransform.eulerAngles.y + angle,
                    door.doorTransform.eulerAngles.z
                );
            }
            
            // Set initial interaction text
            UpdateInteractionText();
        }

        private void Update() {
            if (!isRotating) return;
            
            bool allDoorsInPosition = true;
            
            foreach (var door in activeDoors) {
                if (door.doorTransform == null) continue;
                
                Quaternion targetRot = isOpened ? door.targetRotation : door.initialRotation;
                door.doorTransform.rotation = Quaternion.RotateTowards(
                    door.doorTransform.rotation, 
                    targetRot, 
                    openSpeed * Time.deltaTime
                );
                
                if (Quaternion.Angle(door.doorTransform.rotation, targetRot) > 0.1f) {
                    allDoorsInPosition = false;
                }
            }
            
            if (allDoorsInPosition) {
                isRotating = false;
                
                if (isOpened) {
                    canInteraction = false;
                }
            }
        }

        public override void Interact(Player interactor) {
            if (isRotating) return;
            
            if (isLocked) {
                return;
            }

            if (!isOpened) {
                interactor.animation.PlayTargetAction("Anchor_Discover", true, true, false, false);
                Open();
            }
        }
        
        public void Open() {
            if (isOpened || isRotating) return;
            
            isOpened = true;
            isRotating = true;
        }

        public void Unlock() {
            isLocked = false;
            UpdateInteractionText();
        }

        public void Lock() {
            isLocked = true;
            UpdateInteractionText();
        }
        
        private void UpdateInteractionText() {
            interactionText = isLocked ? LOCKED_INTERACTION_TEXT : DEFAULT_INTERACTION_TEXT;
        }
    }
}