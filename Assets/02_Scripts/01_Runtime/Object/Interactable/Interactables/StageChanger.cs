using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.Runtime.Object.Interactables
{

	public class StageChanger : Interactable
	{
		[SerializeField] private string m_targetStage;
		
		public override void Interact(Player interactor)
		{
			StageManager.Instance.ChangeStage(m_targetStage);
			
			canInteraction = false;
			interactor.interaction.RemoveInteractableInList(this);
			interactor.interaction.RefreshInteractableList();
		}
	}

}