using System;
using MinD.Runtime.Entity;
using MinD.Runtime.Object;
using UnityEngine;

public class GimmickTorch : Interactable
{
    public event Action activated;
    [SerializeField] private ParticleSystem particleOnActivated;

    private bool isActivated;
    
    public override void Interact(Player interactor)
    {
        interactor.animation.PlayTargetAction("Anchor_Discover", 0.2f, true, true, false, false, false);
        
        if (!isActivated)
        {
            isActivated = true;
            
            activated?.Invoke();
            particleOnActivated?.Play();
        }
    }
}
