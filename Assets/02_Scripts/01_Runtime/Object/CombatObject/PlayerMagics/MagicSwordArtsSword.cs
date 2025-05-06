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
    [Space(20)]
    
    private ParticleSystem _chargeLevel0_ParticleSystem;
    private ParticleSystem _chargeLevel1_ParticleSystem;
    private ParticleSystem _chargeLevel2_ParticleSystem;
    private ParticleSystem _chargeLevel3_ParticleSystem;
    
    [SerializeField] public  GameObject _gameObject;
    [SerializeField] private DamageCollider _damageCollider;
    [SerializeField] private Collider _collider;
    
    // [Header("[ Set State ]")]
    // [Space(5)]
    // [SerializeField] private DamageData[] _damageDatas = new DamageData[4]; // 0: 차징X 부터, 차징 단계에 따른 것
    // [SerializeField] private Vector3[] _chargeToTargetScale = new Vector3[3];
    
    [HideInInspector] public Player _castPlayer;
    
    // private GameObject _swordObj;
    // private MagicSwordArtsSword _swordObjofSwordClass;
    
    // [HideInInspector] public bool _isHandler = true;
    
    private void Awake()
    {
        _damageCollider.enabled = false;
        // PhysicUtility.IgnoreCollisionUtil(_castPlayer, _collider);
        
        // _chargeLevel1_ParticleSystem = chargeLevel1_Effect?.GetComponent<ParticleSystem>();
        // _chargeLevel2_ParticleSystem = chargeLevel2_Effect?.GetComponent<ParticleSystem>();
        // _chargeLevel3_ParticleSystem = chargeLevel3_Effect?.GetComponent<ParticleSystem>();
        
        // _chargeLevel0_ParticleSystem = chargeLevel0_Effect.GetComponent<ParticleSystem>();

        // _chargeLevel0_ParticleSystem?.Play(true);

        // _sword = GetComponent<GameObject>();

    }

    private void Update()
    {
        // if (_isHandler)
        // {
        //     Debug.Log((_swordObj == null)? "_swordObj is null":"_swordObj is not null");
        //     Debug.Log((_swordObj.transform == null)? "_swordObj.tranfrom is null":"_swordObj.trasform is not null");
        //     
        //     Debug.Log("_sword.isHandler : " + _swordObjofSwordClass._isHandler);
        //     Debug.Log(_swordObj.transform.position);
        //     
        //     _swordObj.transform.position = _castPlayer.equipment.rightHand.position;
        //     
        // }
        if (_castPlayer != null)
        {
            transform.position = _castPlayer.equipment.rightHand.position;
            transform.rotation = _castPlayer.equipment.rightHand.rotation;
        }
        
    }


    public void ChargeLevel0_SetParticle(Player __castPlayer)
    {
        _castPlayer = __castPlayer;
        PhysicUtility.IgnoreCollisionUtil(_castPlayer, _collider);
        // _chargeLevel0_ParticleSystem?.Play(true);
        
        // Debug.Log("MagicSword Create");
        //
        // _castPlayer = __castPlayer;
        //
        // Debug.Log("MagicSwordCreater : __castPlayer is " + ((__castPlayer == null)?"null":"not null"));
        // Debug.Log("MagicSwordCreater : _castPlayer is "  + (( _castPlayer == null)?"null":"not null"));
        //
        // _swordObj = Instantiate(_gameObject);
        // _swordObjofSwordClass = _swordObj.GetComponent<MagicSwordArtsSword>();
        //
        // _swordObjofSwordClass._isHandler = false;
        //
        // Debug.Log("");
        // _swordObj.SetActive(true);
    }
    
    public void ChargeLevel1_SetParticle()
    {
        // 파티클 생성 범위, 수치 조정 및 검 크기 조정
        _chargeLevel1_ParticleSystem?.Play(true);
        // StartCoroutine(SetScale(_chargeToTargetScale[0]));
    }
    
    public void ChargeLevel2_SetParticle()
    {
        // 파티클 생성 범위, 수치 조정 및 검 크기 조정
        _chargeLevel2_ParticleSystem?.Play(true);
        // StartCoroutine(SetScale(_chargeToTargetScale[1]));
    }
    
    public void ChargeLevel3_SetParticle()
    {
        // 파티클 생성 범위, 수치 조정 및 검 크기 조정
        _chargeLevel3_ParticleSystem?.Play(true);
        // StartCoroutine(SetScale(_chargeToTargetScale[2]));
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

            yield return null;
        }
    }
    
    public void MagicSword_Slash(DamageData __damagedata)
    {
        _damageCollider.enabled = true;

        _damageCollider.soData = __damagedata;
    }
    
    // private IEnumerator Explode()
    // {
    //     
    // }
    
}
