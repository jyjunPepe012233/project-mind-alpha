using System;
using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Object.Magics;
using MinD.SO.Item;
using MinD.SO.Object;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Serialization;
using Task = System.Threading.Tasks.Task;

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Magic Sword Art")]

public class MagicSwordArts : Magic
{
    
    [Header("[ MagicSwordArts ]")]
    [SerializeField] private MagicSwordArtsSword _magicSwordArtsSword;
    [SerializeField] private GameObject _magicSwordArtsSwordObj;
    [FormerlySerializedAs("_magicSwordArtsTrajectory")]
    [SerializeField] private GameObject _magicSwordArtsTrajectoryGameObject;
    private MagicSwordArtsTrajectory _magicSwordArtsTrajectory;
    
    [Header("[ Set State ]")]
    [Space(5)]
    [SerializeField] private DamageData[] _damageDatas = new DamageData[4]; // 0: 차징X 부터, 차징 단계에 따른 것
    [SerializeField] private Vector3[] _chargeToTargetScale = new Vector3[3];

    [Header("[ Combo Attack ]")] [Space(5)]
    [SerializeField] private GameObject[] _magicSword;
    [SerializeField] private MagicSwordProjectile[] _magicSwordProjectile;
    
    private bool _doAttack;
    private bool _doComboStandby;
    private bool _comboAttack;

    private bool _doCharging;
    private bool _ReadyAttack = false;

    private GameObject _sword;
    private MagicSwordArtsSword _swordOfMagicSword;
    
    private float _chargeElapsedTime = 0;
    private int _chargeLevel = 0;

    private void Awake()
    {
        _magicSwordArtsTrajectory = _magicSwordArtsTrajectoryGameObject.GetComponent<MagicSwordArtsTrajectory>();
    }

    public override void OnUse()
    {
        _chargeElapsedTime = 0;
        Debug.Log("MagicSword.OnUse : castPlayer is " + ((castPlayer == null)?"null":"not null"));
        
        _doAttack = false;
        Debug.Log("Use Magic");
        
        if (!castPlayer.isPerformingAction && !_doComboStandby){
            castPlayer.animation.PlayTargetAction("MagicSwrodArt_Charge_1",true, true, false, false); 
            
            _sword = Instantiate(_magicSwordArtsSwordObj);
            _swordOfMagicSword = _sword.GetComponent<MagicSwordArtsSword>();
            _swordOfMagicSword.ChargeLevel0_SetParticle(castPlayer);
            
            // ChargingStep0(); 
            _comboAttack = false;
            }
        else if(!castPlayer.isPerformingAction && _doComboStandby)
        {
            Debug.Log("UseMagic -> Combo");
            _doAttack = true;
            _comboAttack = true;
            _doCharging = false;
            _ReadyAttack = false;
            castPlayer.animation.PlayTargetAction("MagicSwrodArt_Combo",true, true, false, false);
        }
    }

    public override void Tick()
    {
        Debug.Log("isInputReleased : " + isInputReleased);
        
        if (!castPlayer.isPerformingAction)
                {
                    Debug.Log("end Magic");
                    castPlayer.combat.ExitCurrentMagic();
                    return;
                }
        
        if (isInputReleased && !_doAttack &&  !_comboAttack  && _ReadyAttack)
        {
            Debug.Log("비 차징 공격");
            
            ChargeCompleate();
        }

        if (_doCharging)
        {
            _chargeElapsedTime += Time.deltaTime;
            
            // 0.6s ~ 2.2s
            switch (_chargeElapsedTime)
            {
                case < 0f:
                    if (_chargeLevel == 0)
                    {
                        Debug.Log("ChargingStep1");
                        // ChargingStep1();
                    }
                    _chargeLevel = 1;
                    break;
                case < 0.8f:
                    if (_chargeLevel == 1)
                    {
                        Debug.Log("ChargingStep2");
                        // ChargingStep2();
                    }
                    _chargeLevel = 2;
                    break;
                case < 1.6f:
                    if (_chargeLevel == 2)
                    {
                        Debug.Log("ChargingStep3");
                        // ChargingStep3();
                    }
                    _chargeLevel = 3;
                    break;
            }
        }
    }

    public override void OnReleaseInput()
    {
    }

    public override void OnCancel()
    {
        castPlayer.combat.ExitCurrentMagic();
    }

    public override void OnExit()
    {
        _magicSwordArtsTrajectory?.OnDestroy();
        if (!_comboAttack)
        {
            StandbyAdditionalUse();
        }
        
    }
    
    
    public override void OnSuccessfullyCast()
    {
        Debug.Log("OnSuccessfullyCast");

        if (_comboAttack)
        {
            ComboFullyCast();
        }
        else if (!_comboAttack)
        {
            _doCharging = true;
            _ReadyAttack = true;
        }
        
    }

    public void ComboFullyCast()
    {
        Debug.Log("Combo FullyCast");
    }
    
    
    
    async void StandbyAdditionalUse()
    {
        _doComboStandby = true ;
        Debug.Log("StadbyCombo = true");
        
        await Task.Delay(600);
        
        Debug.Log("StandbyCombo = false");
        _doComboStandby = false;
    }
    

    

    public void ChargeCompleate()
    {
        _doCharging = false;
        _ReadyAttack = false;
        
        Debug.Log("_doStadbyCombo :" + _doComboStandby);
                
                _doAttack = true;
                
                Debug.Log("공격 실행");
                castPlayer.animation.PlayTargetAction("MagicSwrodArt_Attack", true, true, false, false);
                
                _magicSwordArtsSword.MagicSword_Slash(_damageDatas[_chargeLevel]);
    }
    
    public void ChargingStep0()
    {
        _chargeLevel = 0;
        _magicSwordArtsTrajectory.ChargeLevel0_SetEffect(castPlayer);
        // _magicSwordArtsSword.SetScale(_chargeToTargetScale[0]);
    }

    public void ChargingStep1()
    {
        _chargeLevel = 1;
        _magicSwordArtsTrajectory.ChargeLevel1_SetEffect();
        _magicSwordArtsSword.SetScale(_chargeToTargetScale[0]);
    }

    public void ChargingStep2()
    {
        _chargeLevel = 2;
        _magicSwordArtsTrajectory.ChargeLevel2_SetEffect();
        _magicSwordArtsSword.SetScale(_chargeToTargetScale[1]);
    }

    public void ChargingStep3()
    {
        _chargeLevel = 3;
        _magicSwordArtsTrajectory.ChargeLevel3_SetEffect();
        _magicSwordArtsSword.SetScale(_chargeToTargetScale[2]);
    }
    
}
