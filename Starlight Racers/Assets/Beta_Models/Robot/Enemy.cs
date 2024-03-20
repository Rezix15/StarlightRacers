using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform laserCannon;

    public GameObject lasers;

    public float maxHealth;

    [SerializeField]
    private float currentHealth;
    
    public GameObject shieldEffect;

    private bool isShielded;

    private List<GameObject> currentCheckpoints; //List of all the checkpoints on the map
    
    private float nearestDistance = 0;

    private GameObject nearestCheckpoint;

    private GameObject endCheckpoint;

    private NavMeshAgent enemyAgent;
    
    private List<GameObject> path;

    private int currentIndex;

    private bool switchOrder;
    
    // Start is called before the first frame update
    void Start()
    {
        path = new List<GameObject>();
        nearestCheckpoint = new GameObject();
        endCheckpoint = new GameObject();
        var scaleFactor = 100;
        currentHealth = maxHealth;
        lasers.transform.localScale = new Vector3(0.05f * scaleFactor, 0.1f * scaleFactor, 0.05f * scaleFactor);
        //enemyAgent = gameObject.GetComponent<NavMeshAgent>();
        RaceManager.GameStarted += OnGameStarted;
    }

    void OnGameStarted()
    {
        //enemyAgent.enabled = true;
        // CheckNearestCheckpoint();
        // GeneratePath();
    }
    
    

    // Update is called once per frame
    void Update()
    {
        //Movement();
        CheckHealth();
        ShootLaser();
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            CoinManager.coinCount += 50;
            Destroy(gameObject);
        }
    }

    private void CheckNearestCheckpoint()
    {
        currentCheckpoints = new List<GameObject>(TrackGen.checkpoints);

        foreach (var currentCheckpoint in currentCheckpoints)
        {
            var checkpointDistance = Vector3.Distance(transform.position, currentCheckpoint.transform.position);

            if (nearestDistance < checkpointDistance)
            {
                nearestDistance = checkpointDistance;
                nearestCheckpoint = currentCheckpoint;
            }
        }
    }

    private void GenerateNavMesh()
    {
        var position = endCheckpoint.transform.position;
        var targetPos = new Vector3(position.x, transform.position.y, position.z);
        //enemyAgent.SetDestination(targetPos);
    }

    private void Movement()
    {
        if (enemyAgent.enabled)
        {
            var position = path[currentIndex].transform.position;
            var targetPos = new Vector3(position.x, transform.position.y, position.z);
            enemyAgent.SetDestination(targetPos);
            
            Debug.Log("targetPos: " + targetPos);

            if (Vector3.Distance(transform.position, targetPos) < 0.3f)
            {
                if (!switchOrder)
                {
                    currentIndex++;
                }
                else
                {
                    currentIndex--;
                }
            }

            if (currentIndex == 0)
            {
                switchOrder = false;
            }
            else if(currentIndex == path.Count-1)
            {
                switchOrder = true;
            }
        }
        
    }

    // private void GeneratePath()
    // {
    //     currentCheckpoints = new List<GameObject>(TrackGen.checkpoints);
    //     //int currentIndex = 0;
    //     var addCheckpoints = false;
    //     var trackCount = 0;
    //
    //     foreach (var currentCheckpoint in currentCheckpoints)
    //     {
    //         //currentIndex++;
    //         
    //         if (nearestCheckpoint == currentCheckpoint)
    //         {
    //             addCheckpoints = true;
    //         }
    //
    //         if (addCheckpoints && trackCount < 3)
    //         {
    //             path.Add(currentCheckpoint);
    //             trackCount++;
    //         }
    //         
    //         Debug.Log("Path Count: " + path.Count);
    //         Debug.Log("Path Nearest Checkpoint: " + nearestCheckpoint);
    //     }
    // }
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
