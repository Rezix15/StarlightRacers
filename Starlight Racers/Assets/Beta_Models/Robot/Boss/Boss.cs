using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    public GameObject normalObj;
    public GameObject angryObj;

    public GameObject[] ghostCannons;

    private bool isSafe;
    private bool isAngry;

    private bool isAttackStateOn;

    public GameObject bossPortalObj;
    public GameObject shieldEffect;

    //HP stat of the boss
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float currentHealth;

    //Shield Rate / defense stat of the boss
    [SerializeField]
    private Stat shieldRate;
    
    private bool isShielded;

    private bool hasShield;

    private bool isAtState;

    private int index;

    [SerializeField]
    private float playerLaserDamage;

    public Slider bossHpSlider;
    // Start is called before the first frame update
    void Start()
    {
        isShielded = false;
        maxHealth = 3000;
        shieldRate.baseValue = 50;
        normalObj.SetActive(true);
        index = 1;
        StartCoroutine(ToggleStates());
        currentHealth = maxHealth;
        var player = GameObject.FindGameObjectWithTag("PlayerRacer");
        playerLaserDamage = player.GetComponent<PlayerBoss>().GetCurrentLaserDamage();
        bossHpSlider.maxValue = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        // var position = transform.position;
        // transform.position = new Vector3(position.x, position.y, position.z + (500 * Time.deltaTime));
        bossHpSlider.value = currentHealth;
        SpawnCannon();
    }

    //Transition between the states (Happy and Angry)
    IEnumerator ToggleStates()
    {
        while (true)
        {
            isSafe = true;
            isAngry = false;
            yield return new WaitForSeconds(10f);
            ToggleState(0);
            yield return new WaitForSeconds(10f);
            ToggleState(1);
            index++;
        }

        //yield return new WaitForSeconds(0.1f);
    }
    
    //Attack State to spawn a random cannon through a portal to attack the player
    private void SpawnCannon()
    {
        //If in the angry state and not already in the attack state
        if (isAngry && !isAttackStateOn)
        {
            var randPortalState = Random.Range(0, 6);

            //Probability to spawn 1 portal on either the left or right side (50%)
            if (randPortalState % 5 == 0 || randPortalState % 5 == 1 || randPortalState % 5 == 2)
            {
                var position = transform.position;
                var spawnState = Random.Range(0, 2);
                var spawnPos = new Vector3(0,0,0);
                switch (spawnState)
                {
                    case 0:
                    {
                        spawnPos = new Vector3(position.x + 150, position.y, position.z);
                        break;
                    }

                    case 1:
                    {
                        spawnPos = new Vector3(position.x - 150, position.y, position.z);
                        break;
                    }
                }

                Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                
            }
            
            //Probability to spawn both portals at once (33.33%)..
            else if(randPortalState % 5 == 3 || randPortalState % 5 == 4)
            {
                var position = transform.position;
                var spawnPos1 = new Vector3(position.x + 150, position.y, position.z);
                var spawnPos2 = new Vector3(position.x - 150, position.y, position.z);
                
                Instantiate(bossPortalObj, spawnPos1, Quaternion.Euler(0,90,0));
                Instantiate(bossPortalObj, spawnPos2, Quaternion.Euler(0,90,0));
               
                
            }
            else
            {
                isAtState = false;
                LookAtPlayer();
            }
            
            //The rest of the probability (16.66%) to do nothing..
            isAttackStateOn = true;
            isAtState = true;
        }
        else if(isSafe)
        {
            var randPortalState = Random.Range(0, 4);
            
            LookAtPlayer();
            
            if (isShielded == false && randPortalState % 3 == 0)
            {
                StartCoroutine(Shield());
            }

            if (isShielded && hasShield == false)
            {
                var spawnPos = transform.position;
                var shieldObj = Instantiate(shieldEffect, spawnPos, Quaternion.Euler(90, 0, 0));
                shieldObj.transform.SetParent(transform);
                hasShield = true;
                isAtState = false;
                transform.rotation = Quaternion.identity;
                StartCoroutine(DestroyShield(shieldObj));
                
            }
        }
        
        isAtState = false;
        LookAtPlayer();
        
    }

    private void LookAtPlayer()
    {
        // var player = GameObject.FindGameObjectWithTag("PlayerRacer");
        //     
        // if (isAtState == false)
        // {
        //     transform.LookAt(transform.position - player.transform.position);
        // }
    }

    private void ToggleState(int state)
    {
        switch (state)
        {
            case 0:
            {
                normalObj.SetActive(false);
                angryObj.SetActive(true);
                isAngry = true;
                isSafe = false;
                break;
            }

            case 1:
            {
                normalObj.SetActive(true);
                angryObj.SetActive(false);
                isAttackStateOn = false;
                break;
            }
        }
    }


    IEnumerator Shield()
    {
        isAtState = true;
        for (int i = 0; i < 15; i++)
        {
            transform.Rotate(transform.up, (i * 30) * Time.deltaTime);
        }
        // for (int i = (int)transform.rotation.y; i < (int)transform.rotation.y + 360; i+=30)
        // {
        //     transform.Rotate(transform.up, i * Time.deltaTime);
        // }
        //
        yield return new WaitForSeconds(2f);
        isShielded = true;
    }

    IEnumerator DestroyShield(GameObject shieldObj)
    {
        yield return new WaitForSeconds(25f);
        Destroy(shieldObj);
        hasShield = false;
        isShielded = false;
        yield return new WaitForSeconds(10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Laser"))
        {
            if (isShielded)
            {
                var shieldBonus = shieldEffect.GetComponent<ShieldEffect>().shieldModifierBonus;
                //currentHealth -= MenuManager.currentSpaceJet.laserDamage * (1 - (shieldBonus / 10));
                currentHealth -= playerLaserDamage * (1 - (shieldBonus / 10));
            }
            else
            {
                //currentHealth -= MenuManager.currentSpaceJet.laserDamage;
                currentHealth -= playerLaserDamage;
            }

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
            
        }
    }
}
