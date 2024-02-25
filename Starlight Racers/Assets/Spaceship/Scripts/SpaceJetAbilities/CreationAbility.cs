using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Special Ability to be used by Absorber
public class CreationAbility : SpecialAbility
{
    private GameObject shieldEffect;
    private string abilityName;
    private float cooldownTimer;
    private AbilityTypes AbilityType;
    private GameObject bomb;

    private int randIndex; //Random Value that dictates which effect is spawned
    
    public CreationAbility(string abilityName, float cooldownTimer, AbilityTypes abilityType, GameObject shieldEffect, GameObject bomb) : base(abilityName, cooldownTimer, abilityType)
    {
        this.abilityName = abilityName;
        this.AbilityType = abilityType;
        this.cooldownTimer = cooldownTimer;
        this.shieldEffect = shieldEffect;
        this.bomb = bomb;
    }
    
    //Ability that creates uses materials found in the race to create weapons and buffs
    public override void AbilityEffect()
    {
        randIndex = Random.Range(0, 3);

        switch (randIndex)
        {
            
        }
    }

    public override void UseAbility()
    {
        
    }
    
    
}
