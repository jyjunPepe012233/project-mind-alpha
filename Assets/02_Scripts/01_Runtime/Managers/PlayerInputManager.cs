using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MinD.Runtime.Managers {

public class PlayerInputManager : Singleton<PlayerInputManager> {

    private PlayerControls playerControls;

    // LOCOMOTION
    public Vector2 movementInput;
    public bool jumpInput;
    public bool sprintInput;
    private bool blinkInput; // HANDLING IN METHOD IN THIS MANAGER

    // CAMERA CONTROL
    public Vector2 rotationInput;
    public bool lockOnInput;

    // INTERACTION
    public bool interactionInput;

    // COMBAT
    public bool useMagicInput;
    public int swapMagicInput;
    public bool useToolInput;
    public int swapToolInput;
    public bool defenseMagicInput;
        // LEFT MAGIC TO -1, RIGHT MAGIC TO 1
        // IF MAGIC IS SWAPPED, RESET TO 0
        
    // MENU CONTROL
    public bool menuQuitInput;
    public bool menuSelectInput;
    public Vector2 menuDirectionInput;
    public int moveMenuTabInput;
    public bool openInventoryInput;


    // on scene changed, Check the scene is world scene
    // if scene is not a world scene, disable the input


    private void OnEnable() {

        if (playerControls == null) {

            playerControls = new PlayerControls();
            playerControls.Enable();
            
            // LOCOMOTION
            playerControls.Locomotion.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                
            playerControls.Locomotion.Jump.performed += i => jumpInput = true; // IF INPUT IS PERFORMED, SET BOOL TO TRUE

            playerControls.Locomotion.Space_Sprint.performed += i => sprintInput = true;
            playerControls.Locomotion.Space_Sprint.canceled += i => sprintInput = false;
            
            playerControls.Locomotion.Space_Blink.started += i => blinkInput = true;
            playerControls.Locomotion.Space_Blink.performed += i => blinkInput = false; // WHEN ELAPSE HOLD TIME
            playerControls.Locomotion.Space_Blink.canceled += AttemptCallBlink;
            
            
            // CAMERA CONTROL
            playerControls.CameraControl.Rotation.performed += i => rotationInput = i.ReadValue<Vector2>();
            playerControls.CameraControl.LockOn.performed += i => lockOnInput = true; // IF INPUT IS PERFORMED, SET BOOL TO TRUE
            
            
            // INTERACTION
            playerControls.Interaction.Interaction.performed += i => interactionInput = true;
            
            
            // COMBAT
            playerControls.Combat.UseMagic.started += i => useMagicInput = true;
            playerControls.Combat.UseMagic.canceled += i => useMagicInput = false;
            
            playerControls.Combat.SwapMagic.started += i => swapMagicInput = (int)(i.ReadValue<float>());

            playerControls.Combat.UseTool.started += i => useToolInput = true;
            playerControls.Combat.UseTool.canceled += i => useToolInput = false;
            
            playerControls.Combat.SwapTool.started += i => swapToolInput = (int)(i.ReadValue<float>());
            
            playerControls.Combat.DefenseMagic.started += i => defenseMagicInput = true;
            playerControls.Combat.DefenseMagic.canceled += i => defenseMagicInput = false;
            
            
            // MENU CONTROL
            playerControls.MenuControl.MenuQuitInput.started += i => menuQuitInput = true;
            playerControls.MenuControl.MenuSelectInput.started += i => menuSelectInput = true;
            playerControls.MenuControl.MenuDirxInput.performed += i => menuDirectionInput = i.ReadValue<Vector2>();
            playerControls.MenuControl.MoveMenuTab.started += i => moveMenuTabInput = (int)i.ReadValue<float>();
            
            playerControls.MenuControl.OpenInventory.started += i => openInventoryInput = true;


        }
    }

    void AttemptCallBlink(InputAction.CallbackContext callbackContext) {

        if (blinkInput) {
            // WASN'T ELAPSE HOLD TIME
            
            Player.player.locomotion.AttemptBlink();
        }

    }

}

}