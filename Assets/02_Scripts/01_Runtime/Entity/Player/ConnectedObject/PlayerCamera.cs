using System.Collections.Generic;
using MinD.Runtime.Managers;
using MinD.Utility;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerCamera : MonoBehaviour {
	
	private const float lockOnInputPoint = 3.5f; // MINIMUM DELTA MOUSE MOVEMENT 

	[HideInInspector] public Player owner; 
	private Camera _camera;
	public Camera camera {
		get {
			if (_camera == null) {
				_camera = GetComponent<Camera>();
			}
			return _camera;
		}
	}

	public float rotationMultiplier = 1.5f;
	

	[Header("[ Settings ]")]
	[SerializeField] private Vector3 cameraOffset;
	[SerializeField, Range(0, 90)] private float limitAngleAbove = 40;
	[SerializeField, Range(0, 90)] private float limitAngleBelow = 60;
	
	[Space(7)]
	public float cameraMaxDistance = 3.5f;
	[SerializeField] private float cameraFollowSpeed = 13;
	[SerializeField] private float cameraRadius = 0.3f;
	[SerializeField] private LayerMask cameraCollisionMask;
	
	[Space(7)]
	[SerializeField] private float lockOnAngle = 50;
	[SerializeField] private float lockOnMaxRadius = 50;
	
	
	[HideInInspector]
	public Transform currentTargetOption;
	private List<Transform> availableTargets = new List<Transform>(); // SORTED BY PROXIMITY


	// POSITION OF VIRTUAL CAMERA ARM
	private Vector3 targetCameraArm; // LERP TARGET POSITION
	private Vector3 cameraArm; // CURRENT POSITION
	
	private float cameraDistance; // CURRENT CAMERA DISTANCE(NOT MAX DISTANCE)

	private float releaseLockOnTimer;


	public void MoveCameraToAppropriatePosition()
	{
		targetCameraArm = owner.transform.position + Quaternion.Euler(transform.eulerAngles) * cameraOffset;
		cameraArm = targetCameraArm;
	}

	public void HandleCamera() {

		HandleLockOn();
		HandleFollowTarget();
		HandleRotation();
		HandleCollision();

	}



	private void HandleFollowTarget() {

		Vector3 angle = transform.eulerAngles;
		angle.x = 0;

		targetCameraArm = owner.transform.position + Quaternion.Euler(angle) * cameraOffset;

		cameraArm = Vector3.Lerp(cameraArm, targetCameraArm, Time.deltaTime * cameraFollowSpeed);
	}

	private void HandleRotation() {

		if (PlayerHUDManager.Instance.currentShowingMenu != null) {
			// DO NOT ROTATE
			
			
		} else if (owner.isLockOn) {
			// AUTO ROTATION BY LOCK ON

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentTargetOption.position - transform.position), 10 * Time.deltaTime);
			Vector3 angle = transform.eulerAngles;


			// LIMIT ANGLE
			if (angle.x > 180)
				angle.x = Mathf.Clamp(angle.x, 360 - limitAngleAbove, 370);
			else
				angle.x = Mathf.Clamp(angle.x, -10, limitAngleBelow);

			angle.z = 0;
			transform.eulerAngles = angle;

		} else {

			Vector2 rotationInput = PlayerInputManager.Instance.rotationInput;
			Vector3 angle = transform.eulerAngles + new Vector3(-rotationInput.y * 0.35f, rotationInput.x) * rotationMultiplier;


			// LIMIT ANGLE
			if (angle.x > 180)
				angle.x = Mathf.Clamp(angle.x, 360 - limitAngleAbove, 370);
			else
				angle.x = Mathf.Clamp(angle.x, -10, limitAngleBelow);

			angle.z = 0;
			transform.eulerAngles = angle;
		}
		
		transform.position = cameraArm + (transform.rotation * Vector3.back * cameraDistance);
	}

	private void HandleCollision() {

		Ray ray = new Ray(cameraArm, transform.rotation * Vector3.back);
		if (Physics.SphereCast(ray, cameraRadius, out RaycastHit hitInfo, cameraMaxDistance, cameraCollisionMask)) {

			cameraDistance = Vector3.Distance(hitInfo.point, ray.origin);

		} else
			cameraDistance = cameraMaxDistance;
	}

	private void HandleLockOn() {

		if (PlayerInputManager.Instance.lockOnInput) {
			PlayerInputManager.Instance.lockOnInput = false;

			if (owner.isLockOn) {
				RemoveLockOnTarget();
			} else {
				SetLockOnTarget();
			}
		}

		if (owner.isLockOn) {

			// CHECK TARGET IS DESTROYED OR DYING, WHATEVER TARGET IS UNSUITABLE AS TARGET 
			bool currentTargetIsAvailable = true;
			try {
				if (currentTargetOption.GetComponentInParent<BaseEntity>().isDeath
				    || !currentTargetOption.gameObject.activeSelf) {
					currentTargetIsAvailable = false;
				}
			} catch {
				currentTargetIsAvailable = false;
			}

			if (!currentTargetIsAvailable) {
				SetLockOnTarget();
				
			} else if (Vector3.Angle(transform.forward, currentTargetOption.position - transform.position) > lockOnAngle) {
				// IF TARGET IS OUT OF ALLOWED ANGLE
				
				releaseLockOnTimer += Time.deltaTime;
				
				if (releaseLockOnTimer > 0.5f) {
					// IF TARGET IS OUT OF ALLOWED ANGLE FOR A TIME, REFRESH TARGET
					
					SetLockOnTarget();
				}
				
			} else if (PlayerInputManager.Instance.rotationInput.x < -3.5) {
				MoveLockOnToLeftTarget();
			} else if (PlayerInputManager.Instance.rotationInput.x > 3.5) {
				MoveLockOnToRightTarget();
				
			} else {
				releaseLockOnTimer = 0;
			}

		}
	}

	

	private void SetLockOnTarget() {
		
		RemoveLockOnTarget();
		

		// GET ENTITY COLLIDERS IN AVAILABLE RADIUS
		Collider[] colliders = Physics.OverlapSphere(transform.position, lockOnMaxRadius, WorldUtility.damageableLayerMask);

		if (colliders.Length == 0) {
			return;
		}


		// CHECK AVAILABLE TARGETS
		availableTargets.Clear();
		foreach (Collider collider in colliders) {
			
			// GET ENTITY
			BaseEntity targetEntity = null;
			
			targetEntity = collider.GetComponentInParent<BaseEntity>();
			if (targetEntity == null) {
				targetEntity = collider.GetComponent<BaseEntity>();

				if (targetEntity == null) {
					continue;
				}
			}

			// CHECK CONDITION OF TARGET ENTITY
			if (targetEntity.isDeath) {
				continue;
			}
			

			// CHECK OPTIONS
			List<Transform> options = targetEntity.targetOptions;
			for (int i = 0; i < options.Count; i++) {

				// CHECK TARGET IS SELF
				if (owner.targetOptions.Contains(options[i]))
					continue;

				// CHECK TARGET IS ALREADY EXIST IN LIST
				if (availableTargets.Contains(options[i]))
					continue;

				// CHECK ANGLE AVAILABLE
				if (Mathf.Abs(Vector3.SignedAngle(transform.forward, (options[i].position - transform.position), Vector3.up)) > lockOnAngle)
					continue;

				// CHECK OBSTACLE BETWEEN CAMERA AND TARGET
				if (Physics.Linecast(owner.targetOptions[0].position, options[i].position, WorldUtility.environmentLayerMask))
					continue;

				availableTargets.Add(options[i]);
			}
		}


		// SORTING TARGET OPTIONS BY PROXIMITY
		for (int i = 0; i < availableTargets.Count; i++) {
			// SELECTION SORT

			Transform closest = availableTargets[i];

			for (int j = i; j < availableTargets.Count; j++) {

				Transform thisOption = availableTargets[j];

				if (Vector3.Distance(transform.position, thisOption.position) < Vector3.Distance(transform.position, closest.position))
					(availableTargets[i], availableTargets[j]) = (availableTargets[j], availableTargets[i]);
			}
		}

		if (availableTargets.Count > 0) {

			currentTargetOption = availableTargets[0];
			owner.combat.target = currentTargetOption.GetComponentInParent<BaseEntity>();
			owner.isLockOn = true;
			
			PlayerHUDManager.Instance.SetLockOnSpotActive(true);

		}

	}
	private void RemoveLockOnTarget() {

		currentTargetOption = null;
		owner.isLockOn = false;
		
		PlayerHUDManager.Instance.SetLockOnSpotActive(false);

	}
	
	private void MoveLockOnToLeftTarget() {

		foreach (Transform option in availableTargets) {

			if (option == currentTargetOption)
				continue;

			if (transform.InverseTransformPoint(option.position).x < 0) {
				currentTargetOption = option;
				owner.combat.target = currentTargetOption.GetComponentInParent<BaseEntity>(); // TARGET UPDATE
				return;
			}
		}
	}
	private void MoveLockOnToRightTarget() {

		foreach (Transform option in availableTargets) {

			if (option == currentTargetOption)
				continue;

			if (transform.InverseTransformPoint(option.position).x > 0) {
				currentTargetOption = option;
				owner.combat.target = currentTargetOption.GetComponentInParent<BaseEntity>(); // TARGET UPDATE
				return;
			}
		}
	}
}

}