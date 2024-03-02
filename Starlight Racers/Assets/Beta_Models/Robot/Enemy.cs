using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform laserCannon;

    public GameObject lasers;

    public float maxHealth;

    [SerializeField]
    private float currentHealth;
    
    public GameObject shieldEffect;

    private bool isShielded;
    
    // Start is called before the first frame update
    void Start()
    {
        var scaleFactor = 100;
        currentHealth = maxHealth;
        lasers.transform.localScale = new Vector3(0.05f * scaleFactor, 0.1f * scaleFactor, 0.05f * scaleFactor);
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        ShootLaser();
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Laser"))
        {
            if (isShielded)
            {
                var shieldBonus = shieldEffect.GetComponent<ShieldEffect>().shieldModifierBonus;
                currentHealth -= MenuManager.currentSpaceJet.laserDamage * (1 - (shieldBonus / 10));
            }
            else
            {
                currentHealth -= MenuManager.currentSpaceJet.laserDamage;
            }
            
            var player = GameObject.FindGameObjectWithTag("PlayerRacer");
            var racer = player.GetComponent<Spacejet>();
            
            // if (other.transform.parent == racer.transform)
            // {
            //     Debug.Log("PlayerShot");
            //     currentHealth -= MenuManager.currentSpaceJet.laserDamage;
            // }
            // else
            // {
                //Debug.Log("Not Player");
                
            //}
        }
    }

    private void ShootLaser()
    {
        var player = GameObject.FindGameObjectWithTag("PlayerRacer");

        var position = player.transform.position;

        var playerDistance = new Vector3(position.x, transform.position.y, position.z);
        
        // Debug.Log("Distance to player"+ Vector3.Distance(playerDistance, transform.position));
        if (Mathf.Abs(Vector3.Distance(playerDistance, transform.position)) < 1000)
        {
            //transform.LookAt(-player.transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * 4);

            if (!isShielded)
            {
                StartCoroutine(SpawnShield());
            }
            // var offset = new Vector3(0, 0, - transform.localScale.z * 20);
            // if (GameObject.FindGameObjectsWithTag("EnemyLaser").Length < 3)
            // {
            //     var laser = Instantiate(lasers, laserCannon.position + offset, Quaternion.Euler(90, 0, 0));
            //     var laserRb = laser.gameObject.GetComponent<Rigidbody>();
            //     laserRb.AddForce(transform.forward * 5, ForceMode.Impulse);
            //     StartCoroutine(DestroyLaser(laser));
            // }
        }
        
        
    }

    IEnumerator DestroyLaser(GameObject laser)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(laser);
    }

    
    IEnumerator SpawnShield()
    {
        //Spin Animation
        for (int i = (int)transform.rotation.y; i < (int)transform.rotation.y + 360; i+=30)
        {
            transform.Rotate(transform.up, i * Time.deltaTime);
        }
        
        var spawnPos = transform.position;
        var shieldObj = Instantiate(shieldEffect, spawnPos, Quaternion.Euler(90, 0, 0));
        shieldObj.transform.SetParent(transform);
        isShielded = true;
        yield return new WaitForSeconds(2f);
    }
    
    
}
