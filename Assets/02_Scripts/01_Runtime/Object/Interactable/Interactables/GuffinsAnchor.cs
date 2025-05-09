using MinD.Interfaces;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.Object.Utils;
using MinD.SO.Object;
using UnityEngine;

namespace MinD.Runtime.Object.Interactables {

public class GuffinsAnchor : Interactable, IWorldIndexable {
	
	private const float TIME_LightFading = 1.5f;

	[SerializeField, HideInInspector] private bool _hasBeenIndexed;
	public bool hasBeenIndexed {
		get => _hasBeenIndexed;
		set => _hasBeenIndexed = value;
	}
	
	[SerializeField, HideInInspector] private int _worldIndex;
	public int worldIndex {
		get => _worldIndex;
		set => _worldIndex = value;
	}

	
	[Header("[ Anchor Setting ]")]
	public GuffinsAnchorInformation anchorInfo;
	public Transform sittingPoint;
	
	[Header("[ Owned Object ]")]
	[SerializeField] private ParticleSystem particleDiscovered;
	[SerializeField] private ParticleSystem particleNotDiscovered;
	[SerializeField] private FadingLight fadingLight;
	
	[Space(5)]
	public bool isDiscovered;
	
	

	
	public override void Interact(Player interactor) {
		
		// CHECK FLAGS TO MAKE SURE USING ANCHOR
		if (!interactor.isGrounded) {
			return;
		}
		if (interactor.isPerformingAction) {
			return;
		}

		WorldDataManager.Instance.latestUsedAnchorId = WorldDataManager.Instance.GetGuffinsAnchorIdToInstance(this);
		if (isDiscovered) {
			UseGuffinsAnchor(interactor);
		} else {
			DiscoverGuffinsAnchor(interactor);
		}
		
	}

	public void LoadData(bool isDiscovered) {
		this.isDiscovered = isDiscovered;
		
		if (isDiscovered) {
			particleDiscovered.gameObject.SetActive(true);
			fadingLight.FadeIn(0);
			particleDiscovered.Play();
		} else {
			particleNotDiscovered.gameObject.SetActive(true);
			fadingLight.FadeOut(0);
			particleNotDiscovered.Play();
		}
	}



	private void UseGuffinsAnchor(Player interactor) {
		
		// DISABLE ALL DAMAGEABLE COLLIDER IN PLAYER 
		interactor.animation.PlayTargetAction("Anchor_Start", 0.2f, true, true, false, false, false);
		
		GameManager.Instance.StartReloadWorldByGuffinsAnchor();
	}
	
	

	private void DiscoverGuffinsAnchor(Player interactor) {
		
		isDiscovered = true;
		particleDiscovered.gameObject.SetActive(true);

		fadingLight.FadeIn(TIME_LightFading);
		particleNotDiscovered.Stop();
		particleDiscovered.Play();
			
		var discoverPopup = PlayerHUDManager.playerHUD.anchorDiscoveredPopup;
		PlayerHUDManager.Instance.PlayBurstPopup(discoverPopup);
		
		interactor.animation.PlayTargetAction("Anchor_Discover", 0.2f, true, true, false, false, false);
	}
}

}