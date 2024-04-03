using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    
    private float health;

    [SerializeField]
    private float currentHealth;

    private float laserDamage;

    public GameObject onState;

    public GameObject destroyedState;
    // Start is called before the first frame update
    void Start()
    {
        health = 30;
        currentHealth = health;
        onState.SetActive(true);
        destroyedState.SetActive(false);
        laserDamage = MenuManager.currentSpaceJet.laserDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            onState.SetActive(false);
            destroyedState.SetActive(true);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {
            currentHealth -= (laserDamage / 2);
        }
    }
}
