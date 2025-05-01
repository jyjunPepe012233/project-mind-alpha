using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using MinD.Runtime.Object.Magics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Magic Sword")]
public class MagicSword : Magic
{

    [SerializeField] private GameObject _magicSwordProjectileOrigin;
    [SerializeField] private Vector3[] _magicSwordSetPosition;
     
    private GameObject[] _magicSwordProjectileGamaObjects;
    private MagicSwordProjectile[] _magicSwordProjectiles; 

    // private void OnEnable()
    // {
    //     _magicSwordProjectileGamaObjects = new GameObject[_magicSwordSetPosition.Length];
    //     _magicSwordProjectiles = new MagicSwordProjectile[_magicSwordSetPosition.Length];
    //
    //     for (int i = 0; i < _magicSwordSetPosition.Length; i++)
    //     {
    //         _magicSwordProjectileGamaObjects[i] = Instantiate(_magicSwordProjectileOrigin, castPlayer.transform.position, castPlayer.transform.rotation);
    //         _magicSwordProjectiles[i] = _magicSwordProjectileGamaObjects[i].GetComponent<MagicSwordProjectile>();
    //
    //         _magicSwordProjectiles[i].PrepareSword(castPlayer, _magicSwordSetPosition[i]);
    //     }
    // }

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
        
        castPlayer.combat.ExitCurrentMagic();
    }

    public override void OnExit()
    {
    }
    
    public override void OnSuccessfullyCast()
    {
        _magicSwordProjectileGamaObjects = new GameObject[_magicSwordSetPosition.Length];
        _magicSwordProjectiles = new MagicSwordProjectile[_magicSwordSetPosition.Length];
        
        for (int i = 0; i < _magicSwordProjectileGamaObjects.Length; i++) // 마법검 생성 및 위치 조정
        {
            _magicSwordProjectileGamaObjects[i] = Instantiate(_magicSwordProjectileOrigin, castPlayer.transform.position +new Vector3(0, 1.2f, 0), castPlayer.transform.rotation);
            _magicSwordProjectiles[i] = _magicSwordProjectileGamaObjects[i].GetComponent<MagicSwordProjectile>();

            _magicSwordProjectiles[i].PrepareSword(castPlayer, _magicSwordSetPosition[i]);
            
            _magicSwordProjectiles[i].GameObject().SetActive(true);
            _magicSwordProjectiles[i].StartCoroutine(_magicSwordProjectiles[i].SetSwordPosition());
        }

    }
    
}
}


