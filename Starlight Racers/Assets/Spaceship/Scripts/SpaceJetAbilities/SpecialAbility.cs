using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
    
public class SpecialAbility 
{
    //The type of ability used
    public enum AbilityTypes
    {
        Buff,
        Effect,
        Offensive,
        Defensive
    }
    
    private string abilityName; //Name of the ability
    private float cooldownTimer; //The time needed for the ability to be used
    private AbilityTypes abilityType;

    public SpecialAbility(string abilityName, float cooldownTimer, AbilityTypes abilityType)
    {
        this.abilityName = abilityName;
        this.cooldownTimer = cooldownTimer;
        this.abilityType = abilityType;
    }

    public virtual void AbilityEffect()
    {
        
    }

    public virtual void UseAbility()
    {
        
    }
}
