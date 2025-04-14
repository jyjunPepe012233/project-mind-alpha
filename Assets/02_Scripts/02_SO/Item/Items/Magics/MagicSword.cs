using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using MinD.Runtime.Object.Magics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Magic Sword")]
public class MagicSword : Magic
{
    [SerializeField] private GameObject magicSword;
    
    private List<GameObject> projectiles; // 발사체 저장 리스트
    private List<MagicSwordProjectile> swordProjectiles; // 발사체들 MagicSwordProjectile
    
    public static readonly Vector3[] projectilePositions = new Vector3[3]
    {
        new Vector3( 1, -1,  1).normalized * 0.8f,
        new Vector3( 0,  0,  0),
        new Vector3(-1,  1, -1).normalized * 0.8f
    };
    
    /*ToDo :: setPosition 리팩토링, 이팩트 조정*/
    
    public override void OnUse()
    {
        
        if (!castPlayer.isPerformingAction){
            castPlayer.animation.PlayTargetAction("MagicSword",true, true, false, false);
        }
        projectiles = new List<GameObject>();
        swordProjectiles = new List<MagicSwordProjectile>();
    }
    
    
    public override void Tick()
    {
        
        if (!castPlayer.isPerformingAction)
        {
            castPlayer.combat.ExitCurrentMagic();
        }
        
    }

    public override void OnReleaseInput()
    {
    }

    public override void OnCancel()
    {
        for (int i = 0; i < 3; i++)
        {
            MagicSwordProjectile sword = swordProjectiles[i];
            sword.StartCoroutine(sword.Explode());
        }
        castPlayer.combat.ExitCurrentMagic();
    }

    public override void OnExit()
    {
    }
    
    public override void OnSuccessfullyCast()
    {
        
        for (int i = 0; i < 3; i++) // 마법검 생성 및 위치 조정
        {
            //  createSword
            projectiles.Add(Instantiate(magicSword, castPlayer.transform.position + new Vector3(0, 2.1f, 0),
                castPlayer.transform.rotation));
            
            swordProjectiles.Add(projectiles[i].GetComponent<MagicSwordProjectile>());

            //  set swordPosition
            swordProjectiles[i].StartCoroutine(swordProjectiles[i]
                .SetSwordPosition(castPlayer,projectilePositions[i]));
            
        }

    }
}
}

