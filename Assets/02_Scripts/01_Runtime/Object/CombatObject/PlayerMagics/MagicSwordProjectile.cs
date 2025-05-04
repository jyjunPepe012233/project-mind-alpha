using System;
using System.Collections;
using MinD.Runtime.Entity;
using MinD.Runtime.Utils;
using MinD.SO.Item;
using MinD.SO.Item.Items;
using MinD.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace MinD.Runtime.Object.Magics
{
public class MagicSwordProjectile : MonoBehaviour
{

    [FormerlySerializedAs("magicProjectile")]
    [Space(10)] 
    [SerializeField] private GameObject magicProjectileFx;
    [SerializeField] private GameObject flightFx;
    [SerializeField] private GameObject explosionFx;
    
    private ParticleSystem magicProjectilePc;
    private ParticleSystem flightPc;
    private ParticleSystem explosionPc;
    private ParticleSystem ShootProjectilePc;
    
    [Space(10)]
    [SerializeField] private DamageCollider explosionDamageCollider;
    
    private Rigidbody rigidbody;
    private Collider collider;
    
    private Player _castPlayer;
    private Vector3 _settingPosition; // 셋 위치
    [SerializeField] private float _setLerpSpace = 0.5f;
    
    [SerializeField] private float _speed;
    
    private bool isExploded;
    
    public void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        collider.enabled = false;

        magicProjectilePc = magicProjectileFx.GetComponent<ParticleSystem>();
        flightPc = flightFx.GetComponent<ParticleSystem>();
        explosionPc = explosionFx.GetComponent<ParticleSystem>();

    }

    
    /* ToDo :: 삭제 이펙트 만들기 */

    public IEnumerator ShootCoroutine()
    {
        float elapsedTime = 0f;
        
        PhysicUtility.IgnoreCollisionUtil(_castPlayer, collider);
        
        GetComponent<Rigidbody>().isKinematic = false;
        
        flightFx.SetActive(true);
        
        /* 콜라이더들 활성화 */
        collider.isTrigger = true;
        collider.enabled = true;
        
        if (_castPlayer.isLockOn) // 적 감지 시 추척하여 발사
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(_castPlayer.camera.currentTargetOption.transform.position - transform.position)
                , 360); // 처음엔 방향 설정
            
            while (!isExploded)
            {
                elapsedTime += Time.deltaTime;

                if (!_castPlayer.combat.target.isDeath)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation,
                        Quaternion.LookRotation(_castPlayer.combat.target.transform.position  + new Vector3(0, _castPlayer.combat.target.transform.lossyScale.y * 1.2f ,0) - transform.position), 15 * Time.deltaTime);
                }
                rigidbody.velocity = (transform.forward * (5 + ( elapsedTime * (elapsedTime /2 )* 3f) * _speed ));
                
                if (elapsedTime >= 5)
                {
                    yield return StartCoroutine(Explode());
                    continue;
                }

                yield return null;

            }
            
        }
        else // 적 감지 실패 시 그냥 발사
        {
            while (true)
            {
                elapsedTime += Time.deltaTime;
                
                rigidbody.velocity = transform.forward * _speed;
                
                if (elapsedTime >= 3f)
                {
                    yield return StartCoroutine(Explode());
                    continue;
                }

                yield return null;

            }
        }
        
        
    }

    public void PrepareSword(Player __castPlayer, Vector3 __setPosition)
    {
        _castPlayer = __castPlayer;
        _settingPosition = __setPosition;
    }

    public IEnumerator SetSwordPosition()
    {
        float __high = 2.1f;
        float __lerpSpace = 0.25f;
        
        float __elapsedTime = 0;
        
        while (true) // 타겟 있을 시 카메라 따라가기, 평소에는 플레이어 시야
        {
            __elapsedTime += Time.deltaTime;

            if (_castPlayer.isLockOn)  // is LookOn
            {
                Vector3 __targetDirection = _castPlayer.transform.position - _castPlayer.combat.target.transform.position;

                // 카메라 기준
                transform.position = Vector3.Lerp(transform.position,
                    Vector3.Cross(__targetDirection, transform.up).normalized * _settingPosition.x
                    
                    + _castPlayer.transform.forward * _settingPosition.z
                    + _castPlayer.transform.up * _settingPosition.y
                    
                    + new Vector3(0,__high ,0) + _castPlayer.transform.position 
                    , __lerpSpace);
                
                transform.rotation = Quaternion.LookRotation( _castPlayer.combat.target.transform.position - transform.position);
                
            }
            else
            {
                // 플레이어 기준
                transform.position = Vector3.Lerp(transform.position,
                _castPlayer.transform.right * _settingPosition.x // 좌,우 방향벡터
                + _castPlayer.transform.up * _settingPosition.y // 아래 방향벡터
                
                + new Vector3(0, __high, 0) + _castPlayer.transform.position // 상수 (사실아님)
                , __lerpSpace);
                
                transform.rotation = _castPlayer.transform.rotation;
            }
            
            if (__elapsedTime >= 1.2f)
            {
                StartCoroutine(ShootCoroutine());
                yield break;
            }
            
            yield return null;

        }
        
    }
    private IEnumerator Explode()
    {
        if (!isExploded)
        {
            isExploded = true;
            explosionDamageCollider.gameObject.SetActive(true);

            magicProjectileFx.SetActive(false);
            flightFx.SetActive(false);
            explosionPc.Play();

            rigidbody.velocity = Vector3.zero;
            Destroy(gameObject, 3);

            yield break;
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
    if (!isExploded)
    {
        StartCoroutine(Explode());
    }
    }
}
}
