using System;
using MinD.Runtime.Managers;
using MinD.Runtime.Object.Magics;
using MinD.SO.Item;
using MinD.SO.Object;
using UnityEngine;
using UnityEngine.Serialization;
using Task = System.Threading.Tasks.Task;

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Magic Sword Art")]

public class MagicSwordArts : Magic
{
    [Header("[ MagicSwordArts ]")]
    [Space(5)]
    [SerializeField] private MagicSwordArtsMainObject _magicSwordArtsMainObject;
    
    [Header("[ States ]")]
    [Space(10)]
    
    /* ToDo
     0. 열심히 하기..?
     1. 파티클 크기 연동 안됨 - 차징단계에 따른 크기변화 : V
     
     2. 차징 고치기, 차징 안됨 ㅅㅂ : X
     3. 록온 관련 기능 구현하gi : X
     4. 칼 사라지기 : X -> 잰행중
     5. 콤보 구현하기 (검기) : X
     6. 파티클 개선 (차징, 확대, 스래쉬)
     */
    
    /*
     * :: combo ::
     * 1. 공격
     * 2. 공격
     * 3. 차지 공격 가능
     */
    
    [SerializeField] private int   _comboStep      = 1;  // 콤보가 얼마나 진행되었는지
    [SerializeField] private int   _chargeLevel    = 0;  // 차지의 단계가 얼마나 되는지
    [SerializeField] private float _chargingTime   = 0;  // 차지를 진행한 시간이 얼마나 되는지
    [SerializeField] private bool  _doChargeAttack = false;     // 차지 공격을 사욘ㅇ하는가? 
    [SerializeField] private bool  _doStandbyCombo = false;     // 콤보 공격에 대해 대비중인가
    [SerializeField] private bool  _doAttack       = false;     // 공격을 진행 하고 있는가.
    // [SerializeField] private bool _readyAttack     = false;

    public override void OnUse()
    {
        _chargingTime = 0;
        _chargeLevel  = 0;
        _doChargeAttack = false;
        _doAttack = false;
        _doStandbyCombo = false;
        
        Debug.Log("SetCastPlayer Compleate");
        _magicSwordArtsMainObject._castPlayer = castPlayer;
    }
    
    public override void Tick()
    {
        
        Debug.Log("PlayerInputManager.Instance.useMagicInput : " + PlayerInputManager.Instance.useMagicInput);
        Debug.Log("isInputReleased : " + isInputReleased);

        if (_magicSwordArtsMainObject._castPlayer == null)
        {
            Debug.Log("_magicSwordArtsMainObject._castPlayer == null");
            _magicSwordArtsMainObject._castPlayer = castPlayer;
        }
        
        // 차징 중인가?
        if ( PlayerInputManager.Instance.useMagicInput ) 
        {
            Debug.Log("do Charging");
            _chargingTime += Time.deltaTime;
        }
        
        // VV 차징이 풀렷을 때
        
        // 조건에 맞다면 일반 콤조 공격 진행
                else if ((!_doChargeAttack && !_doAttack))// && !_doStandbyCombo) 
                {
                    Debug.Log("combo Attack");
                    _doAttack = true;
                    _magicSwordArtsMainObject.StartComboAttack();
        
                    if (_comboStep < 3)
                    {
                        _comboStep = 1;
                    }
                    _chargingTime = 0;
                }
        
        // 조건에 맞다면 차징 공격 진행
        else if ((_doChargeAttack || _comboStep == 3) && !_doAttack) 
        {
            _doAttack = true;
            _chargingTime = 0;
            _comboStep = 1;
            
            Debug.Log("not fullyCast Attack");
            _magicSwordArtsMainObject.StartChargeAttack(_chargeLevel);
            
        }
        
        
        else
        {
            Debug.Log("reset to charge time");
            _chargingTime = 0;
        }

        if ( _doStandbyCombo && PlayerInputManager.Instance.useMagicInput)
        {
            Debug.Log("ComboStandby");
            _doStandbyCombo = false;
            
            if ((castPlayer.CurMp >= this.mpCost) && (castPlayer.CurStamina >= this.staminaCost))
                    {
                        castPlayer.CurMp -= this.mpCost;
                        castPlayer.CurStamina -= this.staminaCost;
                        castPlayer.animator.SetBool("ComboStandby", true);
                    }
            
        }
        
        
        
        // 애니메이션 종료시 끝내기
        if (!castPlayer.isPerformingAction && !( PlayerInputManager.Instance.useMagicInput || !isInputReleased)) 
        {
            Debug.Log("End Magic");
            castPlayer.combat.ExitCurrentMagic();
        }
        
        
        // 차징 시간에 따른 차징 단계
        if (!_doAttack)
        {
            switch (_chargingTime)
                    {
                        case > 4f:
                            Debug.Log("StartChargeAttack");
                            _doAttack = true;
                            _magicSwordArtsMainObject.StartChargeAttack(3);
                            break;
                        
                        case > 3.7f:
                            if (_chargeLevel != 2)
                            {
                                Debug.Log("ChargingLavel3");
                                _magicSwordArtsMainObject.ChargingLavel3();
                            }
                            _chargeLevel = 2;
                            break;
                            
                        case > 2.1f:
                            if (_chargeLevel != 1)
                            {
                                Debug.Log("ChargingLavel2");
                                _magicSwordArtsMainObject.ChargingLavel2();
                            }
                            _chargeLevel = 1;
                            break;
                                        
                        case > 1.1f:
                            if (_chargeLevel != 0)
                            {
                                Debug.Log("ChargingLavel1");
                                _magicSwordArtsMainObject.ChargingLavel1();
                            }
                            _chargeLevel = 0;
                            break;
                        
                        // case > 0.2f:
                        //     _magicSwordArtsMainObject.StartCharging();
                        //     break;
                        
                        case > 0.3f:
                            
                            Debug.Log("charge Start");
                            _magicSwordArtsMainObject.StartCharging();
                            _magicSwordArtsMainObject.ChargingLavel0();
                            
                            _doChargeAttack = true;
                            _chargeLevel = 0;
                            break;
                    }
                    
        }
        
    }


    public override void OnReleaseInput()
    {
    }

    public override void OnCancel()
    {
        // _magicSwordArtsMainObject._
        castPlayer.combat.ExitCurrentMagic();
    }

    public override void OnExit() // 기본 불리언 초기화 하기
    {
        
        _chargingTime = 0;
        _doChargeAttack = false;
        _chargeLevel = 0;
        _doAttack = false;
        
        _magicSwordArtsMainObject.MagicSwordExplode();
    }
    
    
    public override void ComboStandbyStart()
    {
        Debug.Log("ComboStandbyStart");
        _doStandbyCombo = true;
        // castPlayer.animator.SetBool("ComboStandby", true);
    }

    public override void UseComboAttack()
    {
        
        Debug.Log("UseComboAttack");
        castPlayer.animator.SetBool("ComboStandby", false);
        _magicSwordArtsMainObject.ReUseComboAttack();
        
        // castPlayer.animation.PlayTargetAction("Default Movement", true, true, false, false);
        castPlayer.combat.ExitCurrentMagic();
    
    }
}
