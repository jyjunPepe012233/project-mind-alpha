using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.Utils;
using MinD.SO.Object;
using MinD.Utility;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Serialization;

public class MagicSwordArtsMainObject : MonoBehaviour
{
    
    [SerializeField] private MagicSwordArtsSword _magicSwordArtsSword_Origin;
    [SerializeField] private GameObject          _magicSwordArtsSwordGameObject_Origin;
    
    [Header("[ Set State ]")] [Space(5)]
    // [SerializeField] private DamageData[] _comboAttackDamageDatas = new DamageData[2];
    [SerializeField] private GameObject[]   _comboAttackColliders   = new GameObject[2];

    // [SerializeField] private DamageData _chargeAttackDamageData;
    [SerializeField] private GameObject   _chargeAttackCollider;
    
    private bool _readyChargeAttack    = false; // 차지 공격 시 공격 준비가 완료 되었는가
    
    // 로직 내에서 사용 할 클래스 저장 변수들(위에 애들 복사본)
    private GameObject               _magicSwordGameObject;
    private MagicSwordArtsSword       _magicSwordArtsSword;
    
    private GameObject                       _damageCollider; // 데미지 콜라이더 저장
    
    // 현재 차지 레벨과 차지를 진행한 시간
    private float _chargeElapsedTime = 0;
    private int _chargeLevel = 0;

    public Player _castPlayer;
    
    
    
    private void OnEnable()
    {
        ChackNotNullMagicsword();
    }

    private void ChackNotNullMagicsword()
    {
        if (_magicSwordArtsSword == null)
        {
            Debug.Log("_castPlayer : " + (_castPlayer == null));
            Debug.Log("_castPlayer.equipment" + (_castPlayer.equipment == null));
            Debug.Log("_castPlayer.equipment.rightHand" + (_castPlayer.equipment.rightHand == null));
            _magicSwordArtsSword = Instantiate(_magicSwordArtsSword_Origin, _castPlayer.equipment.rightHand.position,
                _castPlayer.equipment.rightHand.rotation);;
        }
    }

    public void StartComboAttack(bool __comboStep)
    {
        Debug.Log("StartComboAttack");
        
        ComboAttack(__comboStep);
    }

    public void StartChargeAttack()
    {
        ChargeAttack();
    }
    public void CreateComboAttackCollider(bool __stepIsOne)
    {
        Debug.Log("CreateComboAttackCollider");
        _damageCollider = Instantiate(_comboAttackColliders[(__stepIsOne) ? 0 : 1], _castPlayer.transform.position,
            _castPlayer.transform.rotation);
        PhysicUtility.IgnoreCollisionUtil(_castPlayer, _damageCollider.GetComponent<Collider>());
        _damageCollider.SetActive(true);
        Destroy(_damageCollider, 1);
    }
    
    public void CreateChargeAttackCollider()
    {
        Debug.Log("CreateChargeAttackCollider");
        _damageCollider = Instantiate(_chargeAttackCollider, _castPlayer.transform.position,
            _castPlayer.transform.rotation);
        PhysicUtility.IgnoreCollisionUtil(_castPlayer, _damageCollider.GetComponent<Collider>());
        _damageCollider.SetActive(true);
        Destroy(_damageCollider, 1);
    }

    public void DestroyAttackCollider()
    {
        Debug.Log("DestroyAttackCollider");
        Destroy(_damageCollider);
    }

    #region Attacks

    private void ComboAttack(bool __comboStep) // 이거 콤보스텝에 연동하기
    {
        ChackNotNullMagicsword();
        
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

    private void ChargeAttack()
    {
        ChackNotNullMagicsword();
        
        _magicSwordArtsSword.ChargeAttack_Set(_castPlayer);
        _magicSwordArtsSword.Slash_setParticle();
        _magicSwordArtsSword.MagicSword_Slash();
        
        Debug.Log("Play MagicSwrodArt_ChargeAttack");
        _castPlayer.animation.PlayTargetAction("MagicSwrodArt_Attack", true, true, false, false);
    }
    
    #endregion
    
    public void MagicSwordExplode()
    {
        _magicSwordArtsSword?.Explode();
        _magicSwordArtsSword = null;
    }
    
    
}
