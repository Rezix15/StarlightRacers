using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrack : MonoBehaviour
{
    public enum TrackDirection
    {
        Back,
        Forward,
        Initial
    }

    [SerializeField]
    private float trackSpeed;

    public TrackDirection trackDirection;

    private PlayerBoss player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRacer").GetComponent<PlayerBoss>();

        if (player != null)
        {
            trackSpeed = player.ReturnPlayerSpeed() / 10;
        }
        
    }

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
    }
}
