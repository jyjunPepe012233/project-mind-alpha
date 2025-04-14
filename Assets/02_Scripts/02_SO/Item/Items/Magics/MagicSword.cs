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
    
    public event Action OnMagicSwordSetEvent;
    public event Action OnMagicSwordShootEvent; // 발사체에서 관리하기
    public event Action OnDestroyMagicSwordEvent;
    
    /*ToDo :: 이팩트 조정*/
     
    [SerializeField] private GameObject[] _magicSwordProjectileGamaObgect;
    [SerializeField] private MagicSwordProjectile[] _magicSwordProjectiles; 

    private void OnEnable()
    {
        // for (int i = 0; i < _magicSwordProjectileGamaObgect.Length; i++) 
        // {
        //     _magicSwordProjectiles[i] = _magicSwordProjectileGamaObgect[i].GetComponent<MagicSwordProjectile>();
        // }
    }

    public override void OnUse()
    {
        
        if (!castPlayer.isPerformingAction){
            castPlayer.animation.PlayTargetAction("MagicSword",true, true, false, false);
        }
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
        OnDestroyMagicSwordEvent.Invoke();
        castPlayer.combat.ExitCurrentMagic();
    }

    public override void OnExit()
    {
    }
    
    public override void OnSuccessfullyCast()
    {
        
        for (int i = 0; i < _magicSwordProjectileGamaObgect.Length; i++) // 마법검 생성 및 위치 조정
        {
            Debug.Log(_magicSwordProjectileGamaObgect.Length);
            Debug.Log(_magicSwordProjectiles.Length);
            Debug.Log(i);
            _magicSwordProjectiles[i] = _magicSwordProjectileGamaObgect[i].GetComponent<MagicSwordProjectile>();
            _magicSwordProjectiles[i].GameObject().SetActive(true);
            Instantiate(_magicSwordProjectileGamaObgect[i], castPlayer.transform.position + new Vector3(0,1.5f,0), castPlayer.transform.rotation);
        }
        
        OnMagicSwordSetEvent.Invoke();
        
    }
}
}

