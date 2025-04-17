using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.Runtime.Utils;
using UnityEngine;

public class DreadDwarfMinerQuake : MonoBehaviour
{
	[SerializeField] private ParticleSystem _waitingVfx;
	[SerializeField] private ParticleSystem _quakeVfx;
	[SerializeField] private float _quakeTiming = 1;
	[SerializeField] private float _damageColliderLifeTime = 0.2f;
	[SerializeField] private float _wholeLifeTime = 5;
	[SerializeField] private DamageCollider damageCollider;

	public void SetOwner(BaseEntity owner)
	{
		damageCollider.blackList.Add(owner);
	}
	
	public void OnEnable()
	{
		damageCollider.gameObject.SetActive(false);

		StartCoroutine(StartQuake());
	}

	private IEnumerator StartQuake()
	{
		_waitingVfx.Play();
		yield return new WaitForSeconds(_quakeTiming);
		_quakeVfx.Play();
		damageCollider.gameObject.SetActive(true);
		yield return new WaitForSeconds(_damageColliderLifeTime);
		damageCollider.gameObject.SetActive(false);
		yield return new WaitForSeconds(Mathf.Max(_wholeLifeTime - _damageColliderLifeTime - _quakeTiming, 0));
		Destroy(this);
	}
}
