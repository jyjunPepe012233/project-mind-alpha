using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.Utility;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Quaternion = System.Numerics.Quaternion;

namespace MinD.Runtime.Object.Magics
{

public class LazerProjectile : MonoBehaviour // LazerProjectile
{
    private const float flightTime = 3;
    
    [SerializeField] private GameObject useFx;
    [SerializeField] private GameObject damageFx;
    [SerializeField] private GameObject magicCircleFx;
    [SerializeField] private GameObject ShootProjectileFx;
    
    private ParticleSystem usePc;
    private ParticleSystem damagePc;
    private ParticleSystem magicCirclePc;
    private ParticleSystem ShootProjectilePc;
    
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Collider collider;
    
    [Header("[ Status ]")] [SerializeField]
    private float lazerSpeed;

    private Player castPlayer;
    private Vector3 targetDirx;

    private bool doShoot = false;

    private float _flightTime;

    /* ToDo :: 콜라이저 무시랑 달리기 중 시전 시 방향 */

    private void OnEnable()
    {
        magicCirclePc = magicCircleFx.GetComponent<ParticleSystem>();
        usePc = useFx.GetComponent<ParticleSystem>();
        damagePc = damageFx.GetComponent<ParticleSystem>();
        ShootProjectilePc = ShootProjectileFx.GetComponent<ParticleSystem>();
    }

    public void SetUseMagic(Player player)
    {
        castPlayer = player;
        lazerSpeed = 0;
        magicCirclePc.Play();
        
        PhysicUtility.IgnoreCollisionUtil(castPlayer,collider);
        
        StartCoroutine(SetRotation());

    }

    private IEnumerator SetRotation()
    {
        while (true)
        {
            if (castPlayer.isLockOn)
            {
                castPlayer.transform.rotation = UnityEngine.Quaternion.RotateTowards
                (castPlayer.transform.rotation, UnityEngine.Quaternion.LookRotation(castPlayer.combat.target.transform.position - castPlayer.transform.position),
                    720 * Time.deltaTime);
            }

            if (doShoot)
            {
                yield break;
            }

            yield return null;
        }
    }
        
        
    public void SetShootLazer()
    {
        lazerSpeed = 30;
        magicCircleFx.SetActive(false);
        ShootProjectilePc.Play();
        useFx.SetActive(true);
        doShoot = true;
    }

    public void ShootLazer(Vector3 targetDirx)
    {
        SetShootLazer();
        this.targetDirx = targetDirx;
        transform.rotation = UnityEngine.Quaternion.LookRotation(targetDirx - transform.position);
    }

    public void ShootLazer()
    {
        SetShootLazer();
        targetDirx = castPlayer.transform.forward;
    }

    public void Update()
    {
        rigidbody.velocity = transform.forward * lazerSpeed;


        _flightTime += Time.deltaTime;
        if (flightTime < _flightTime)
        {
            Explode();
        }
    }

    public void Explode()
    {
        useFx.SetActive(false);
        damagePc.Play();

        rigidbody.isKinematic = true;
        Destroy(gameObject,1);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        Explode();
    }
}
}
