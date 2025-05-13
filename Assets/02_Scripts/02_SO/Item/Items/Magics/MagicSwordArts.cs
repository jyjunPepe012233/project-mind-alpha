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
    
    [FormerlySerializedAs("_comboStep")]
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
    
    [SerializeField] private bool   _comboStepIsOne = true;  // 콤보가 얼마나 진행되었는지
    [SerializeField] private float _chargingTime    = 0;  // 차지를 진행한 시간이 얼마나 되는지
    [SerializeField] private bool  _doChargeAttack  = false;     // 차지 공격을 사욘ㅇ하는가? 
    [SerializeField] private bool  _doStandbyCombo  = false;     // 콤보 공격에 대해 대비중인가
    [SerializeField] private bool  _doAttack        = false;     // 공격을 진행 하고 있는가.
    // [SerializeField] private bool _readyAttack     = false;

    public override void OnUse()
    {
        _chargingTime = 0;
        _doChargeAttack = false;
        _doAttack = false;
        _doStandbyCombo = false;
        CheckNullCastPlayerToMagicMainObj();
    }

    private void CheckNullCastPlayerToMagicMainObj()
    {
        if (_magicSwordArtsMainObject._castPlayer == null)
        {
            _magicSwordArtsMainObject._castPlayer = castPlayer;
        }
    }
    
    public override void OnDamageCollider()
    {
        Debug.Log("OnDamageCollider");
        
        CheckNullCastPlayerToMagicMainObj();
        if (_doChargeAttack)
        {
            _magicSwordArtsMainObject.CreateChargeAttackCollider();
        }
        else
        {
            _magicSwordArtsMainObject.CreateComboAttackCollider(_comboStepIsOne);
        }
    }

    public override void OffDamageCollider()
    {
        Debug.Log("OffDamageCollider");
        CheckNullCastPlayerToMagicMainObj();
        _magicSwordArtsMainObject.DestroyAttackCollider();
    }
    
    public override void Tick()
    {
        
        // castPlayer Setting
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
            
            if (_doStandbyCombo)
            {
                Debug.Log("next combo Attack, ComboStepIsOne : " + _comboStepIsOne );
                _doStandbyCombo = false;    
                    
                if ((castPlayer.CurMp >= this.mpCost) && (castPlayer.CurStamina >= this.staminaCost))
                {
                    castPlayer.CurMp -= this.mpCost;
                    castPlayer.CurStamina -= this.staminaCost;
                                
                    _magicSwordArtsMainObject.StartComboAttack(_comboStepIsOne);
                    _comboStepIsOne = !_comboStepIsOne;
                }
            }
        }
        // 조건에 맞다면 일반 콤조 공격 진행
        else if ((!_doChargeAttack && !_doAttack))
        {
            Debug.Log("combo Attack");
            _doAttack = true;
            _magicSwordArtsMainObject.StartComboAttack(_comboStepIsOne);
            _comboStepIsOne = !_comboStepIsOne;
            _chargingTime = 0;
        }
        // 조건에 맞다면 차징 공격 진행
        else if ((_doChargeAttack || !_comboStepIsOne) && !_doAttack) 
        {
            _doAttack = true;
            _chargingTime = 0;
            
            Debug.Log("not fullyCast Attack");
            _magicSwordArtsMainObject.StartChargeAttack();
        }
        // 아무것도 아니면 차징시간 초기화
        else
        {
            Debug.Log("reset to charge time");
            _chargingTime = 0;
        }
        
        // 차징 시간에 따른 차징 단계
        if (!_doAttack && _chargingTime > 0.3f)
        {
            Debug.Log("charge Attack");
            _magicSwordArtsMainObject.StartChargeAttack();
            
            _doAttack = true;
            _doChargeAttack = true;
            _chargingTime = 0;
                    
        }
        
        
        // 애니메이션 종료시 끝내기
        if (!castPlayer.isPerformingAction && !( PlayerInputManager.Instance.useMagicInput ) && !_doStandbyCombo) 
        {
            Debug.Log("End Magic");
            castPlayer.combat.ExitCurrentMagic();
        }
        
        
    }


    public override void OnReleaseInput()
    {
    }

    public override void OnCancel()
    {
        castPlayer.combat.ExitCurrentMagic();
    }

    public override void OnExit() // 기본 불리언 초기화 하기
    {
        _chargingTime = 0;
        _doChargeAttack = false;
        _doAttack = false;
        _comboStepIsOne = true;
        
        _magicSwordArtsMainObject.MagicSwordExplode();
    }
    
    
    public override void ComboStandbyStart()
    {
        Debug.Log("ComboStandbyStart");
        _doStandbyCombo = true;

        // CheckNullCastPlayerToMagicMainObj();
        // _magicSwordArtsMainObject.DestroyAttackCollider();
    }

    public override void ComboStandbyEnd()
    {
        Debug.Log("ComboStandbyEnd");
        _comboStepIsOne = true;
        _doStandbyCombo = false;

        // CheckNullCastPlayerToMagicMainObj();
        // _magicSwordArtsMainObject.DestroyAttackCollider();
    }

}
