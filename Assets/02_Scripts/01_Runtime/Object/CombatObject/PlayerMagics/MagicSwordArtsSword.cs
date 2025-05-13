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
    [SerializeField] private GameObject explode_Effect;
    [Space(20)]
    
    private ParticleSystem _chargeLevel0_ParticleSystem;
    private ParticleSystem _chargeLevel1_ParticleSystem;
    private ParticleSystem _chargeLevel2_ParticleSystem;
    private ParticleSystem _chargeLevel3_ParticleSystem;
    private ParticleSystem _slash_ParticleSystem;
    private ParticleSystem _explode_ParticleSystem;

    [SerializeField] private GameObject _sword; //  이거 파티클임
    
    [HideInInspector] public Player _castPlayer;
    
    
    private void Awake()
    {
        _chargeLevel0_ParticleSystem = chargeLevel0_Effect.GetComponent<ParticleSystem>();
        _chargeLevel1_ParticleSystem = chargeLevel1_Effect.GetComponent<ParticleSystem>();
        _chargeLevel2_ParticleSystem = chargeLevel2_Effect.GetComponent<ParticleSystem>();
        _chargeLevel3_ParticleSystem = chargeLevel3_Effect.GetComponent<ParticleSystem>();
        _slash_ParticleSystem   = slash_Effect.GetComponent<ParticleSystem>();
        _explode_ParticleSystem = explode_Effect.GetComponent<ParticleSystem>();
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
        Debug.Log("Slash_setParticle");
        slash_Effect.SetActive(true);
        _slash_ParticleSystem?.Play(true);
    }

    public void ComboAttack_Set(Player __castPlayer)
    {
        _castPlayer = __castPlayer;
        Debug.Log("ComboAttack_Set");
        Slash_setParticle();
        
    }

    public void ChargeAttack_Set(Player __castPlayer)
    {
        _castPlayer = __castPlayer;
        Debug.Log("ChargeAttack_Set");
    }
    
    public void ChargeLevel0_SetParticle(Player __castPlayer)
    {
        _castPlayer = __castPlayer;
        
        Debug.Log("ChargeLevel0_SetParticle");

        chargeLevel0_Effect.SetActive(true);
        _chargeLevel0_ParticleSystem?.Play(true);

    }
    
    
    public void MagicSword_Slash()
    {
        _chargeLevel0_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _chargeLevel1_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _chargeLevel2_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _chargeLevel3_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        
        Slash_setParticle();
    }

    public void Explode()
    {
        
        gameObject.SetActive(false);
        Destroy(gameObject, 3);
    }
    
    private IEnumerator ExplodeCoroutine()
    {
        yield break;
    }
    
}
