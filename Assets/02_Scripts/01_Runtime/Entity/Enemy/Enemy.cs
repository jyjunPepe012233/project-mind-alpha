using MinD.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using MinD.Runtime.Managers;
using MinD.SO.EnemySO;
using MinD.SO.StatusFX.Effects;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyAnimationHandler))]
[RequireComponent(typeof(EnemyAttributeHandler))]
[RequireComponent(typeof(EnemyLocomotionHandler))]
[RequireComponent(typeof(EnemyCombatHandler))]
[RequireComponent(typeof(EnemyUtilityHandler))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : BaseEntity, IWorldIndexable {
	
	[HideInInspector] public NavMeshAgent navAgent;
	
	[HideInInspector] public EnemyStateMachine state;
	[HideInInspector] public EnemyAnimationHandler animation;
	[HideInInspector] public EnemyAttributeHandler attribute;
	[HideInInspector] public EnemyLocomotionHandler locomotion;
	[HideInInspector] public EnemyCombatHandler combat;
	[HideInInspector] public EnemyUtilityHandler utility;

	[SerializeField, HideInInspector] private bool _hasBeenIndexed;
	public bool hasBeenIndexed {
		get => _hasBeenIndexed;
		set => _hasBeenIndexed = value;
	}

	[SerializeField, HideInInspector] private int _worldIndex;
	public int worldIndex {
		get => _worldIndex;
		set => _worldIndex = value;
	}
	
	[HideInInspector] public Vector3 worldPlacedPosition;
	[HideInInspector] public Quaternion worldPlacedRotation;
	
	public BaseEntity currentTarget;
	
	public override int CurHp {
		get => curHp;
		set {
			curHp = value;
			if (curHp <= 0) {
				OnDeath();
			}
			curHp = Mathf.Clamp(curHp, 0, attribute.MaxHp);
			combat.HandleLimitedAttacks();
		}
	}
	
	[Header("[ Flags ]")]
	public bool isPerformingAction;
	public bool isInAttack;
	
	[Header("[ States ]")]
	public EnemyState currentState; 
	public IdleState idleState; 
	public PursueTargetState pursueTargetState;
	public CombatStanceState combatStanceState;
	
	private AttackState _attackState;
	public AttackState attackState {
		get {
			if (_attackState == null) {
				_attackState = new();
			}
			return _attackState;
		}
	} // ATTACK STATE IS INSTANTIATING AUTOMATICALLY
	
	

	// SETUP A OBJECT SETTINGS
	protected override void Setup() {

		base.Setup();

		navAgent = GetComponent<NavMeshAgent>();

		state = GetComponent<EnemyStateMachine>();
		animation = GetComponent<EnemyAnimationHandler>();
		attribute = GetComponent<EnemyAttributeHandler>();
		locomotion = GetComponent<EnemyLocomotionHandler>();
		combat = GetComponent<EnemyCombatHandler>();
		utility = GetComponent<EnemyUtilityHandler>();


		navAgent.updatePosition = false;
		navAgent.updateRotation = false;
		
		// SET START POSITION
		// TODO: Remake this method with save data
		NavMesh.SamplePosition(transform.position, out NavMeshHit placedPositionOnSurface, Mathf.Infinity, NavMesh.AllAreas);
		worldPlacedPosition = placedPositionOnSurface.position;
		transform.position = worldPlacedPosition;
		worldPlacedRotation = transform.rotation;
		
		Reload();
	} 
	
	
	protected override void Update() {
		
		base.Update();
		state.HandleState();

	}

	private void LateUpdate() {
		locomotion.ResetMoveDirectionParameter();
		navAgent.nextPosition = transform.position;
	}

	public override void OnDamaged(TakeHealthDamage damage)
	{
		UserInfoManager.Instance.currentUser.damageDealt += damage.damage.AllDamage;
	}

	protected override void OnDeath() {
		base.OnDeath();
	}
	
	
	
	// SETUP START STATE AND RUNTIME ATTRIBUTE SETTING
	public void Reload() {
		
		transform.position = worldPlacedPosition;
		transform.position = worldPlacedPosition;
		
		curHp = attribute.MaxHp;
		
	}
	
}

}