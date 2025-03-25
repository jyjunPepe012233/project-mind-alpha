using System.Collections.Generic;
using MinD.Runtime.Managers;
using MinD.Runtime.Object;
using MinD.Runtime.UI;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerInteractionHandler : EntityOwnedHandler {

	public List<Interactable> currentInteractables = new List<Interactable>();
	private InteractionPanelController panelCtrl;


	public void AddInteractableInList(Interactable interactable) {

		if (!currentInteractables.Contains(interactable))
			currentInteractables.Add(interactable);
	}

	public void RemoveInteractableInList(Interactable interactable) {

		if (currentInteractables.Contains(interactable))
			currentInteractables.Remove(interactable);
	}

	public void RefreshInteractableList() {

		for (int i = currentInteractables.Count - 1; i < -1; i--) /*REVERSE FOR*/ {

			Interactable interactable = currentInteractables[i];

			// IS INTERACTION IS DESTROYED
			// OR INTERACTION CAN'T INTERACTION BY PARAMETER
			if (interactable == null || !interactable.canInteraction)
				currentInteractables.Remove(interactable);

			if (currentInteractables.Count == 0)
				break;
		}

		// refresh popup
		if (panelCtrl == null)
		{
			panelCtrl = FindObjectOfType<InteractionPanelController>();
		}
		panelCtrl.RefreshInteractionPanel();
	}

	public void HandleInteraction() {

		if (owner.isDeath)
			return;

		// CHECK INPUT
		if (!PlayerInputManager.Instance.interactionInput)
			return;
		PlayerInputManager.Instance.interactionInput = false;

		
		if (currentInteractables.Count == 0)
			return;

		if (currentInteractables[0] == null)
			return;

		if (currentInteractables[0].canInteraction)
			currentInteractables[0].Interact(((Player)owner));
		else
			RefreshInteractableList();

	}
}

}