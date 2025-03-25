using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Managers;
using MinD.Runtime.Object.Interactables;
using MinD.Utility;
using MinD.SO.StatusFX.Effects;
using UnityEngine;
using UnityEngine.AI;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(PlayerLocomotionHandler))]
[RequireComponent(typeof(PlayerAnimationHandler))]
[RequireComponent(typeof(PlayerAttributeHandler))]
[RequireComponent(typeof(PlayerInventoryHandler))]
[RequireComponent(typeof(PlayerEquipmentHandler))]
[RequireComponent(typeof(PlayerInteractionHandler))]
[RequireComponent(typeof(PlayerCombatHandler))]
public class Player : BaseEntity {
    
    public static Player player {
        get {
            if (_player == null) {
                _player = FindObjectOfType<Player>();
            }
            return _player;
        }
    }
    private static Player _player;
    
    [HideInInspector] public PlayerCamera camera;
    [HideInInspector] public PlayerDefenseMagic defenseMagic;
    [HideInInspector] public PlayerAnimationHandler animation;
    [HideInInspector] public PlayerAttributeHandler attribute;
    [HideInInspector] public PlayerLocomotionHandler locomotion;
    [HideInInspector] public PlayerInventoryHandler inventory;
    [HideInInspector] public PlayerEquipmentHandler equipment;
    [HideInInspector] public PlayerInteractionHandler interaction;
    [HideInInspector] public PlayerCombatHandler combat;
    
    public override int CurHp {
        get => curHp;
        set {
            curHp = value;
            if (curHp <= 0) {
                OnDeath();
            }
            curHp = Mathf.Clamp(curHp, 0, attribute.MaxHp);
            PlayerHUDManager.Instance.RefreshHPBar();
        }
    }
    
    [SerializeField] private int curMp;
    [SerializeField] private int curStamina;
    public int CurMp {
        get => curMp;
        set {
            curMp = value;
            curMp = Mathf.Clamp(curMp, 0, attribute.maxMp);
            PlayerHUDManager.Instance.RefreshMPBar();
        }
    }
    public int CurStamina {
        get => curStamina;
        set {
            curStamina = value;
            curStamina = Mathf.Clamp(curStamina, 0, attribute.maxStamina);
            PlayerHUDManager.Instance.RefreshStaminaBar();
        }
    }


    [Header("Flags")]
    public bool isPerformingAction;
    public bool isGrounded;
    public bool isMoving;
    public bool isLockOn;
    public bool canRotate;
    public bool canMove;
    


    protected override void Setup() {

        base.Setup();
        
        animation = GetComponent<PlayerAnimationHandler>();
        attribute = GetComponent<PlayerAttributeHandler>();
        locomotion = GetComponent<PlayerLocomotionHandler>();
        inventory = GetComponent<PlayerInventoryHandler>();
        equipment = GetComponent<PlayerEquipmentHandler>();
        interaction = GetComponent<PlayerInteractionHandler>();
        combat = GetComponent<PlayerCombatHandler>();
        
        camera = FindObjectOfType<PlayerCamera>();
        camera.owner = this;
        defenseMagic = FindObjectOfType<PlayerDefenseMagic>();
        defenseMagic.owner = this;
        PhysicUtility.IgnoreCollisionUtil(this, defenseMagic.defenseCollider);
    }
    
    
    
    public void LoadData() { // TODO: Need getting save data instance to parameter(Currently, method only work as refreshing method)

        if (!hasBeenSetup) {
            Setup();
        }
        
        // TODO: Load(Apply to save data) Attribute Data(MaxHp, DamageNegation.. etc.)
        attribute.SetBaseAttributesAsPerStats();
        attribute.CalculateAttributesByEquipment();
        PlayerHUDManager.Instance.RefreshAllStatusBar();
        


        
        // TODO: Load camera direction
        #region LOAD_CHARACTER_TRANSFORM_DATA
        Vector3 playerPosition = default;
        Vector3 playerDirx = default;
        
        if (WorldDataManager.Instance.GetDiscoveredGuffinsAnchorCount() > 0 && GameManager.Instance.willAwakeFromLatestAnchor) {
            GuffinsAnchor latestAnchor = WorldDataManager.Instance.GetGuffinsAnchorInstanceToId(WorldDataManager.Instance.latestUsedAnchorId);
            if (NavMesh.SamplePosition(latestAnchor.transform.TransformPoint(GuffinsAnchor.playerPosition), out NavMeshHit hitInfo, 1.5f, NavMesh.AllAreas)) {
                playerPosition = hitInfo.position;
                playerDirx = latestAnchor.transform.position - playerPosition;
                playerDirx.y = 0; 
            }
        } else {
            // TODO: Temp.  
            playerPosition = transform.position;
            playerDirx = transform.forward;
        }
        
        // Disable character controller to setting position by transform operation 
        cc.enabled = false;
        transform.position = playerPosition;
        cc.enabled = true;
        
        transform.forward = playerDirx;
        #endregion
        
        
        if (GameManager.Instance.willAwakeWithAnchorIdle) {
            animation.PlayTargetAction("Anchor_Idle", 0, true, true, false, false);
            
        } else if (GameManager.Instance.willAwakeFromLatestAnchor) {
            animation.PlayTargetAction("Anchor_End", 0, true, true, false, false);
        }
        
    }
    
    
    protected override void Update() {
        
        base.Update();
        
        attribute.HandleStamina();

        camera.HandleCamera();
        locomotion.HandleAllLocomotion();
        animation.HandleAllParameter();
        inventory.HandleInventoryOpen();
        inventory.HandleQuickSlotSwapping();
        interaction.HandleInteraction();
        combat.HandleAllCombatAction();

    }

    public override void OnDamaged(TakeHealthDamage damage) {
        // HANDLE POISE BREAK AND CANCELING ACTION
		
        // IF PLAYER HAS IMMUNE OF POISE BREAK, DON'T GIVE POISE BREAK
        // AND PLAYER IS DIED AFTER DRAIN HP, DON'T GIVE POISE BREAK
        if (immunePoiseBreak || isDeath) {
            return;
        }

        // CANCEL ACTIONS
        combat.CancelMagicOnGetHit();
        locomotion.CancelBlink();
        
        // PLAY POISE BREAK ANIMATION
        int poiseBreakAmount = TakeHealthDamage.GetPoiseBreakAmount(damage.poiseBreakDamage, attribute.PoiseBreakResistance);
        animation.PlayTargetAction(animation.GetPoiseBreakAnimation(poiseBreakAmount, damage.attackAngle), true, true, false, false);
        
    }

    protected override void OnDeath() {
        isDeath = true;
        
        // CANCEL ACTIONS
        combat.CancelMagicOnGetHit();
        locomotion.CancelBlink();
        
        PlayerHUDManager.Instance.PlayBurstPopup(PlayerHUDManager.playerHUD.youDiedPopup, true);
        
        animation.PlayTargetAction("Death", 0.2f, true, true, false, false, false);

        GameManager.Instance.StartReloadWorldCauseDeath(4);
    }
}

}