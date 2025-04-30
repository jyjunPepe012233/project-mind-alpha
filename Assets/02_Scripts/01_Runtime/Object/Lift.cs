using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MinD.Runtime.Object.Interactables
{
    public class Lift : MonoBehaviour
    {
        [Header("Lift Settings")]
        [SerializeField] public Vector3 upperPosition;
        [SerializeField] public Vector3 lowerPosition;
        [SerializeField] private float moveSpeed = 2.0f;
        [SerializeField] private float waitTimeAtPosition = 1.0f;
        [SerializeField] private bool startAtUpperPosition = false;

        [Header("Passenger Settings")]
        [SerializeField] private Transform passengerParent;
        [SerializeField] private LayerMask passengerLayers;
        [SerializeField] private Vector3 detectionBoxSize = new Vector3(2f, 1f, 2f);
        [SerializeField] private Vector3 detectionBoxOffset = new Vector3(0f, 1f, 0f);
        [SerializeField] private bool useKinematicAttachment = true;

        private Vector3 targetPosition;
        private bool isMoving = false;
        private bool isAtUpperPosition;

        private List<Transform> currentPassengers = new List<Transform>();
        private Dictionary<Transform, Rigidbody> passengerRigidbodies = new Dictionary<Transform, Rigidbody>();
        private Dictionary<Transform, bool> originalKinematicStates = new Dictionary<Transform, bool>();
        private Dictionary<Transform, Transform> originalParents = new Dictionary<Transform, Transform>();

        // CharacterController 탑승자 목록
        private List<CharacterController> characterPassengers = new List<CharacterController>();

        private void Awake()
        {
            if (passengerParent == null)
            {
                GameObject parentObj = new GameObject("PassengerParent");
                parentObj.transform.SetParent(transform);
                parentObj.transform.localPosition = Vector3.zero;
                passengerParent = parentObj.transform;
            }

            isAtUpperPosition = startAtUpperPosition;
            Vector3 startPosition = startAtUpperPosition ? upperPosition : lowerPosition;
            transform.position = startPosition;
            targetPosition = startPosition;
        }

        private void Start()
        {
            float distToUpper = Vector3.Distance(transform.position, upperPosition);
            float distToLower = Vector3.Distance(transform.position, lowerPosition);

            if (distToUpper < distToLower)
            {
                isAtUpperPosition = true;
                transform.position = upperPosition;
            }
            else
            {
                isAtUpperPosition = false;
                transform.position = lowerPosition;
            }
        }

        private void Update()
        {
            Vector3 lastPosition = transform.position;

            if (isMoving)
            {
                if (Application.isPlaying)
                {
                    ManagePassengers();
                }

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );

                // 이동한 만큼 CharacterController 탑승자도 Move로 이동
                Vector3 delta = transform.position - lastPosition;
                if (delta != Vector3.zero && characterPassengers.Count > 0)
                {
                    foreach (var cc in characterPassengers)
                    {
                        if (cc != null)
                        {
                            cc.Move(delta);
                        }
                    }
                }

                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    transform.position = targetPosition;
                    isMoving = false;

                    if (Application.isPlaying)
                    {
                        ReleaseAllPassengers();
                    }
                }
            }
            else if (Application.isPlaying)
            {
                ManagePassengers();
            }
        }

        private void ManagePassengers()
        {
            Collider[] hitColliders = Physics.OverlapBox(
                transform.position + detectionBoxOffset,
                detectionBoxSize / 2,
                Quaternion.identity,
                passengerLayers
            );

            HashSet<Transform> detectedPassengers = new HashSet<Transform>();
            foreach (Collider col in hitColliders)
            {
                Transform passengerRoot = col.transform;
                CharacterController cc = col.GetComponent<CharacterController>();
                if (cc != null)
                {
                    passengerRoot = cc.transform;
                }
                else
                {
                    while (passengerRoot.parent != null &&
                        passengerRoot.parent.GetComponent<CharacterController>() == null &&
                        !passengerRoot.CompareTag("Player") &&
                        passengerRoot.parent.gameObject.layer == passengerRoot.gameObject.layer)
                    {
                        passengerRoot = passengerRoot.parent;
                    }
                }

                detectedPassengers.Add(passengerRoot);

                if (!currentPassengers.Contains(passengerRoot))
                {
                    AttachPassenger(passengerRoot);
                }
            }

            List<Transform> passengersToRemove = new List<Transform>();
            foreach (Transform passenger in currentPassengers)
            {
                if (!detectedPassengers.Contains(passenger))
                {
                    passengersToRemove.Add(passenger);
                }
            }

            foreach (Transform passenger in passengersToRemove)
            {
                DetachPassenger(passenger);
            }
        }

        private void AttachPassenger(Transform passenger)
        {
            if (passenger == null) return;

            currentPassengers.Add(passenger);

            Rigidbody rb = passenger.GetComponent<Rigidbody>();
            CharacterController cc = passenger.GetComponent<CharacterController>();

            originalParents[passenger] = passenger.parent;

            if (useKinematicAttachment && rb != null)
            {
                passengerRigidbodies[passenger] = rb;
                originalKinematicStates[passenger] = rb.isKinematic;
                rb.isKinematic = true;
                passenger.SetParent(passengerParent);
            }
            else if (cc != null)
            {
                // CharacterController는 별도 목록에 추가
                if (!characterPassengers.Contains(cc))
                    characterPassengers.Add(cc);

                passenger.SetParent(passengerParent);
            }
            else
            {
                passenger.SetParent(passengerParent);
            }
        }

        private void DetachPassenger(Transform passenger)
        {
            if (passenger == null) return;

            currentPassengers.Remove(passenger);

            if (originalParents.ContainsKey(passenger))
            {
                passenger.SetParent(originalParents[passenger]);
                originalParents.Remove(passenger);
            }
            else
            {
                passenger.SetParent(null);
            }

            if (passengerRigidbodies.ContainsKey(passenger))
            {
                Rigidbody rb = passengerRigidbodies[passenger];
                if (rb != null && originalKinematicStates.ContainsKey(passenger))
                {
                    rb.isKinematic = originalKinematicStates[passenger];
                }

                passengerRigidbodies.Remove(passenger);
                originalKinematicStates.Remove(passenger);
            }

            // CharacterController라면 목록에서 제거
            CharacterController cc = passenger.GetComponent<CharacterController>();
            if (cc != null && characterPassengers.Contains(cc))
            {
                characterPassengers.Remove(cc);
            }
        }

        private void ReleaseAllPassengers()
        {
            List<Transform> passengersCopy = new List<Transform>(currentPassengers);
            foreach (Transform passenger in passengersCopy)
            {
                DetachPassenger(passenger);
            }

            currentPassengers.Clear();
            passengerRigidbodies.Clear();
            originalKinematicStates.Clear();
            originalParents.Clear();
            characterPassengers.Clear();
        }

        public void TogglePosition()
        {
            if (!isMoving)
            {
                if (Application.isPlaying)
                {
                    StartCoroutine(MoveLift());
                }
                else
                {
                    isAtUpperPosition = !isAtUpperPosition;
                    transform.position = isAtUpperPosition ? upperPosition : lowerPosition;
                }
            }
        }

        private IEnumerator MoveLift()
        {
            yield return new WaitForSeconds(waitTimeAtPosition);

            isAtUpperPosition = !isAtUpperPosition;
            targetPosition = isAtUpperPosition ? upperPosition : lowerPosition;
            isMoving = true;
        }

        public void SetCurrentAsUpper()
        {
            upperPosition = transform.position;
        }

        public void SetCurrentAsLower()
        {
            lowerPosition = transform.position;
        }

        public bool IsMoving()
        {
            return isMoving;
        }

        public bool IsAtUpperPosition()
        {
            return isAtUpperPosition;
        }

        public void MoveToUpper()
        {
            if (!isMoving && !isAtUpperPosition)
            {
                if (Application.isPlaying)
                {
                    isAtUpperPosition = true;
                    targetPosition = upperPosition;
                    StartCoroutine(MoveLift());
                }
                else
                {
                    isAtUpperPosition = true;
                    transform.position = upperPosition;
                }
            }
        }

        public void MoveToLower()
        {
            if (!isMoving && isAtUpperPosition)
            {
                if (Application.isPlaying)
                {
                    isAtUpperPosition = false;
                    targetPosition = lowerPosition;
                    StartCoroutine(MoveLift());
                }
                else
                {
                    isAtUpperPosition = false;
                    transform.position = lowerPosition;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(upperPosition, transform.localScale);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(lowerPosition, transform.localScale);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(upperPosition, lowerPosition);

            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(detectionBoxOffset, detectionBoxSize);
        }
    }
}
