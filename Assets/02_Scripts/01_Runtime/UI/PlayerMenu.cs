using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace MinD.Runtime.UI {

public abstract class PlayerMenu : MonoBehaviour {

	public CanvasGroup group;

	[Space(10)]
	public float fadeInTime = 0.3f;
	public float fadeOutTime = 0.5f;
	
	[HideInInspector] public bool isInFade;

	
	
	public abstract void Open();
	public abstract void Close();

	public virtual void OnInputWithDirection(Vector2 inputDirx ) {
	}
	public virtual void OnSelectInput() {
	}
	public virtual void OnQuitInput() {
	}
	public virtual void OnMoveTabInput(int inputDirx) {
	}
	
	public IEnumerator FadeOpenAndClose(float duration, bool fadeDirection) {

		if (duration <= 0) {
			group.alpha = fadeDirection ? 1 : 0;
			
			gameObject.SetActive(fadeDirection);
			yield break;
		}
		
		
		
		isInFade = true;
		gameObject.SetActive(true);
		
		group.alpha = fadeDirection ? 0 : 1;
		group.interactable = false;
		
		
		float elapsedTime = 0;
		while (true) {

			elapsedTime += Time.deltaTime;
			
			group.alpha = fadeDirection ? (elapsedTime / duration) : (1-elapsedTime / duration); 
			yield return null;

			if (elapsedTime > duration) { 
				break;
			}
		}
		
		
		group.alpha = fadeDirection ? 1 : 0;
		group.interactable = true;
		
		isInFade = false;
		gameObject.SetActive(fadeDirection);
	}
}

}