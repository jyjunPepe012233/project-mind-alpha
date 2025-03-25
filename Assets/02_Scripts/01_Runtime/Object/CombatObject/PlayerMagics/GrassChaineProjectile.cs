using System;
using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.Utility;
using UnityEngine;
using UnityEngine.AI;


namespace MinD.Runtime.Object.Magics
{
public class GrassChaineProjectile : MonoBehaviour
{
    private const float playerHigh = 1.2f;

    [SerializeField] private GameObject chaineFx;
    [SerializeField] private GameObject projectileFx;

    [SerializeField] private ParticleSystem chainePc;
    private ParticleSystem projectilePc;

    [Space(10)] 
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Collider collider;

    private Player castPlayer;


    /* damageCollider 레이어랑 그 피격할때 방향 정해주는거 하기 */


    public void SetMagic(Player player)
    {
        castPlayer = player;
        
        chainePc = chaineFx.GetComponent<ParticleSystem>();
        projectilePc = projectileFx.GetComponent<ParticleSystem>();
        
        PhysicUtility.IgnoreCollisionUtil(castPlayer,collider);
    }

    public void ShootProjectile()
    {
        projectilePc.Play();
        navMeshAgent.SetDestination(castPlayer.combat.target.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        projectilePc.Stop();
        chainePc.Play();

        /* 이동불가 디버프 부여 코드 :: 예준이가 해줄거임 */

        Destroy(this, 3);
    }
}
}