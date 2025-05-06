using System;
using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.Serialization;

public class MagicSwordArtsTrajectory : MonoBehaviour
{
    [SerializeField] private GameObject chargeLevel1_Slash_Effect;
    [SerializeField] private GameObject chargeLevel2_Slash_Effect;
    [SerializeField] private GameObject chargeLevel3_Slash_Effect;

    [SerializeField] private GameObject chargeLevel0_Charging_Effect;
    [SerializeField] private GameObject chargeLevel1_Charging_Effect;
    [SerializeField] private GameObject chargeLevel2_Charging_Effect;
    [SerializeField] private GameObject chargeLevel3_Charging_Effect;

    private ParticleSystem _chargeLevel1_Slash_ParticleSystem;
    private ParticleSystem _chargeLevel2_Slash_ParticleSystem;
    private ParticleSystem _chargeLevel3_Slash_ParticleSystem;

    private ParticleSystem _chargeLevel0_Charging_ParticleSystem;
    private ParticleSystem _chargeLevel1_Charging_ParticleSystem; // Loop 빼고 파티클 생성헤서 시작 파티클 구현하기
    private ParticleSystem _chargeLevel2_Charging_ParticleSystem;
    private ParticleSystem _chargeLevel3_Charging_ParticleSystem;

    [Space(10)]
    
    [SerializeField] private MagicSwordArtsSword _magicSwordArtsSword;

    // private Player _castPlayer;
    
    private void Awake()
    {
        _chargeLevel1_Slash_ParticleSystem = chargeLevel1_Slash_Effect.GetComponent<ParticleSystem>();
        _chargeLevel2_Slash_ParticleSystem = chargeLevel2_Slash_Effect.GetComponent<ParticleSystem>();
        _chargeLevel3_Slash_ParticleSystem = chargeLevel3_Slash_Effect.GetComponent<ParticleSystem>();

        _chargeLevel0_Charging_ParticleSystem = chargeLevel0_Charging_Effect.GetComponent<ParticleSystem>();
        _chargeLevel1_Charging_ParticleSystem = chargeLevel1_Charging_Effect.GetComponent<ParticleSystem>();
        _chargeLevel2_Charging_ParticleSystem = chargeLevel2_Charging_Effect.GetComponent<ParticleSystem>();
        _chargeLevel3_Charging_ParticleSystem = chargeLevel3_Charging_Effect.GetComponent<ParticleSystem>();
        
    }

    public void ChargeLevel0_SetEffect(Player __castPlayer) // 모이는 이펙펙
    {
        // _castPlayer = __castPlayer;
        // _chargeLevel0_Charging_ParticleSystem.Play(true);
        // GameObject  = _castPlayer.equipment.rightHand.position;

    }
    
    public void ChargeLevel1_SetEffect() 
    {
        _chargeLevel1_Charging_ParticleSystem.Play(true); // 기본 파티클
        _magicSwordArtsSword.ChargeLevel1_SetParticle();
    }
    public void ChargeLevel2_SetEffect()
    {
        _chargeLevel2_Charging_ParticleSystem.Play(true); // 파티클 추가
        _magicSwordArtsSword.ChargeLevel2_SetParticle();
    }
    
    public void ChargeLevel3_SetEffect() // 파티클 추가
    {
        _chargeLevel3_Charging_ParticleSystem.Play(true);
        _magicSwordArtsSword.ChargeLevel3_SetParticle();
    }

    public void MagicSwordSlash(int __chargeLavel) // 다 지우고 공격파티클로 전환
    {
        _chargeLevel0_Charging_ParticleSystem.Stop(true);
        _chargeLevel1_Charging_ParticleSystem.Stop(true);
        _chargeLevel2_Charging_ParticleSystem.Stop(true);
        _chargeLevel3_Charging_ParticleSystem.Stop(true);
        
        // _magicSwordArtsSword.MagicSword_Slash(__chargeLavel);
    }
    
    public void OnDestroy() // 그냥 사라짐 (카리아의 대검 참고) 
    {
        _magicSwordArtsSword._gameObject.SetActive(false);
    }
    
}
