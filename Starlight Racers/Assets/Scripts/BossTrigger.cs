using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public static bool bossTriggered;

    public GameObject boss;

    public GameObject bossCanvas;

    private GameObject player;
    private Vector3 playerPos;
    private Vector3 position;
    private Vector3 bossPos;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRacer");
        playerPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bossTriggered && Spacejet.readyToTriggerBoss)
        {
            StartCoroutine(SpawnBoss());
        }
    }

   

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(2);
        boss.SetActive(true);
        bossCanvas.SetActive(true);
        yield return new WaitForSeconds(5f);
        bossTriggered = true;
        GameDataManager.laserAmmoMax += 100;
    }
}
