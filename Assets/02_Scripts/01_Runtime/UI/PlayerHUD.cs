using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace MinD.Runtime.UI {

public class PlayerHUD : MonoBehaviour {

    [Header("[ Status Bar ]")]
    public StatusBarHUD hpBar;
    public StatusBarHUD mpBar;
    public StatusBarHUD staminaBar;

    [Header("[ Lock On Spot ]")]
    public RectTransform lockOnSpot;

    [Header("[ Burst Popup ]")]
    public PlayableDirector youDiedPopup;
    public PlayableDirector anchorDiscoveredPopup;
    
    [Header("[ Black Screen ]")]
    public Image blackScreen;


    [Header("[ Menus ]")]
    public GuffinsAnchorMenu guffinsAnchorMenu;
    public InventoryMenu inventoryMenu;
}

}