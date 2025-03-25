using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace MinD.Structs {

[Serializable]
public struct Damage {

	public int physical;
	public int magic;
	public int fire;
	public int frost;
	public int lightning;
	public int holy;

	public Damage(int physical, int magic, int fire, int frost, int lightning, int holy) {
		this.physical = physical;
		this.magic = magic;
		this.fire = fire;
		this.frost = frost;
		this.lightning = lightning;
		this.holy = holy;
	}

	public int AllDamage {
		get => physical + magic + fire + frost + lightning + holy;
		set {
			physical = value;
			magic = value;
			fire = value;
			frost = value;
			lightning = value;
			holy = value;
		}
	}
}

[Serializable]
public struct DamageNegation {

	// 0~1
	public float physical {
		get => GetCalculatedNegation(0);
		set {
			_physical = value;
		}
	}
	public float magic {
		get => GetCalculatedNegation(1);
		set {
			_magic = value;
		}
	}
	public float fire {
		get => GetCalculatedNegation(2);
		set {
			_fire = value;
		}
	}
	public float frost {
		get => GetCalculatedNegation(3);
		set {
			_frost = value;
		}
	}
	public float lightning {
		get => GetCalculatedNegation(4);
		set {
			_lightning = value;
		}
	}
	public float holy {
		get => GetCalculatedNegation(5);
		set {
			_holy = value;
		}
	}

	// BASE NEGATION VALUE OF THIS STRUCT
	[SerializeField, Range(-1, 1)] private float _physical, _magic, _fire, _frost, _lightning, _holy;

	
	private List<DamageNegation> multiplyingNegations;
	
	
	
	/// <summary>
	/// Return calculated damage value by negation type parameter
	/// </summary>
	/// <param name="negationType"></param>
	/// <returns></returns>
	private float GetCalculatedNegation(int negationType) {

		if (multiplyingNegations == null) {
			multiplyingNegations = new List<DamageNegation>();
		}
		
		

		float finalNegation = 0;
		
		switch (negationType) {
			
			case 0:
				finalNegation = _physical;
				for (int i = 0; i < multiplyingNegations.Count; i++) {
					finalNegation += (1 - finalNegation) * multiplyingNegations[i].physical;
				}
				break;
			
			case 1:
				finalNegation = _magic;
				for (int i = 0; i < multiplyingNegations.Count; i++) {
					finalNegation += (1 - finalNegation) * multiplyingNegations[i].magic;
				}
				break;
			
			case 2:
				finalNegation = _fire;
				for (int i = 0; i < multiplyingNegations.Count; i++) {
					finalNegation += (1 - finalNegation) * multiplyingNegations[i].fire;
				}
				break;
			
			case 3:
				finalNegation = _frost;
				for (int i = 0; i < multiplyingNegations.Count; i++) {
					finalNegation += (1 - finalNegation) * multiplyingNegations[i].frost;
				}
				break;
			
			case 4:
				finalNegation = _lightning;
				for (int i = 0; i < multiplyingNegations.Count; i++) {
					finalNegation += (1 - finalNegation) * multiplyingNegations[i].lightning;
				}
				break;
			
			case 5:
				finalNegation = _holy;
				for (int i = 0; i < multiplyingNegations.Count; i++) {
					finalNegation += (1 - finalNegation) * multiplyingNegations[i].holy;
				}
				break;
		}

		return finalNegation;

	}

	
	
	public static DamageNegation operator *(DamageNegation a, DamageNegation b) {

		if (a.multiplyingNegations == null) {
			a.multiplyingNegations = new List<DamageNegation>();
		}
		
		a.multiplyingNegations.Add(b);
		
		return a;
	}
	
	public static DamageNegation operator /(DamageNegation a, DamageNegation b) {
		
		if (a.multiplyingNegations == null) {
			a.multiplyingNegations = new List<DamageNegation>();
		}
		
		// FIND AND REMOVE ONCE OF EQUAL STRUCT
		for (int i = 0; i < a.multiplyingNegations.Count; i++) {
			
			if (a.multiplyingNegations[i].Equals(b)) {
				a.multiplyingNegations.RemoveAt(i);
				return a;
			}
		}
		
		Debug.Log("!! DAMAGE NEGATION OPERATOR CAN'T OPERATE!");
		return a;
	}
	
}

}