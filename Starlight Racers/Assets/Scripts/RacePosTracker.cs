using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RacePosTracker : MonoBehaviour
{
    private Spacejet SpaceJetPlayer;
    private GameObject currentRacer1Checkpoint;
    private GameObject currentRacer2Checkpoint;
    
    private SpacejetAI[] SpaceJetAis;
    
    private float racerCheckpos;
    
    public TrackGen trackGen;

    private List<GameObject> racers;
    //private TextMeshProUGUI[] racerTextPosition;

    private string[] racePosTexts;
    private Vector3 finishPos;
    private int racer1CheckpointCount;
    private int racer2CheckpointCount;
    private TextMeshProUGUI[] racerText;
    
    
    
    
    // Start is called before the first frame update

    void Start()
    {
        trackGen = trackGen.GetComponent<TrackGen>();

        SpaceJetPlayer = FindObjectOfType<Spacejet>();
        SpaceJetAis = FindObjectsOfType<SpacejetAI>();
        
        racers = new List<GameObject> { SpaceJetPlayer.gameObject };
        
        //racerTextPosition = new TextMeshProUGUI[racers.Count];
        
        foreach (var spaceJetAi in SpaceJetAis)
        {
            racers.Add(spaceJetAi.gameObject);
        }

        racerText = new TextMeshProUGUI[racers.Count];
        
        racePosTexts = new[] { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th" };
        //finishPos = trackGen.ReturnFinishTrackPos();
        

        for (int i = 0; i < racers.Count; i++)
        {
            racerText[i] = racers[i].GetComponentInChildren<TextMeshProUGUI>();
            racerText[i].text = racePosTexts[i];
        }
    }

    void SetRaceText()
    {
        Debug.Log("racerTextPos: " + racePosTexts.Length);
    }
    // Update is called once per frame
    void Update()
    {
        CheckRacerPosition();
    }

    void CheckRacerPosition()
    {

        //var index = 0;
        var currentRacers = new List<GameObject>();
        
        for (int i = 0; i < racers.Count - 1; i++)
        {
            //Debug.Log( "Racer1Count: " + racer1CheckpointCount);
            //Debug.Log( "Racer2Count: " + racer2CheckpointCount);
            CheckIfPlayer(racers[i], racers[i+1]);
            
            if (racer1CheckpointCount == racer2CheckpointCount)
            {
                var checkRacerDist = Vector3.Dot(racers[i].transform.position, racers[i + 1].transform.position);
                
                if (checkRacerDist >= 0)
                {
                    racerText[i].text = racePosTexts[1];
                    racerText[i + 1].text = racePosTexts[0];
                }
                else
                {
                    racerText[i].text = racePosTexts[0];
                    racerText[i + 1].text = racePosTexts[1];
                }

            }
            else if(racer1CheckpointCount > racer2CheckpointCount)
            {
                racerText[i].text = racePosTexts[0];
                racerText[i + 1].text = racePosTexts[1];
            }
            else
            {
                racerText[i].text = racePosTexts[1];
                racerText[i + 1].text = racePosTexts[0];
            
            }
            
            
        }

        // racers.Sort((racer1, racer2) =>
        // {
        //     return 0;
        // });

        // foreach (var racer in racers)
        // {
        //     //index++;
        //     
        //     racerTextPosition[index] = racerText;
        //     racerTextPosition[index].text = racePosTexts[index];
        //     
        //     
        //     
        //     //check if racer is a player:
        //     if (racer.TryGetComponent<Spacejet>(out Spacejet player))
        //     {
        //         racer1CheckpointCount = player.ReturnCurrentCheckpointCount();
        //         currentRacer1Checkpoint = player.ReturnCurrentCheckpoint();
        //     }
        //     else
        //     {
        //         racer.TryGetComponent<SpacejetAI>(out SpacejetAI enemy);
        //         racer1CheckpointCount = enemy.ReturnCurrentCheckpointCount();
        //         currentRacer1Checkpoint = enemy.ReturnCurrentCheckpoint();
        //     }
        //     
        //     
        //     
        //     if(player)
        //     // if (racer)
        //     // {
        //     //     racerCheckpointCount = player.ReturnCurrentCheckpointCount();
        //     //     currentRacerCheckpoint = player.ReturnCurrentCheckpoint();
        //     // }
        //     //
        //     // var enemyRacer
        //     
        //     // if(racer)
        //     
        //     //if the player and other racers are not empty
        //     index++;
        // }
        //
        // // racers.Sort((racer1, racer2) =>
        // //     {
        // //         return 0;
        // //     }
        // // );
    }

    private void CheckIfPlayer(GameObject racer1, GameObject racer2)
    {
        //check if racer is a player:
        if (racer1.TryGetComponent<Spacejet>(out Spacejet player1))
        {
            racer1CheckpointCount = player1.ReturnCurrentCheckpointCount();
            currentRacer1Checkpoint = player1.ReturnCurrentCheckpoint();
            Debug.Log( "Racer1Count: " + racer1CheckpointCount);
        }
        else
        {
            racer1.TryGetComponent<SpacejetAI>(out SpacejetAI enemy);
            racer1CheckpointCount = enemy.ReturnCurrentCheckpointCount();
            currentRacer1Checkpoint = enemy.ReturnCurrentCheckpoint();
        }
        
        //Check if racer2 is the player
        if (racer2.TryGetComponent<Spacejet>(out Spacejet player))
        {
            racer2CheckpointCount = player.ReturnCurrentCheckpointCount();
            currentRacer2Checkpoint = player.ReturnCurrentCheckpoint();
        }
        else
        {
            racer2.TryGetComponent<SpacejetAI>(out SpacejetAI enemy);
            racer2CheckpointCount = enemy.ReturnCurrentCheckpointCount();
            currentRacer2Checkpoint = enemy.ReturnCurrentCheckpoint();
            Debug.Log( "Racer2Count: " + racer2CheckpointCount);
        }
    }
}
