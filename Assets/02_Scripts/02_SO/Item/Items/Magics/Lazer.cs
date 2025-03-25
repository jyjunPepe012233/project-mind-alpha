using System;
using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Object.Magics;
using MinD.SO.Item;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = System.Numerics.Vector3;


namespace MinD.SO.Item.Items
{


[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Lazer")]
public class Lazer : Magic
{
    
     [SerializeField] private GameObject lazer;
     private Transform targetOption;

     [SerializeField] private float createHigh = 1.2f;

     
    private LazerProjectile lazerProjectile;
    private GameObject copyLazer;

    /* ToDo :: 완료 후 마감하기 */

    public override void OnUse()
    {
        if (!castPlayer.isPerformingAction)
        {
            castPlayer.animation.PlayTargetAction("Lazer", true, true, true, false);
            targetOption = castPlayer.camera.currentTargetOption;
            if (castPlayer.isLockOn)
            {
                copyLazer = Instantiate(
                    lazer,
                    (castPlayer.combat.target.transform.position - castPlayer.transform.position).normalized*1.4f
                        + castPlayer.transform.up * createHigh
                        + castPlayer.transform.position
                        , Quaternion.LookRotation(castPlayer.combat.target.transform.position - castPlayer.transform.position));
            }
            else
            {
                copyLazer = Instantiate(lazer, castPlayer.transform.position + castPlayer.transform.forward * 1.4f + castPlayer.transform.up * createHigh, castPlayer.transform.rotation);
            }
            
            
            lazerProjectile = copyLazer.GetComponent<LazerProjectile>();
            lazerProjectile.SetUseMagic(castPlayer);
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
        //
    }

    public override void OnCancel()
    {
        //
    }

    public override void OnExit()
    {
        //
    }

    public override void OnSuccessfullyCast()
    {
        copyLazer.SetActive(true);
        if (castPlayer.isLockOn)
        {
            lazerProjectile.ShootLazer(targetOption.position);
        }
        else
        {
            lazerProjectile.ShootLazer();
        }
    }

}
}