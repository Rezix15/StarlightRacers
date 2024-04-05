using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrack : MonoBehaviour
{
    public GameObject powerUpObj;
    public enum TrackDirection
    {
        Back,
        Forward,
        Initial,
        Shift
    }

    [SerializeField]
    private float trackSpeed;

    public TrackDirection trackDirection;

    private PlayerBoss player;

    private int randIndex;

    public GameObject specialObj;

    private int specialIndex;

    //private GameObject powerUps;
    // Start is called before the first frame update
    void Start()
    {
        
        if (specialObj != null)
        {
            specialObj.SetActive(false);
            
            if (BossTrackScript.spawnTrafficLights)
            {
                specialObj.SetActive(true);
                BossTrackScript.spawnTrafficLights = false;
            }
        
        }
        //powerUps = new GameObject();
        player = GameObject.FindGameObjectWithTag("PlayerRacer").GetComponent<PlayerBoss>();

        if (player != null)
        {
            trackSpeed = 25000f / 10;
        }
        
        
        //SpawnPowerUp();
        
    }

    // void SpawnPowerUp()
    // {
    //     randIndex = Random.Range(0, 4);
    //
    //     if (randIndex % 4 == 0)
    //     {
    //         powerUpObj.transform.localScale = new Vector3(30,30,30);
    //         var positionIndex = Random.Range(0, 3);
    //         
    //         var powerupPos = new Vector3(0,0f,0);
    //         powerUps = Instantiate(powerUpObj, powerupPos, Quaternion.identity);
    //         powerUps.transform.SetParent(transform);
    //
    //         switch (positionIndex)
    //         {
    //             case 0:
    //             {
    //                 powerupPos = new Vector3(0, 0.4f, 0);
    //                 break;
    //             }
    //
    //             case 1:
    //             {
    //                 powerupPos = new Vector3(-2.5f, 0.4f, 0);
    //                 break;
    //             }
    //
    //             case 2:
    //             {
    //                 powerupPos = new Vector3(2.5f, 0.4f, 0);
    //                 break;
    //             }
    //         }
    //
    //         powerUps.transform.position = powerupPos;
    //     }
    //     
    // }

    // Update is called once per frame
    void Update()
    {
        
        switch (trackDirection)
        {
            case TrackDirection.Back:
            {
                transform.position += Vector3.back * (trackSpeed * Time.deltaTime);
                break;
            }
            
            case TrackDirection.Shift:
            {
                var offsetPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                if (Vector3.Distance(player.transform.position, offsetPos) < 0.3f)
                {
                    
                }
                else
                {
                   
                }
                
                
                break;
            }

            case TrackDirection.Forward:
            {
                transform.position += Vector3.forward * (trackSpeed * Time.deltaTime);
                break;
            }

            case TrackDirection.Initial:
            {
                StartCoroutine(DestroyObj());
                break;
            }
        }
    }
    
    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        //Destroy(powerUps);
    }
}
