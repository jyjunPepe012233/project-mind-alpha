using MinD.Enums;
using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.StatusFX {

public abstract class StackingEffect : MonoBehaviour {

    public StackingEffectType enumId;

    public float lifeTimeAfterFulled;

    [HideInInspector] public float currentStack;
    [HideInInspector] public bool isFulled;

    private BaseEntity owner;


    public void OnInstantiate(BaseEntity owner) {

        if (owner is Player player)
            OnInstantiateAs(player);

        if (owner is Enemy enemy)
            OnInstantiateAs(enemy);
    }

    protected abstract void OnInstantiateAs(Player target);
    protected abstract void OnInstantiateAs(Enemy target);


    public void Execute() {

        if (owner is Player player)
            ExecuteAs(player);

        if (owner is Enemy enemy)
            ExecuteAs(enemy);
    }

    protected abstract void ExecuteAs(Player target);
    protected abstract void ExecuteAs(Enemy target);


    public void OnFulled() {
        isFulled = true;

        if (owner is Player player)
            OnFulledAs(player);

        if (owner is Enemy enemy)
            OnFulledAs(enemy);
    }

    protected abstract void OnFulledAs(Player target);
    protected abstract void OnFulledAs(Enemy target);


    public void OnRemove() {

        if (owner is Player player)
            OnRemoveAs(player);

        if (owner is Enemy enemy)
            OnRemoveAs(enemy);
    }

    protected abstract void OnRemoveAs(Player target);
    protected abstract void OnRemoveAs(Enemy target);
}

}