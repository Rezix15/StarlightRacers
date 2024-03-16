using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDesperation : MonoBehaviour
{
    private float playerLaserDamage;
    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("PlayerRacer");
        playerLaserDamage = player.GetComponent<PlayerBoss>().GetCurrentLaserDamage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Laser"))
        {
            Boss.currentHealth -= (playerLaserDamage * 0.25f);
        }
    }
}
