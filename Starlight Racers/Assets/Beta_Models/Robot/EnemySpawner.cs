using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyRobot1;

    public GameObject enemyRobot2;

    private bool hasSpawned = true;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyRobot2.transform.localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var randIndex = Random.Range(0, 3);
        var position = transform.position;
        
        var enemyPos = new Vector3(position.x,position.y + 40f,position.z);
        
        if (other.CompareTag("Player"))
        {
            // if (randIndex % 3 == 1 && randIndex % 3 == 2)
            // {
            //     Instantiate(enemyRobot2, enemyPos, Quaternion.identity);
            //     //hasSpawned = false;
            // }
            if (GameObject.FindGameObjectWithTag("RobotEnemy") == null)
            {
                Instantiate(enemyRobot2, enemyPos, Quaternion.identity);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) //&& hasSpawned)
        {
            //hasSpawned = true;
        }
    }
}
