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
    [Space(5)]
    [SerializeField] private MagicSwordArtsSword _magicSwordArtsSword;
    [SerializeField] private GameObject _magicSwordArtsSwordObj;
    [SerializeField] private GameObject _magicSwordArtsTrajectoryObj;
    
    private MagicSwordArtsTrajectory _magicSwordArtsTrajectory;
    
    [Header("[ Set State ]")]
    [Space(5)]
    [SerializeField] private DamageData[] _damageDatas = new DamageData[3]; // 0: 차징X 부터, 차징 단계에 따른 것
    [SerializeField] private Vector3[] _chargeToTargetScale = new Vector3[3];

    
    [FormerlySerializedAs("magicSword")]
    [Header("[ Combo Attack ]")]
    [Space(5)]
    [SerializeField] private GameObject[] _magicSword;
    [FormerlySerializedAs("magicSwordProjectile")] [SerializeField] private MagicSwordProjectile[] _magicSwordProjectile;
    
    
    private bool _doAttack;             // 공격 하고 있는가.
    private bool _doComboStandby;       // 공격 후 콤보 공격 사용을 대기 중인가
    private bool _comboAttack;          // 현재 공격이 콤보 어택인가
    private bool _doCharging;           // 현재 차지 중인가
    private bool _ReadyAttack = false;  // 공격 준비가 완료 되었는가

    // 생성된 마법검 관리 및 저장 변수
    private GameObject _sword;
    private MagicSwordArtsSword _swordOfMagicSword;  
    
    // 현재 차지 레벨과 차지를 진행한 시간
    private float _chargeElapsedTime = 0;
    private int _chargeLevel = 0;
    
    /* ToDo
     0. 열심히 하기..?
     1. 파티클 크기 연동 안됨 - 차징단계에 따른 크기변화
     2. 차징 고치기, 차징 안됨
     */
    
    private void Awake()
    {
        _magicSwordArtsTrajectory = _magicSwordArtsTrajectoryObj.GetComponent<MagicSwordArtsTrajectory>();
    }

    public override void OnUse()
    {
        _doAttack    = false;         
        _doCharging  = false;
        _comboAttack = false;            
        _ReadyAttack = false;  
        
        if (!castPlayer.isPerformingAction && !_doComboStandby) // 콤보 공격이 아님
        {
            PlayDefultAttack();
        }
        else if(!castPlayer.isPerformingAction && _doComboStandby) // 콤보 공격임
        {
            PlayChargeAttack();
        }
    }
    private void PlayDefultAttack()
    {
        _chargeElapsedTime = 0;
        
        _doAttack    = true;
        _comboAttack = false;
        
        castPlayer.animation.PlayTargetAction("MagicSwrodArt_Charge_1",true, true, false, false); 
                    
        _sword = Instantiate(_magicSwordArtsSwordObj);
        _swordOfMagicSword = _sword.GetComponent<MagicSwordArtsSword>();
        ChargingStep0();
        
        // ChargingStep0(); 
        _comboAttack = false;
    }
    private void PlayChargeAttack()
    {
        _doAttack    = true;
        _comboAttack = true;
        
        castPlayer.animation.PlayTargetAction("MagicSwrodArt_Combo", true, true, false, false);
    }
    
    
    public override void Tick()
    {
        // 중간에 차징 끓고 공격하는 조건문 - 문제가 심각함
                if (isInputReleased && !_doAttack && !_comboAttack && _ReadyAttack)
                {
                    Debug.Log("not full Charge Attack");
                    ChargeCompleate();
                }
        
        // 애니메이션 종료시 끝내기
        if (!castPlayer.isPerformingAction) 
        {
            Debug.Log("Magic And");
            castPlayer.combat.ExitCurrentMagic();
        }
        
        Debug.Log("isInputReleased : " + isInputReleased);
        
        // 차징 시간에 따른 차징 단계 - 볼 거 있음
        if (_doCharging)
        {
            _chargeElapsedTime += Time.deltaTime;
            
            // 0.6s ~ 2.2s
            // 파티클 활성화 주석 상태임
            // 차징 시간에 따른 차징 단계의 변화
            switch (_chargeElapsedTime)
            {
                case > 1.6f:
                    Debug.Log("Charge Compleate");
                    ChargeCompleate();
                    break;
                
                case > 1.4f:
                    if (_chargeLevel == 2)
                    {
                        Debug.Log("ChargingStep3");
                        ChargingStep3();
                    }
                    _chargeLevel = 3;
                    break;
                    
                case > 0.7f:
                    if (_chargeLevel == 1)
                    {
                        Debug.Log("ChargingStep2");
                        ChargingStep2();
                    }
                    _chargeLevel = 2;
                    break;
                                
                case > 0f:
                    if (_chargeLevel == 0)
                    {
                        Debug.Log("ChargingStep1");
                        ChargingStep1();
                    }
                    _chargeLevel = 1;
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

    public override void OnExit() // 이거 기본 불리언 초기화 하기
    {
        _doAttack    = false;           
        _doCharging  = false;        
        _ReadyAttack = false;  
        
        if (!_comboAttack)
        {
            _swordOfMagicSword.Explode();
            StandbyAdditionalUse();
        }
        
    }
    
    async void StandbyAdditionalUse()
        {
            _doComboStandby = true ;
            await Task.Delay(600);
            _doComboStandby = false;
        }
    
    public override void OnSuccessfullyCast() // 차지 시작
    {
        if (_comboAttack)
        {
            ComAttack();
        }
        else if (!_comboAttack)
        {
            _doCharging = true;
            _ReadyAttack = true;
        }
        
    }

    private void ComAttack()
    {
        Debug.Log("PlayCombo Attack");
    }
    
    private void ChargeCompleate()
    {
        _ReadyAttack = false;
        _doCharging  = false;
        _doAttack    = true;
        
        _magicSwordArtsSword.MagicSword_Slash(_damageDatas[_chargeLevel - 1]);
        
        castPlayer.animation.PlayTargetAction("MagicSwrodArt_Attack", true, true, false, false);
        
        
    }
    
    public void ChargingStep0()
    {
        _chargeLevel = 0;
        // _magicSwordArtsTrajectory.ChargeLevel0_SetEffect(castPlayer);
        _swordOfMagicSword.ChargeLevel0_SetParticle(castPlayer);
    }

    public void ChargingStep1()
    {
        _chargeLevel = 1;
        // _magicSwordArtsTrajectory.ChargeLevel1_SetEffect();
        _swordOfMagicSword.SetScale(_chargeToTargetScale[0]);
        _swordOfMagicSword.ChargeLevel1_SetParticle();
    }

    public void ChargingStep2()
    {
        _chargeLevel = 2;
        // _magicSwordArtsTrajectory.ChargeLevel2_SetEffect();
        _swordOfMagicSword.SetScale(_chargeToTargetScale[1]);
        _swordOfMagicSword.ChargeLevel2_SetParticle();
    }

    public void ChargingStep3()
    {
        _chargeLevel = 3;
        // _magicSwordArtsTrajectory.ChargeLevel3_SetEffect();
        _swordOfMagicSword.SetScale(_chargeToTargetScale[2]);
        _swordOfMagicSword.ChargeLevel3_SetParticle();
    }
    
}
