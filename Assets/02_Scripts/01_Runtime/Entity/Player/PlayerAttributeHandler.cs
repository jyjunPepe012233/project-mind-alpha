using MinD.Structs;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerAttributeHandler : BaseEntityAttributeHandler {

    public override int MaxHp {
        get => maxHp;
        set => maxHp = value;
    }
    public override DamageNegation DamageNegation {
        get => damageNegation; 
        set => damageNegation = value;
    }
    public override int PoiseBreakResistance {
        get => poiseBreakResistance; 
        set => poiseBreakResistance = value;
    }
    
    
    public int maxHp, maxMp, maxStamina;
    public DamageNegation damageNegation;
    public int poiseBreakResistance;

    [Header("[ Stats ]")]
    [Range(0, 99)] public int vitality;
    [Range(0, 99)] public int endurance, mind, intelligence, faith;
    
    [Header("[ ]")]
    public float divine;
    public int memoryCapacity = 5;
    public float staminaRecoverySpeed = 50;
    public float staminaRecoveryDelay = 1;
    public int blinkCostStamina = 35;

    private float staminaRecoveryTimer;
    private float staminaRecoveryFloatTemp;
    


    public void SetBaseAttributesAsPerStats() {

        // Vitality
        maxHp = 100 + (vitality * 15);
        damageNegation.fire = vitality * 0.4f / 100;

        // Endurance
        maxStamina = 45 + (endurance * 2);

        // Mind
        maxMp = 100 + (mind * 2);
        damageNegation.magic = (mind * 0.25f) / 100;

        // Intelligence

        // Faith
        divine = faith * 0.04f;
    }

    public void CalculateAttributesByEquipment() {
        
        // TODO: Need Implement this method to calculating player attributes by current equipment
    }



    public void HandleStamina() {

        // FILL STAMINA
        if (((Player)owner).CurStamina < maxStamina) {

            // CHECK FLAGS AND CONTROL TIMER TO RECOVERY STAMINA
            if (!((Player)owner).isPerformingAction && !((Player)owner).locomotion.isSprinting) {

                staminaRecoveryTimer += Time.deltaTime;

            } else {
                staminaRecoveryTimer = 0;
            }


            if (staminaRecoveryTimer > staminaRecoveryDelay) {
                staminaRecoveryFloatTemp += staminaRecoverySpeed * Time.deltaTime;

                if (staminaRecoveryFloatTemp > 1) {

                    // TO ELIMINATE ERROR THAT OCCUR IN CONVERTING FLOAT TO INT
                    while (true) {
                        if (staminaRecoveryFloatTemp < 1)
                            break;

                        ((Player)owner).CurStamina += 1;
                        staminaRecoveryFloatTemp -= 1;
                    }

                }
            }
        }
    }
}

}