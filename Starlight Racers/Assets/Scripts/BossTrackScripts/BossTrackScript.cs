using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrackScript : MonoBehaviour
{
    public GameObject movingTrack;
    private List<GameObject> currentTracks;

    private PlayerBoss player;

    private float trackSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRacer").GetComponent<PlayerBoss>();

        
        trackSpeed = player.ReturnPlayerSpeed() / 10;
        
        
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
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                var currentTrack = Instantiate(track, transform.position, Quaternion.identity);
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
