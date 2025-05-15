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
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class MagicSwordArtsSword : MonoBehaviour
{
    [FormerlySerializedAs("[ cargeLevel1_Effect ]")]
    [Header("[ Charge Effects ]")]
    [Space(5)]
    [SerializeField] private GameObject slash_Effect;
    [SerializeField] private GameObject explode_Effect;
    [SerializeField] private GameObject magicSword; //  이거 본체
    [SerializeField] private GameObject defult_Effect;
    [Space(20)]
    
    private ParticleSystem _slash_ParticleSystem;
    private ParticleSystem _explode_ParticleSystem;
    private ParticleSystem _magicSword_ParticleSystem;
    private ParticleSystem _deafult_ParticleSystem;
    
    [HideInInspector] public Player _castPlayer;
    
    private void Awake()
    {
        _slash_ParticleSystem   = slash_Effect.GetComponent<ParticleSystem>();
        _explode_ParticleSystem = explode_Effect.GetComponent<ParticleSystem>();
        _magicSword_ParticleSystem = magicSword.GetComponent<ParticleSystem>();
        _deafult_ParticleSystem = defult_Effect.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_castPlayer != null)
        {
            transform.position = _castPlayer.equipment.rightHand.position;
            transform.rotation = _castPlayer.equipment.rightHand.rotation;
        }
    }

    private void OnEnable()
    {
        _deafult_ParticleSystem.Play(true);
    }
    
    public void Attack_SetParticle(Player __castPlayer)
    {
        _castPlayer = __castPlayer;
        
        slash_Effect.SetActive(true);
        _slash_ParticleSystem.Play(true);
    }

    public void Attack_AndParticle()
    {
        // _slash_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        slash_Effect.SetActive(false);
    }


    public void Explode() // 공격 끝날때 
    {
        explode_Effect.SetActive(true);
        _explode_ParticleSystem.Play(true);
        
        _magicSword_ParticleSystem.Stop(false);
        _deafult_ParticleSystem.Stop(true);
        
        Destroy(gameObject,3);
    }
    
    
}
