using System;
using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.Serialization;

public class MagicSwordArtsTrajectory : MonoBehaviour
{
    [SerializeField] private GameObject chargeLevel0_Slash_Effect;
    [SerializeField] private GameObject chargeLevel1_Slash_Effect;
    [SerializeField] private GameObject chargeLevel2_Slash_Effect;
    [SerializeField] private GameObject chargeLevel3_Slash_Effect;
    [SerializeField] private GameObject comboStep1_Slash_Effect;
    [SerializeField] private GameObject comboStep2_Slash_Effect;

    private ParticleSystem _chargeLevel0_Slash_ParticleSystem;
    private ParticleSystem _chargeLevel1_Slash_ParticleSystem;
    private ParticleSystem _chargeLevel2_Slash_ParticleSystem;
    private ParticleSystem _chargeLevel3_Slash_ParticleSystem;
    private ParticleSystem _comboStep1_Slash_ParticleSystem;
    private ParticleSystem _comboStep2_Slash_ParticleSystem;
    
    private void Awake()
    {
        _chargeLevel0_Slash_ParticleSystem = chargeLevel0_Slash_Effect.GetComponent<ParticleSystem>();
        _chargeLevel1_Slash_ParticleSystem = chargeLevel1_Slash_Effect.GetComponent<ParticleSystem>();
        _chargeLevel2_Slash_ParticleSystem = chargeLevel2_Slash_Effect.GetComponent<ParticleSystem>();
        _chargeLevel3_Slash_ParticleSystem = chargeLevel3_Slash_Effect.GetComponent<ParticleSystem>();
        
        _comboStep1_Slash_ParticleSystem = comboStep1_Slash_Effect.GetComponent<ParticleSystem>();
        _comboStep2_Slash_ParticleSystem = comboStep2_Slash_Effect.GetComponent<ParticleSystem>();
        Debug.Log("Compleate prajectory Awake");
    }
    
    public void ChargeLevel0_Slash() 
    {
        Debug.Log("ChargeLevel1_Slash");
        _chargeLevel0_Slash_ParticleSystem = chargeLevel1_Slash_Effect.GetComponent<ParticleSystem>();
        chargeLevel0_Slash_Effect.SetActive(true);
        _chargeLevel0_Slash_ParticleSystem.Play(true);
    }
    public void ChargeLevel1_Slash() 
    {
        Debug.Log("ChargeLevel1_Slash");
        _chargeLevel1_Slash_ParticleSystem = chargeLevel1_Slash_Effect.GetComponent<ParticleSystem>();
        chargeLevel1_Slash_Effect.SetActive(true);
        _chargeLevel1_Slash_ParticleSystem.Play(true);
    }
    public void ChargeLevel2_Slash()
    {
        Debug.Log("ChargeLevel2_Slash");
        _chargeLevel2_Slash_ParticleSystem = chargeLevel2_Slash_Effect.GetComponent<ParticleSystem>();
        chargeLevel2_Slash_Effect.SetActive(true);
        _chargeLevel2_Slash_ParticleSystem.Play(true);
    }
    
    public void ChargeLevel3_Slash() // 파티클 추가
    {
        Debug.Log("ChargeLevel3_Slash");
        _chargeLevel3_Slash_ParticleSystem = chargeLevel3_Slash_Effect.GetComponent<ParticleSystem>();
        chargeLevel3_Slash_Effect.SetActive(true);
        _chargeLevel3_Slash_ParticleSystem.Play(true);
    }

    public void ComboStep1_Slash()
    {
        Debug.Log("ComboStep1_Slash");
        //
        // _comboStep1_Slash_ParticleSystem = comboStep1_Slash_Effect.GetComponent<ParticleSystem>();
        // comboStep1_Slash_Effect.SetActive(true);
        // _comboStep1_Slash_ParticleSystem.Play(true);
    }
    public void ComboStep2_Slash()
    {
        Debug.Log("ComboStep2_Slash");
        
        _comboStep2_Slash_ParticleSystem = comboStep2_Slash_Effect.GetComponent<ParticleSystem>();
        comboStep2_Slash_Effect.SetActive(true);
        _comboStep2_Slash_ParticleSystem.Play(true);
    }

}
