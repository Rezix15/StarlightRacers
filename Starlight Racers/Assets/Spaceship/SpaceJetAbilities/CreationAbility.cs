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

    //private bool isAbilityActive;
    
    public void Initialize(string powerName, float cooldownTime, AbilityTypes abilityType, GameObject shieldEffectObj, GameObject bombObj)
    {
        this.abilityName = powerName;
        this.AbilityType = abilityType;
        this.cooldownTimer = cooldownTime;
        this.shieldEffect = shieldEffectObj;
        this.bomb = bombObj;
    }
    
    public CreationAbility(string abilityName, float cooldownTimer, AbilityTypes abilityType, GameObject shieldEffect, GameObject bomb) : base(abilityName, cooldownTimer, abilityType)
    {
        
        Initialize(abilityName, cooldownTimer, abilityType, shieldEffect, bomb);
    }
    
    //Ability that creates uses materials found in the race to create weapons and buffs
    public override void AbilityEffect()
    {
        randIndex = Random.Range(0, 2);
        var player = GameObject.FindGameObjectWithTag("PlayerRacer");
        var playerPos = player.transform.position;

        var spawnPos = new Vector3(playerPos.x, playerPos.y + 2, playerPos.z);
        var bombSpawnPos = new Vector3(playerPos.x, playerPos.y, playerPos.z - (20 + (bomb.transform.localScale.x / 30 )) );
        
        switch (randIndex)
        {
            //Generate shield
            case 0:
            {
                var shieldObj = Instantiate(shieldEffect, spawnPos, Quaternion.Euler(90, 0, 0));
                shieldObj.transform.SetParent(player.transform);
                StartCoroutine(StartTimer(shieldObj, 1));
                break;
            }

            //Generate bomb
            case 1:
            {
                var bombObj = Instantiate(bomb, bombSpawnPos, Quaternion.Euler(90, 0, 0));
                //StartCoroutine(BombDrops());
                StartCoroutine(StartTimer(bombObj, 3));
                break;
            }
        }
    }

    public override void UseAbility()
    {
        AbilityEffect();
    }

    IEnumerator StartTimer(GameObject obj, float rate)
    {
        yield return new WaitForSeconds(cooldownTimer / rate);
        Destroy(obj);
    }

    // IEnumerator BombDrops()
    // {
    //     for (int i = 0; i < 3; i++)
    //     {
    //         var player = GameObject.FindGameObjectWithTag("PlayerRacer");
    //         var playerPos = player.transform.position;
    //         var bombSpawnPos = new Vector3(playerPos.x, playerPos.y, playerPos.z - (20 + (bomb.transform.localScale.x / 30 )) );
    //         var bombObj = Instantiate(bomb, bombSpawnPos, Quaternion.Euler(90, 0, 0));
    //         yield return new WaitForSeconds(cooldownTimer / 3);
    //         Destroy(bombObj);
    //         yield return new WaitForSeconds(0.2f);
    //     }
    // }
    
    
}
