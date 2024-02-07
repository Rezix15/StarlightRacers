using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RacePosTracker : MonoBehaviour
{
    private Spacejet SpaceJetPlayer;
    private GameObject currentRacerCheckpoint;
    
    
    private SpacejetAI[] SpaceJetAis;
    
    private float racerCheckpos;
    
    public TrackGen trackGen;

    private List<GameObject> racers;
    private TextMeshProUGUI[] racerTextPosition;

    private string[] racePosTexts;
    private Vector3 finishPos;

    private int racerCheckpointCount;
    private TextMeshProUGUI racerText;
    
    
    // Start is called before the first frame update

    void Start()
    {
        trackGen = trackGen.GetComponent<TrackGen>();

        SpaceJetPlayer = FindObjectOfType<Spacejet>();
        SpaceJetAis = FindObjectsOfType<SpacejetAI>();
        
        racers = new List<GameObject> { SpaceJetPlayer.gameObject };
        
        racerTextPosition = new TextMeshProUGUI[racers.Count];
        
        racePosTexts = new[] { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th" };
        //finishPos = trackGen.ReturnFinishTrackPos();

        foreach (var spaceJetAi in SpaceJetAis)
        {
            racers.Add(spaceJetAi.gameObject);
        }

        foreach (var racer in racers)
        {
            Debug.Log("Racers: " + racer.name);
            racerText = racer.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    void SetRaceText()
    {
        Debug.Log("racerTextPos: " + racerTextPosition.Length);
    }
    // Update is called once per frame
    void Update()
    {
        CheckRacerPosition();
    }

    void CheckRacerPosition()
    {
        racerTextPosition = new TextMeshProUGUI[racers.Count];

        var index = 0;
        var currentRacers = new List<GameObject>();
        
        foreach (var racer in racers )
        {
            //index++;
            
            racerTextPosition[index] = racerText;
            racerTextPosition[index].text = racePosTexts[index];
            
            
            
            //check if racer is a player:
            if (racer.TryGetComponent<Spacejet>(out Spacejet player))
            {
                racerCheckpointCount = player.ReturnCurrentCheckpointCount();
                currentRacerCheckpoint = player.ReturnCurrentCheckpoint();
            }
            else
            {
                racer.TryGetComponent<SpacejetAI>(out SpacejetAI enemy);
            }
            
            if(player)
            // if (racer)
            // {
            //     racerCheckpointCount = player.ReturnCurrentCheckpointCount();
            //     currentRacerCheckpoint = player.ReturnCurrentCheckpoint();
            // }
            //
            // var enemyRacer
            
            // if(racer)
            
            //if the player and other racers are not empty
            index++;
        }
        
        // racers.Sort((racer1, racer2) =>
        //     {
        //         return 0;
        //     }
        // );
    }
}
