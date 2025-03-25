using UnityEngine;
using UnityEngine.Serialization;

namespace MinD.SO.Item.Items
{

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Healing")]

public class Healing : Magic
{
    [FormerlySerializedAs("healingFx")] [SerializeField] private GameObject healingObj;

    private GameObject healing;
    private HealingParticle healingPc;
    
    public override void OnUse()
    {
        castPlayer.animation.PlayTargetAction("Healing",true,true,false,false);

        healing = Instantiate(healingObj,castPlayer.transform.position,castPlayer.transform.rotation, castPlayer.transform);
        healingPc = healing.GetComponent<HealingParticle>();
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
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnSuccessfullyCast()
    {
        healingPc.PlayHeal(castPlayer);
    }
}
}