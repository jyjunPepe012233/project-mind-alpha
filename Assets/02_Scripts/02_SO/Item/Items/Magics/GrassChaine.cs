using System;
using System.Collections;
using System.Collections.Generic;
using MinD.SO.Item;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = System.Numerics.Quaternion;
using Vector3 = System.Numerics.Vector3;
using MinD.Runtime.Object.Magics;

namespace MinD.SO.Item.Items
{

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Grass Chaine")]

public class GrassChaine : Magic
{
    private const float playerHigh = 1.2f;

    [SerializeField] private GameObject projectile;

    private GrassChaineProjectile grassChaineProjectile;
    private GameObject grassChaine;
    
    public override void OnUse()
    {
        castPlayer.animation.PlayTargetAction("GrassChaine", true, true, false, false);

        /* ToDo :: 복사해서 이동하게 하는거 하는중 이었슴,  */
        UnityEngine.Vector3 lookTarget =
            (castPlayer.combat.target.transform.position - castPlayer.transform.position).normalized;

        grassChaine = Instantiate(projectile,
            lookTarget * 1.4f
            + castPlayer.transform.position
            + castPlayer.transform.up * playerHigh,
            UnityEngine.Quaternion.LookRotation(castPlayer.transform.position, lookTarget));

        grassChaineProjectile = grassChaine.GetComponent<GrassChaineProjectile>();
        grassChaineProjectile.SetMagic(castPlayer);

    }

    public override void Tick()
    {
        Debug.Log(castPlayer);
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

    }

    public override void OnExit()
    {

    }

    public override void OnSuccessfullyCast()
    {
        grassChaineProjectile.ShootProjectile();
    }
}
}