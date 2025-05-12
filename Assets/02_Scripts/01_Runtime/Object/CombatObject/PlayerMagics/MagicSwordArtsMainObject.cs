using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.Utils;
using MinD.SO.Object;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Serialization;

public class MagicSwordArtsMainObject : MonoBehaviour
{
    
    [SerializeField] private MagicSwordArtsSword _magicSwordArtsSword_Origin;
    [SerializeField] private GameObject          _magicSwordArtsSwordGameObject_Origin;
    
    // private MagicSwordArtsTrajectory             _magicSwordArtsTrajectory_Origin;
    // [SerializeField] private GameObject          _magicSwordArtsTrajectoryGameObject_Origin; // 이거 굳이 이렇게 해야 하는지 검사하기 그냥 바로 받아오면 안돼나?
    

    [Header("[ Set State ]")] [Space(5)]
    [SerializeField] private DamageData[] _comboAttackDamageDatas   = new DamageData[1];
    [SerializeField] private DamageData[] _chargeLevelToDamageDatas = new DamageData[4]; // 0: 차징X 부터, 차징 단계에 따른 것
    [SerializeField] private Vector3[]  _chargeToTargetScales       = new Vector3[4];
    
    private bool _readyChargeAttack    = false;      // 차지 공격 시 공격 준비가 완료 되었는가
    
    // 로직 내에서 사용 할 클래스 저장 변수들(위에 애들 복사본)
    private GameObject               _magicSwordGameObject;
    private MagicSwordArtsSword       _magicSwordArtsSword;
    // private MagicSwordArtsTrajectory _magicSwordTrajectory;
    
    
    // 현재 차지 레벨과 차지를 진행한 시간
    private float _chargeElapsedTime = 0;
    private int _chargeLevel = 0;

    public Player _castPlayer;
    
    
    
    private void OnEnable()
    {
        ChackNotNullMagicsword();
        // ChackNotNullMagicSwordTrajectory();
    }

    private void ChackNotNullMagicsword()
    {
        if (_magicSwordArtsSword == null)
        {
            _magicSwordArtsSword = Instantiate(_magicSwordArtsSword_Origin, _castPlayer.equipment.rightHand.position,
                _castPlayer.equipment.rightHand.rotation);;
        }
    }

    // private void ChackNotNullMagicSwordTrajectory()
    // {
    //     if (_magicSwordTrajectory == null)
    //     {
    //
    //         if (_magicSwordArtsTrajectory_Origin == null)
    //         {
    //             _magicSwordArtsTrajectory_Origin = _magicSwordArtsTrajectoryGameObject_Origin.GetComponent<MagicSwordArtsTrajectory>();
    //         }
    //         
    //         _magicSwordTrajectory = Instantiate(_magicSwordArtsTrajectory_Origin, _castPlayer.transform.position,
    //             _castPlayer.transform.rotation);;
    //     }
    // }
    

    public void StartComboAttack(bool __comboStep)
    {
        Debug.Log("StartComboAttack");
        
        ComboAttack(__comboStep);
    }

    public void StartChargeAttack(int __chargeLevel)
    {
        ChargeAttack(__chargeLevel);
    }

    public void MagicSwordColliderActiveTrue()
    {
        ChackNotNullMagicsword();
        if (_magicSwordArtsSword._collider == null)
        {
            _magicSwordArtsSword._collider = _magicSwordGameObject.GetComponent<CapsuleCollider>();
        }
        _magicSwordArtsSword._collider.enabled = true;
    }
    
    public void MagicSwordColliderActiveFalse()
    {
        ChackNotNullMagicsword();
        if (_magicSwordArtsSword._collider == null)
        {
            _magicSwordArtsSword._collider = _magicSwordGameObject.GetComponent<CapsuleCollider>();
        }
        _magicSwordArtsSword._collider.enabled = false;
    }

    #region Attacks

    private void ComboAttack(bool __comboStep) // 이거 콤보스텝에 연동하기
    {
        ChackNotNullMagicsword();
        // _magicSwordArtsSword.ReSetDamageCollider();
        
        _magicSwordArtsSword.ComboAttack_Set(_castPlayer);
        

        if (!__comboStep)
        {
            Debug.Log("Play MagicSwrodArt_Combo_2");
            _castPlayer.animation.PlayTargetAction("MagicSwordArt_Combo_2", true, true, false, false);
        }
        else
        {
            Debug.Log("Play MagicSwordArt_Combo_1");
            _castPlayer.animation.PlayTargetAction("MagicSwordArt_Combo_1", true, true, false, false);
        }
        
    }

    private void ChargeAttack(int __chargeLevel)
    {
        ChackNotNullMagicsword();
        // ChackNotNullMagicSwordTrajectory();
        
        _magicSwordArtsSword.Slash_setParticle();
        _magicSwordArtsSword.MagicSword_Slash(_chargeLevelToDamageDatas[__chargeLevel]);
        
        Debug.Log("Play MagicSwrodArt_ChargeAttack");
        _castPlayer.animation.PlayTargetAction("MagicSwrodArt_Attack", true, true, false, false);
    }
    
    #endregion


    public void MagicSwordExplode()
    {
        _magicSwordArtsSword?.Explode();
        _magicSwordArtsSword = null;
    }
    
    #region ChargingLevel To Effect

    public void StartCharging()
    {
        _castPlayer.animation.PlayTargetAction("MagicSwordArt_Charge", true, true, false, false);
    }
    public void ChargingLavel0()
    {
        ChackNotNullMagicsword();
     
        StartCharging();
        
        _magicSwordArtsSword.ChargeLevel0_SetParticle(_castPlayer);
        _magicSwordArtsSword.SetScale(_chargeToTargetScales[0]);
    }
    public void ChargingLavel1()
    {
        ChackNotNullMagicsword();
        _magicSwordArtsSword.ChargeLevel1_SetParticle();
        _magicSwordArtsSword.SetScale(_chargeToTargetScales[1]);
    }
    public void ChargingLavel2()
    {
        ChackNotNullMagicsword();
        _magicSwordArtsSword.ChargeLevel2_SetParticle();
        _magicSwordArtsSword.SetScale(_chargeToTargetScales[2]);
    }
    public void ChargingLavel3()
    {
        ChackNotNullMagicsword();
        
        _magicSwordArtsSword.ChargeLevel3_SetParticle();
        _magicSwordArtsSword.SetScale(_chargeToTargetScales[3]);
    }
    
    #endregion
    
}
