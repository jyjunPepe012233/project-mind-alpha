using System.Collections;
using System.Numerics;
using MinD.Runtime.Entity;
using MinD.Runtime.UI;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using NotImplementedException = System.NotImplementedException;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace MinD.Runtime.Managers {

public class PlayerHUDManager : Singleton<PlayerHUDManager> {

	private const float TIME_menuDirectionMinCool = 0.1f;
	
	
	public static PlayerHUD playerHUD {
		get {
			if (playerHUD_ == null) {
				playerHUD_ = FindObjectOfType<PlayerHUD>();
			}
			return playerHUD_;
		}
	}
	private static PlayerHUD playerHUD_;

	private Player player => Player.player;

	private bool _isLockOnSpotEnable;
	private bool _isPlayingBurstPopup;
	private bool _isFadingWithBlack;
	private float _menuDirectionCoolDown;
	
	private Coroutine fadingBlackScreenCoroutine;

	public PlayerMenu currentShowingMenu;


	protected override void OnSceneChanged(Scene oldScee, Scene newScene) {
		_isLockOnSpotEnable = false;
		_isPlayingBurstPopup = false;
		_isFadingWithBlack = false;
		fadingBlackScreenCoroutine = null;
	}

	public void Update() {

		HandleStatusBar();
		HandleLockOnSpot();
		HandleMenuInput();
		HandlePauseMenuInput();
	}

	private void HandlePauseMenuInput()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			playerHUD.tutorialPopupController.ShowPopup("제목", "내용", 5f);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (currentShowingMenu != null)
			{
				CloseMenu(playerHUD.pauseMenu);
			}
			else
			{
				OpenMenu(playerHUD.pauseMenu);
			}
		}
	}
	
	
	private void HandleStatusBar() {

		playerHUD.hpBar.HandleTrailFollowing();
		playerHUD.mpBar.HandleTrailFollowing();
		playerHUD.staminaBar.HandleTrailFollowing();

	}

	private void HandleLockOnSpot() {
		if (!_isLockOnSpotEnable) {
			return;
		}

		playerHUD.lockOnSpot.position = player.camera.camera.WorldToScreenPoint(player.camera.currentTargetOption.position);
		if (Vector3.Angle(player.camera.transform.forward, player.camera.currentTargetOption.position - player.camera.transform.position) > 90) {
			playerHUD.lockOnSpot.gameObject.SetActive(false);
		}
	}

	private void HandleMenuInput() {
		
		// MENU QUIT INPUT
		if (PlayerInputManager.Instance.menuQuitInput) {
			
			if (currentShowingMenu != null) {
				currentShowingMenu.OnQuitInput();
			}
			PlayerInputManager.Instance.menuQuitInput = false;
		}
		
		// MENU SELECT INPUT
		if (PlayerInputManager.Instance.menuSelectInput) {
			
			if (currentShowingMenu != null) {
				currentShowingMenu.OnSelectInput();
			}
			PlayerInputManager.Instance.menuSelectInput = false;
		}
		
		
		// MENU DIRECTION INPUT
		Vector2 dirxInput = PlayerInputManager.Instance.menuDirectionInput;
		if (dirxInput.magnitude != 0) {
			if (currentShowingMenu != null && _menuDirectionCoolDown <= 0) {
				while (_menuDirectionCoolDown <= 0) {
					currentShowingMenu.OnInputWithDirection(dirxInput);
					_menuDirectionCoolDown += TIME_menuDirectionMinCool / Mathf.Clamp(dirxInput.magnitude, 0.5f, 1f);
				}
			} else {
				_menuDirectionCoolDown -= Time.deltaTime;
			}
			
		} else {
			_menuDirectionCoolDown = 0;
		}
		
		
		// Moving Menu Tab Input
		int moveTabInput = PlayerInputManager.Instance.moveMenuTabInput;
		if (moveTabInput != 0 && currentShowingMenu != null) {
			currentShowingMenu.OnMoveTabInput(moveTabInput);
		} 
		PlayerInputManager.Instance.moveMenuTabInput = 0;
		
	}
	
	
	
	public void RefreshAllStatusBar() {
		RefreshHPBar();
		RefreshMPBar();
		RefreshStaminaBar();
	}
	
	public void RefreshHPBar() {
		playerHUD.hpBar.SetMaxValue(player.attribute.maxHp);
		playerHUD.hpBar.SetValue(player.CurHp);
	}
	public void RefreshMPBar() {
		playerHUD.mpBar.SetMaxValue(player.attribute.maxMp);
		playerHUD.mpBar.SetValue(player.CurMp);
	}
	public void RefreshStaminaBar() {
		playerHUD.staminaBar.SetMaxValue(player.attribute.maxStamina);
		playerHUD.staminaBar.SetValue(player.CurStamina);
	}

	
	
	public void SetLockOnSpotActive(bool value) {
		_isLockOnSpotEnable = value;
		playerHUD.lockOnSpot.gameObject.SetActive(value);
	}



	public void PlayBurstPopup(PlayableDirector burstPopupDirector, bool playWithForce = false) {

		if (_isPlayingBurstPopup) {
			
			if (playWithForce) {
				StartCoroutine(PlayBurstPopupCoroutine(burstPopupDirector));
			} else {
				throw new UnityException("!! BURST POPUP IS ALREADY PLAYING!");
			}
			
		} else {
			StartCoroutine(PlayBurstPopupCoroutine(burstPopupDirector));
			
		}
		
	}

	private IEnumerator PlayBurstPopupCoroutine(PlayableDirector burstPopupDirector) {

		burstPopupDirector.gameObject.SetActive(true);
		burstPopupDirector.Play();

		yield return new WaitForSeconds((float)burstPopupDirector.duration);

		if (burstPopupDirector != null)
		{
			burstPopupDirector.gameObject.SetActive(false);
		}

	}



	public void FadeInToBlack(float duration) {

		if (_isFadingWithBlack) {
			StopCoroutine(fadingBlackScreenCoroutine);
		}

		fadingBlackScreenCoroutine = StartCoroutine(FadeBlackScreen(duration, true));
	}

	public void FadeOutFromBlack(float duration) {
		
		if (_isFadingWithBlack) {
			StopCoroutine(fadingBlackScreenCoroutine);
		}
		
		fadingBlackScreenCoroutine = StartCoroutine(FadeBlackScreen(duration, false));
	}

	private IEnumerator FadeBlackScreen(float duration, bool fadeDirection) {
		
		_isFadingWithBlack = true;
		playerHUD.blackScreen.gameObject.SetActive(true);
		
		playerHUD.blackScreen.color = new Color(0, 0, 0, fadeDirection ? 0 : 1);
		
		
		float elapsedTime = 0;
		while (true) {

			elapsedTime += Time.deltaTime;
			
			playerHUD.blackScreen.color = new Color(0, 0, 0, (fadeDirection ? (elapsedTime / duration) : (1-elapsedTime / duration))); 
			yield return null;

			if (elapsedTime > duration) { 
				break;
			}
		}
		
		
		_isFadingWithBlack = false;
		playerHUD.blackScreen.gameObject.SetActive(fadeDirection);
		
		playerHUD.blackScreen.color = new Color(0, 0, 0, fadeDirection ? 1 : 0);
		
	}
	
	public void PlayPopup(string message)
	{
		StopAllCoroutines();
		playerHUD.popupUIController.popupText.text = message;
		StartCoroutine(PopupRoutine());
	}

	private IEnumerator PopupRoutine()
	{
		CanvasGroup cg = playerHUD.popupUIController.popupCanvasGroup;
		GameObject go = cg.gameObject;

		float fadeDuration = 0.5f;
		float t = 0f;

		go.SetActive(true);
		yield return new WaitForSeconds(0.3f);
		
		// Fade In
		cg.alpha = 0f;
		while (t < fadeDuration)
		{
			t += Time.deltaTime;
			cg.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
			yield return null;
		}
		cg.alpha = 1f;

		// 유지
		yield return new WaitForSeconds(1f);

		// Fade Out
		t = 0f;
		while (t < fadeDuration)
		{
			t += Time.deltaTime;
			cg.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
			yield return null;
		}
		cg.alpha = 0f;
		go.SetActive(false);
	}

	public void ShowTutorialPopup(string title, string contents, float displayTime)
	{
		playerHUD.tutorialPopupController.ShowPopup(title,contents, displayTime);
		currentShowingMenu = null;
	}
	
	public void OpenMenu(PlayerMenu menu, bool openWithForce = false) {
		
		if (FadeCanvasGroup.isFading) {
			return;
		}

		if (currentShowingMenu != null) {
			
			if (!openWithForce) {
				// OTHER MENU IS ALREADY SHOWING 
				throw new UnityException("!! " + menu.name + " TRIED TO OPEN WHILE " + currentShowingMenu + " IS SHOWING");
				
			} else {
				menu.Close();
				StartCoroutine(currentShowingMenu.FadeOpenAndClose(0, false));
			}
		}
		
		StartCoroutine(menu.FadeOpenAndClose(menu.fadeInTime, true));
		menu.Open();
		
		currentShowingMenu = menu;
	}
	
	public void CloseMenu(PlayerMenu menu) {
		
		if (FadeCanvasGroup.isFading) {
			return;
		}

		if (!menu.Equals(currentShowingMenu)) {
			throw new UnityException("!! " + menu.name + " IS NOT CURRENT MENU! \n" + "CURRENT MENU IS " + currentShowingMenu.name);
		}
		
		menu.Close();
		StartCoroutine(menu.FadeOpenAndClose(menu.fadeOutTime, false));

		currentShowingMenu = null;
	}
}

}