using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostAbility : SpecialAbility
{
    private bool isInvisible;
    
    public GhostAbility() : base("GhostRider", 30.0f, AbilityTypes.Defensive)
    {
        
    }

    public override void AbilityEffect()
    {
        isInvisible = true;
    }

    public override void UseAbility()
    {
        
    }
}
