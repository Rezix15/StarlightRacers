using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrackScript : MonoBehaviour
{
    public GameObject movingTrack;
    private List<GameObject> currentTracks;

    public GameObject undergroundMovingTrack;
    public GameObject undergroundShift;

    private PlayerBoss player;

    private float trackSpeed;

    public static bool spawnTrafficLights;

    private bool isUnderground;

    [SerializeField] private int randIndex;

    public GameObject undergroundWall;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRacer").GetComponent<PlayerBoss>();

        
        trackSpeed = 25000f / 10;

        if (trackSpeed == 0)
        {
            trackSpeed = 250;
        }

        var position = transform.position;

        currentTracks = new List<GameObject>();
        // StartCoroutine(SpawnInitialTracks(movingTrack));
        StartCoroutine(SpawnTrack(movingTrack));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnTrack(GameObject track)
    {
        var currentTrack = new GameObject();

        var undergroundShifter = new GameObject();

        var spawnShift = false;

        var undergroundFactor = 10;
        
        while (BossCanvas.GameOver == false)
        {
            randIndex = Random.Range(0, undergroundFactor);
            
            if (randIndex == 9)
            {
                isUnderground = !isUnderground;
            }
            
            for (int i = 0; i < 5; i++)
            {
                if (i % 5 == 4)
                {
                    spawnTrafficLights = true;
                }
                
                if (!isUnderground)
                {
                    undergroundWall.SetActive(false);
                    currentTrack = Instantiate(track, transform.position, Quaternion.identity);
                    undergroundFactor = 10;
                }
                else
                {
                    undergroundWall.SetActive(true);
                    currentTrack = Instantiate(undergroundMovingTrack, transform.position, Quaternion.identity); 
                    undergroundFactor = 10;
                }
                
                
                currentTracks.Add(currentTrack);
                yield return new WaitForSeconds(1 / (trackSpeed/450));
                
                //StartCoroutine(MoveTrack(newTrack));
                if (currentTracks.Count >= 4)
                {
                    Destroy(currentTracks[0]);
                    currentTracks.RemoveAt(0);
                }
            }

            
        }
        

    }

    // IEnumerator SpawnInitialTracks(GameObject track)
    // {
    //     var track1 = Instantiate(track, new Vector3(0, 0, 500), Quaternion.identity);
    //     var track2 = Instantiate(track, new Vector3(0, 0, 50), Quaternion.identity);
    //     var track3 = Instantiate(track, new Vector3(0, 0, -400), Quaternion.identity);
    //     initialTracks.Add(track1);
    //     initialTracks.Add(track2);
    //     initialTracks.Add(track3);
    //
    //     foreach (var initialTrack in initialTracks)
    //     {
    //         Destroy(initialTrack);
    //     }
    //     
    //     StartCoroutine(SpawnTrack(movingTrack));
    //     
    //     yield return new WaitForSeconds(0.1f);
    // }

    
}
