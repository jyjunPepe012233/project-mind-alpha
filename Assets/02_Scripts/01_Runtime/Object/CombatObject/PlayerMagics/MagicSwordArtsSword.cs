using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MinD.Runtime.Entity;
using MinD.Runtime.Object.Magics;
using MinD.Runtime.Utils;
using MinD.SO.Object;
using MinD.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MagicSwordArtsSword : MonoBehaviour
{
    [FormerlySerializedAs("[ cargeLevel1_Effect ]")]
    [Header("[ Charge Effects ]")]
    [Space(5)]
    [SerializeField] private GameObject chargeLevel0_Effect;
    [SerializeField] private GameObject chargeLevel1_Effect;
    [SerializeField] private GameObject chargeLevel2_Effect;
    [SerializeField] private GameObject chargeLevel3_Effect;
    [SerializeField] private GameObject slash_Effect;
    [Space(20)]
    
    private ParticleSystem _chargeLevel0_ParticleSystem;
    private ParticleSystem _chargeLevel1_ParticleSystem;
    private ParticleSystem _chargeLevel2_ParticleSystem;
    private ParticleSystem _chargeLevel3_ParticleSystem;
    private ParticleSystem _slash_ParticleSystem;
    
    [SerializeField] public DamageCollider _damageCollider;
    [SerializeField] private CapsuleCollider _collider;
    
    
    [HideInInspector] public Player _castPlayer;
    
    
    /* Todo :: 콜라이더 사이즈 문제있음 */
    
    private void Awake()
    {
        _chargeLevel0_ParticleSystem = chargeLevel0_Effect.GetComponent<ParticleSystem>();
        _chargeLevel1_ParticleSystem = chargeLevel1_Effect.GetComponent<ParticleSystem>();
        _chargeLevel2_ParticleSystem = chargeLevel2_Effect.GetComponent<ParticleSystem>();
        _chargeLevel3_ParticleSystem = chargeLevel3_Effect.GetComponent<ParticleSystem>();
        _slash_ParticleSystem = slash_Effect.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_castPlayer != null)
        {
            transform.position = _castPlayer.equipment.rightHand.position;
            transform.rotation = _castPlayer.equipment.rightHand.rotation;
        }
    }

    public void Slash_setParticle()
    {
        slash_Effect.SetActive(true);
        _slash_ParticleSystem?.Play(true);
    }
    
    public void ChargeLevel0_SetParticle(Player __castPlayer)
    {
        _castPlayer = __castPlayer;
        PhysicUtility.IgnoreCollisionUtil(_castPlayer, _collider);

        chargeLevel0_Effect.SetActive(true);
        _chargeLevel0_ParticleSystem?.Play(true);
    }
    
    public void ChargeLevel1_SetParticle()
    {
        Debug.Log("ChargeLevel1_SetParticle");
        chargeLevel1_Effect.SetActive(true);
        _chargeLevel1_ParticleSystem?.Play(true);
    }
    
    public void ChargeLevel2_SetParticle()
    {
        Debug.Log("ChargeLevel2_SetParticle");
        chargeLevel2_Effect.SetActive(true);
        _chargeLevel2_ParticleSystem?.Play(true);
    }
    
    public void ChargeLevel3_SetParticle()
    {
        Debug.Log("ChargeLevel3_SetParticle");
        chargeLevel3_Effect.SetActive(true);
        _chargeLevel3_ParticleSystem?.Play(true);
     }

    public void SetScale(Vector3 __targetScale)
    {
        StartCoroutine(SetScaleCoroutine(__targetScale));
    }
    
    private IEnumerator SetScaleCoroutine(Vector3 __targetSize)
    {
        float __elapsedTime = 0;
        while (__elapsedTime < 0.3f)
        {
            __elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp( transform.localScale, __targetSize ,0.25f);
            _collider.height = __targetSize.x * 1.8f;
            _collider.radius = __targetSize.y / 5;

            yield return null;
        }
    }
    
    public void MagicSword_Slash(DamageData __damagedata)
    {
        _chargeLevel0_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _chargeLevel1_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _chargeLevel2_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _chargeLevel3_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        
        _damageCollider.soData = __damagedata;
    }

    public void Explode()
    {
        Destroy(gameObject);
    }
    
    private IEnumerator ExplodeCoroutine()
    {
        yield break;
    }
    
}
