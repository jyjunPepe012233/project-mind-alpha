using System;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.Object;
using UnityEngine;

public class GimmickTorch : Interactable
{
    public event Action activated;
    [SerializeField] private ParticleSystem particleOnActivated;

    private bool isActivated;

    private void Start()
    {
//        activated?.Invoke(); // event가 잘 호출됨
    }
    
    public override void Interact(Player interactor)
    {
        interactor.animation.PlayTargetAction("Anchor_Discover", 0.2f, true, true, false, false, false);
        
        activated?.Invoke(); // event가 호출되지 않음
        
        isActivated = true;
        
        particleOnActivated?.Play(); 

        canInteraction = false;
        interactor.interaction.RemoveInteractableInList(this);
        interactor.interaction.RefreshInteractableList();
        
        PlayerHUDManager.playerHUD.tutorialPopupController.SetTutorial(4);
    }
}
