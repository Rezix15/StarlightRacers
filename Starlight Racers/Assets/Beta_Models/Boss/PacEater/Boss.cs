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
    public GameObject desperationObj;

    public GameObject[] ghostCannons;

    private bool isSafe;
    private bool isAngry;

    private bool isAttackStateOn;

    public GameObject bossPortalObj;
    public GameObject shieldEffect;

    public static bool bossReady;

    //HP stat of the boss
    [SerializeField]
    private float maxHealth;
    
    public static float currentHealth;

    public float currentHealthVal;

    //Shield Rate / defense stat of the boss
    [SerializeField]
    private Stat shieldRate;
    
    private bool isShielded;

    private bool hasShield;

    private bool isAtState;

    private int index;
    private bool desperationMode;

    [SerializeField]
    private float playerLaserDamage;

    public Slider bossHpSlider;

    private int desperationIndex;

    private bool hasShifted;

    private PlayerBoss player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ToggleStates());
    }

    private void OnEnable()
    {
        SetHealth();
        desperationIndex = 3;
        isShielded = false;
        shieldRate.baseValue = 50;
        normalObj.SetActive(true);
        index = 1;
        var position = transform.position;
        player = GameObject.FindGameObjectWithTag("PlayerRacer").GetComponent<PlayerBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        // var position = transform.position;
        // transform.position = new Vector3(position.x, position.y, position.z + (500 * Time.deltaTime));
        currentHealthVal = currentHealth;
        bossHpSlider.value = currentHealth;
        SpawnCannon();
    }

    IEnumerator HealthAppear()
    {
        while (currentHealth <= maxHealth)
        {
            currentHealth += 500;
            bossHpSlider.value = currentHealth;
            yield return new WaitForSeconds(0.1f);
        }

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            bossReady = true;
            playerLaserDamage = player.GetCurrentLaserDamage();
        }
    }
    
    void SetHealth()
    {
        switch (MenuManager.difficultyLevel)
        {
            case 0:
            {
                maxHealth = 4000;
                bossHpSlider.maxValue = maxHealth;
                StartCoroutine(HealthAppear());
                break;
            }

            case 1:
            {
                Debug.Log("Normal: " + currentHealth);
                maxHealth = 6000;
                bossHpSlider.maxValue = maxHealth;
                StartCoroutine(HealthAppear());
                break;
            }

            case 2:
            {
                maxHealth = 9000;
                bossHpSlider.maxValue = maxHealth;
                StartCoroutine(HealthAppear());
                break;
            }

            default:
            {
                maxHealth = 6000;
                bossHpSlider.maxValue = maxHealth;
                StartCoroutine(HealthAppear());;
                break;
            }
        }
    }

    //Transition between the states (Happy and Angry)
    IEnumerator ToggleStates()
    {
        while (true)
        {
            
            if (desperationIndex != 1)
            {
                isSafe = true;
                isAngry = false;
                yield return new WaitForSeconds(4f);
                ToggleState(0);
                yield return new WaitForSeconds(4f);
                ToggleState(1);
                index++;
            }
            else
            {
                isAngry = false;
                isAttackStateOn = false;
                yield return new WaitForSeconds(4f);
                ToggleState(2);
                isAttackStateOn = true;
                yield return new WaitForSeconds(4f);
            }
            
            
        }

        //yield return new WaitForSeconds(0.1f);
    }
    
    //Attack State to spawn a random cannon through a portal to attack the player
    private void SpawnCannon()
    {
        var desperationChance = Random.Range(0, desperationIndex);
        //If in the angry state and not already in the attack state
        if (bossReady)
        { 
            if (isAngry && !isAttackStateOn)
            {
                var randPortalState = Random.Range(0, 6);

                //Probability to spawn 1 portal on either the left or right side (50%)
                if (randPortalState is 0 or 1 or 2)
                {
                    var position = transform.position;
                    var spawnState = Random.Range(0, 3);
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

                        case 2:
                        {
                            spawnPos = new Vector3(position.x, position.y, position.z - 20);
                            break;
                        }
                    }

                    if (desperationMode && desperationChance == desperationIndex - 1)
                    {
                        StartCoroutine(SpawnCannon2(spawnPos, 1));
                    }
                    else
                    {
                        Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    }
                }
            
                //Probability to spawn both portals at once (33.33%)..
                else if(randPortalState is 3 or 4)
                {
                    var position = transform.position;
                    var spawnPos = new Vector3(position.x, position.y, position.z - 20);
                    var spawnPos1 = new Vector3(position.x + 150, position.y, position.z);
                    var spawnPos2 = new Vector3(position.x - 150, position.y, position.z);
                    var spawnPos3 = new Vector3(position.x + 75, position.y, position.z + 75);
                    var spawnPos4 = new Vector3(position.x - 75, position.y, position.z + 75);
                
                
                    Instantiate(bossPortalObj, spawnPos1, Quaternion.Euler(0,90,0));
                    Instantiate(bossPortalObj, spawnPos2, Quaternion.Euler(0,90,0));
                
                    if (desperationMode && desperationChance == desperationIndex - 1)
                    {
                        StartCoroutine(WaitShoot(2f, spawnPos3, spawnPos4));
                    }
                    else
                    {
                        StartCoroutine(WaitShoot(2f, spawnPos));
                    }
                }
                else
                {
                    if (desperationMode && desperationChance == desperationIndex - 1)
                    {
                        var spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 20);
                        StartCoroutine(SpawnCannon2(spawnPos, 3f));
                    }
                
                    isAtState = false;
                }
            
                //The rest of the probability (16.66%) to do nothing..
                isAttackStateOn = true;
                isAtState = true;
            
            }
            else if(isSafe)
            {
                var randPortalState = Random.Range(0, 4);
            
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
            
        }
        
    }

    IEnumerator WaitShoot(float waitTime, Vector3 position)
    {
        yield return new WaitForSeconds(waitTime);
        Instantiate(bossPortalObj, position, Quaternion.Euler(0,90,0));
        yield return new WaitForSeconds(waitTime - 2);
    }

    IEnumerator WaitShoot(float waitTime, Vector3 position1, Vector3 position2)
    {
        var position = transform.position;
        var spawnPos = new Vector3(position.x, position.y, position.z - 20);
        yield return new WaitForSeconds(waitTime);
        Instantiate(bossPortalObj, position1, Quaternion.Euler(0,90,0));
        Instantiate(bossPortalObj, position2, Quaternion.Euler(0,90,0));
        yield return new WaitForSeconds(waitTime - 2);
        Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
        yield return new WaitForSeconds(waitTime + 2);
    }

    
    //Attack state that happens when under 50 health
    IEnumerator SpawnCannon2(Vector3 initialPosition, float waitTime)
    {
        var position = transform.position;
        var spawnPos = new Vector3(position.x, position.y, position.z);
        var indexOrder = Random.Range(0, 2);
        var offset = 0;

        if (desperationObj.activeSelf)
        {
            offset = 2;
        }
        yield return new WaitForSeconds(waitTime + offset);
        switch (initialPosition.x)
        {
            case 0:
            {
                if (indexOrder == 0)
                {
                    Instantiate(bossPortalObj, initialPosition, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x - 150, position.y, position.z);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x + 150, position.y, position.z);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    Instantiate(bossPortalObj, initialPosition, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x + 150, position.y, position.z);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x - 150, position.y, position.z);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                }
                
                break;
            }

            case -150:
            {
                if (indexOrder == 0)
                {
                    Instantiate(bossPortalObj, initialPosition, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x + 150, position.y, position.z);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x, position.y, position.z - 20);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    Instantiate(bossPortalObj, initialPosition, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x, position.y, position.z);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x + 150, position.y, position.z - 20);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                }
                
                break;
            }

            case 150:
            {
                if (indexOrder == 0)
                {
                    Instantiate(bossPortalObj, initialPosition, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x - 150, position.y, position.z);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x, position.y, position.z - 20);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    Instantiate(bossPortalObj, initialPosition, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x, position.y, position.z);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                    spawnPos = new Vector3(position.x - 150, position.y, position.z - 20);
                    Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
                    yield return new WaitForSeconds(0.5f);
                }
                
                break;
            }
        }
        
    }
    
    private void ShiftAngle(float angleVal)
    {
        transform.rotation = Quaternion.Euler(angleVal, 0, 0);
    }

    private void ToggleState(int state)
    {
        switch (state)
        {
            case 0:
            {
                if (desperationIndex != 1)
                {
                    normalObj.SetActive(false);
                    angryObj.SetActive(true);
                    isAngry = true;
                    isSafe = false;
                }
                else
                {
                    ToggleState(2);
                }
                break;
            }
            case 1:
            {
                if (desperationIndex != 1)
                {
                    normalObj.SetActive(true);
                    angryObj.SetActive(false);
                    isAttackStateOn = false;
                }
                else
                {
                    ToggleState(2);
                }
                break;
            }
            case 2:
            {
                transform.rotation = Quaternion.identity;
                normalObj.SetActive(false);
                angryObj.SetActive(false);
                desperationObj.SetActive(true);
                isAngry = true;
                isSafe = false;
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
        if (bossReady)
        {
            if (other.transform.CompareTag("Laser"))
            {
                if (isShielded)
                {
                    var shieldBonus = shieldEffect.GetComponent<EnemyShieldEffect>().shieldModifierBonus;
                    
                    if (desperationIndex > 1)
                    {
                        if (desperationMode)
                        {
                            currentHealth -= (playerLaserDamage * 0.7f * (shieldBonus - 1.4f));
                        }
                        else
                        {
                            currentHealth -= (playerLaserDamage * 0.7f * shieldBonus);
                        }
                    }
                }
                else
                {
                    if (desperationIndex > 1)
                    {
                        currentHealth -= (playerLaserDamage * 0.7f);
                    }
                }
                if ((currentHealth / maxHealth) <= 0.8 && (currentHealth / maxHealth) > 0.6f)
                {
                    desperationMode = true;
                }
                if((currentHealth / maxHealth) <= 0.6 && (currentHealth / maxHealth) > 0.3f)
                {
                    desperationIndex = 2;
                }
                else if((currentHealth / maxHealth) <= 0.3)
                {
                    desperationIndex = 1;
                    ToggleState(2);
                }
                if (currentHealth <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        
        if (other.gameObject.CompareTag("DownTrack"))
        {
            ShiftAngle(45);
            hasShifted = true;
        }
        
        if (other.gameObject.CompareTag("ForwardTrack"))
        {
            ShiftAngle(0);
            hasShifted = false;
        }
    }

    public float ReturnHealthRatio()
    {
        return currentHealth / maxHealth;
    }
    
    
}
