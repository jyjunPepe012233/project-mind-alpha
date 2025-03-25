using System;
using System.Collections.Generic;
using MinD.Utility;
using MinD.SO.StatusFX.Effects;
using UnityEngine;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(EntityStatusFxHandler))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public abstract class BaseEntity : MonoBehaviour {

	[HideInInspector] public CharacterController cc;
	[HideInInspector] public Animator animator;

	[HideInInspector] public EntityStatusFxHandler statusFx;
	protected bool hasBeenSetup;
	
	
	public bool isDeath;
	public bool isInvincible;
	public bool immunePoiseBreak;
	public List<Transform> targetOptions;
	[Space(5)]
	[SerializeField] protected int curHp;
	public abstract int CurHp { get; set; }

	
	public Action getHitAction = new Action(() => {});
	public Action dieAction = new Action(() => {});


	private void Awake() {
		Setup();
	}

	protected virtual void Setup() {

		PhysicUtility.SetUpIgnoreBodyCollision(this);
		
		cc = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		statusFx = GetComponent<EntityStatusFxHandler>();


	}
	protected virtual void Update() {
		
		statusFx.HandleAllEffect();
		
	}

	public abstract void OnDamaged(TakeHealthDamage damage);

	protected abstract void OnDeath();
}

}