using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrackGen : MonoBehaviour
{
    public int trackSeed = 0;
    public static int buildingSeed = buildingSeed = (int)System.DateTime.Now.Ticks;
    
    private NavMeshSurface meshSurface;
    public enum TrackType
    {
        StartTrack = 0,
        StraightForward = 1,
        StraightLeft = 2,
        StraightRight = 3,
        UpCurvedLeft = 4,
        UpCurvedRight = 5,
        DownCurvedLeft = 6,
        DownCurvedRight = 7,
        JunctionTrack = 8,
        BoosterTrack = 9,
        FinishTrack = 10,
        ArchTrack = 11,
        DiagonalForward = 12,
        DiagonalLeft = 13,
        DiagonalRight = 14,
        Checkpoint = 15,
    }

    #region objectdeclaration

    //Tracks
    public GameObject startTrackObj;
    public GameObject finishTrackObj;
    public GameObject straightForwardTrackObj;
    public GameObject straightLeftTrackObj;
    public GameObject straightRightTrackObj;
    public GameObject upCurvedLeftTrackObj;
    public GameObject upCurvedRightTrackObj;
    public GameObject downCurvedLeftTrackObj;
    public GameObject downCurvedRightTrackObj;
    public GameObject junctionTrackObj;
    public GameObject archTrackObj;
    public GameObject checkpointObj;
    public GameObject diagonalForwardObj;
    public GameObject diagonalLeftObj;
    public GameObject diagonalRightObj;
    public GameObject angleShifterObj; //An object that will be used to shift the angle of our player
    public GameObject enemySpawnerObj;

    //Unique Obj
    public GameObject boosterObj;
    private Vector3 boosterPos;
    public GameObject portalObj;
    
    public GameObject trafficLightObj;
    private Vector3 trafficPos;

    #endregion

    //Enemy Objs
    #region enemies

    public GameObject enemyRobot1;
    public GameObject enemyRobot2;

    #endregion
    
    //Generate the Start positions
    private Vector3 startPos;

    //Generate the initial position offset
    private Vector3 initialPos;
    
    //Generate the End positions
    private Vector3 finishPos;
    
    private int scale = MenuManager.scaleLevel;
    
    //modifier value for the scale. Primarily used in regards to setting the correct positions for the neighbours
    private int scaleFactor;

    //Unused variable
    //private int count = 0;
    
    private int reachLimit = MenuManager.reachLimit;

    public int junctionTrackCountLimit;
    int junctionTrackCount = 0;

    private int trackCount = 0;

    private bool stopGeneration = false;

    public static List<GameObject> checkpoints;
    
    bool shouldFinish = false;

    private List<GameObject> junctionTrackNeighbours;

    private bool junctionTrackCheck = false;

    private int specialChance = 4; // probability that the arch track will be generated. Currently it is at 3 which means 33%

    public bool generateRandomSeed = true;
    private void Awake()
    {
        //Initialize objects respective local scales
        # region scaleObjects
        startTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        finishTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        straightForwardTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        straightLeftTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        straightRightTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        upCurvedLeftTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        upCurvedRightTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        downCurvedLeftTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        downCurvedRightTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        junctionTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        archTrackObj.transform.localScale = new Vector3(scale, scale, scale);
        boosterObj.transform.localScale = new Vector3(scale, scale, scale);
        portalObj.transform.localScale = new Vector3(scale * 15, scale * 15, scale * 15);
        trafficLightObj.transform.localScale = new Vector3(scale * 66.66f, scale * 66.66f, scale * 66.66f);
        angleShifterObj.transform.localScale = new Vector3(scale, scale, scale); 
        enemySpawnerObj.transform.localScale = new Vector3(scale, scale, scale);
        enemyRobot2.transform.localScale = new Vector3(scale / 2f, scale / 2f, scale / 2f);
        meshSurface = GetComponent<NavMeshSurface>();
        # endregion
        
        //Seed Generation
        GenerateTrackSeed();
    }

    private void GenerateNavMesh()
    {
        meshSurface.BuildNavMesh();
    }

    private void GenerateTrackSeed()
    {
        if (generateRandomSeed)
        {
            trackSeed = (int)System.DateTime.Now.Ticks;
        }
        
        Random.InitState(trackSeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3(0, 0, 0); //Starting position should start at 0,0,0
        GenerateTrack(TrackType.StartTrack, startPos, Quaternion.identity);

        //Set a scaleFactor
        scaleFactor = scale * 10;
        boosterPos = new Vector3(0, (-0.44f * scale), 0);
        trafficPos = new Vector3(scale * 3, (0.75f * scale) + 1.15f , scale * 1.5f);
        

        //GenerateInitialPosition
        initialPos = new Vector3(0, 0, scaleFactor);
        GenerateTrack(TrackType.StraightForward, initialPos, Quaternion.identity);
        GenerateNeighbours(TrackType.StraightForward, initialPos);
    }

    //Function to generate the Starting and Finish Tracks
    void GenerateTrack(TrackType trackType, Vector3 position, Quaternion quaternion)
    {
        switch (trackType)
        {
            case TrackType.StartTrack:
                Instantiate(startTrackObj, position, quaternion, transform);
                break;
            
            case TrackType.StraightForward:
                Instantiate(straightForwardTrackObj, position, quaternion, transform);
                break;
            
            case TrackType.FinishTrack:
                Instantiate(finishTrackObj, position, quaternion, transform);
                break;
            
            case TrackType.BoosterTrack:
                Instantiate(boosterObj, position, quaternion, transform);
                break;
            
            case TrackType.Checkpoint:
                Instantiate(checkpointObj, position, quaternion, transform);
                break;
        }
    }
    
    
    //Function to generate the neighbours for each of the given tracks
    void GenerateNeighbours(TrackType trackType, Vector3 prevPosition)
    {
        GameObject[] upNeighbours = null;
        GameObject[] rightNeighbours = null;
        GameObject[] leftNeighbours = null;

        int randIndex;

        int boosterRandomness = Random.Range(0, 3); //probability that a booster is spawned at that region

        Vector3 initialPosition;

        Vector3 newPosition;

        var archIndex = -1;
        
        //GenerateTrack(TrackType.Checkpoint, prevPosition, Quaternion.identity);

        junctionTrackCheck = false;

        trackCount++;
        
        switch (trackType)
        {
            case TrackType.StraightForward:
            {
                upNeighbours = new GameObject[] {straightForwardTrackObj, upCurvedLeftTrackObj, upCurvedRightTrackObj, archTrackObj, diagonalForwardObj, junctionTrackObj, finishTrackObj};
                
                //upNeighbours = new GameObject[] {straightForwardTrackObj, upCurvedLeftTrackObj, upCurvedRightTrackObj, finishTrackObj};
                
                // var curvedPositionOffsetX = scale * 0.34f;
                // var curvedPositionOffsetZ = scale * 1.2f;
                
                // var curvedPositionOffsetX = scale * 16f;
                // var curvedPositionOffsetZ = scale * 26.4f;
                
                // var curvedPositionOffsetX = scale * 0.37f;
                // var curvedPositionOffsetZ = scale * 1.24f;

                var upCurvedPosL = new Vector3(scale * 16f, 0, scale * 16.44f);
                var upCurvedPosR = new Vector3(scale * 0.356f, 0, scale * 1.24f);
                
                var diagonalPos = new Vector3((scale * -0.014f), (scale * -4.273f), (scale * 16.31f));
                
                initialPosition = new Vector3(0, 0, scaleFactor);
                
                newPosition = new Vector3(
                    prevPosition.x + initialPosition.x, 
                    prevPosition.y + initialPosition.y, 
                    prevPosition.z + initialPosition.z
                );

                var boosterPosition = new Vector3(
                    newPosition.x + boosterPos.x,
                    newPosition.y + boosterPos.y,
                    newPosition.z + boosterPos.z
                );
                
                var trafficLightPosLeft = new Vector3(
                    newPosition.x - trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z + trafficPos.z
                );

                var trafficLightPosRight = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z + trafficPos.z
                );

                var newDiagonalPos = new Vector3(
                    prevPosition.x + diagonalPos.x,
                    prevPosition.y + diagonalPos.y,
                    prevPosition.z + diagonalPos.z
                );  

                randIndex = Random.Range(0, 3);

                if ((trackCount > 0 && trackCount % 5 == 0) && shouldFinish == false && junctionTrackCheck == false) 
                {
                    archIndex = Random.Range(0, specialChance);
                    randIndex = 3;
                }
                
                if (junctionTrackCount < junctionTrackCountLimit)
                {
                    // Debug.Log("NewPos: " + newPosition);
                    // Debug.Log("SpecificReachLimitMin: " + (reachLimit / 2f - 2000));
                    // Debug.Log("SpecificReachLimitMax: " + (reachLimit / 2f + 2500));
                    
                    if(((newPosition.x >= (reachLimit / 2f - 2000)) && (newPosition.x < (reachLimit / 2f + 2500))) || ((newPosition.z >= (reachLimit / 2f - 2000)) && (newPosition.z < (reachLimit / 2f + 2500))))
                    {
                        //Debug.Log("GENERATE JUNCTION!");
                        junctionTrackCheck = true;
                        //randIndex = Random.Range(0, 4);
                        randIndex = 3;
                    }
                }
                
                
                //If the newPosition track has reached the 9,000 barrier on the z axis, allow the possible generation of
                //the finish Track. If the position exceeds 15,000 force generation of finishTrack.
                if ((newPosition.z >= (reachLimit * 0.9f) && newPosition.z < reachLimit) || (Mathf.Abs(newPosition.x) >= (reachLimit * 0.9f) && Mathf.Abs(newPosition.x) < reachLimit))
                {
                    randIndex = Random.Range(0, 4);
                    shouldFinish = true;
                }
                else if(newPosition.z >= reachLimit || Mathf.Abs(newPosition.x) >= reachLimit)
                {
                    randIndex = 3;
                    shouldFinish = true;
                }
                
                GenerateTrack(TrackType.Checkpoint, newPosition, Quaternion.identity);
                
                switch (randIndex)
                {
                    case 0:
                    {
                        //Generate Up Neighbours
                        Instantiate(upNeighbours[0], newPosition, Quaternion.identity, transform);
                        
                        //Booster Track Generation
                        if ((trackCount > 0 && trackCount % 3 == 0 && boosterRandomness == 1) )
                        {
                            GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.identity);
                        }
                        
                        //Traffic Light Generation
                        if (trackCount % 4 == 0) 
                        {
                            Instantiate(trafficLightObj, trafficLightPosLeft, Quaternion.Euler(0,270,0), transform);
                            Instantiate(trafficLightObj, trafficLightPosRight, Quaternion.Euler(0,270,0), transform);
                        }
                        
                        if ((trackCount > 0 && trackCount % 7 == 0) )
                        {
                            var enemyPos = new Vector3(newPosition.x, newPosition.y + 40f, newPosition.z);
                            Instantiate(enemyRobot2, enemyPos, Quaternion.identity);
                        }

                       
                        GenerateNeighbours(TrackType.StraightForward, newPosition);
                        break;
                    }

                    case 1:
                    {
                        newPosition = new Vector3(
                            (prevPosition.x + initialPosition.x) - upCurvedPosL.x, 
                            prevPosition.y + initialPosition.y, 
                            (prevPosition.z + initialPosition.z) + upCurvedPosL.z
                        );
                        
                        Instantiate(upNeighbours[1], newPosition, Quaternion.identity, transform);
                        GenerateNeighbours(TrackType.UpCurvedLeft, newPosition);
                        break;
                    }

                    case 2:
                    {
                        newPosition = new Vector3(
                            (prevPosition.x + initialPosition.x) + upCurvedPosR.x, 
                            prevPosition.y + initialPosition.y, 
                            (prevPosition.z + initialPosition.z) + upCurvedPosR.z
                        );
                        
                        Instantiate(upNeighbours[2], newPosition, Quaternion.identity, transform);
                        GenerateNeighbours(TrackType.UpCurvedRight, newPosition);
                        break;
                    }
                    
                    //Special Track Generation (Finish/Arch/Junction)
                    case 3:
                    {
                        // if (archIndex % specialChance == 1 ) 
                        // {
                        //     newPosition = new Vector3(
                        //         (prevPosition.x + initialPosition.x),
                        //         prevPosition.y + initialPosition.y, 
                        //         (prevPosition.z + 2 * initialPosition.z)
                        //     );
                        //
                        //     Instantiate(upNeighbours[3], newPosition + initialPosition, Quaternion.identity, transform);
                        //     GenerateNeighbours(TrackType.ArchTrack, newPosition + initialPosition);
                        //     //Debug.Log("Generated Arch");
                        // }
                        if(archIndex % specialChance == 1 || archIndex % specialChance == 2)
                        {
                            Instantiate(upNeighbours[4], newDiagonalPos, Quaternion.identity, transform);
                            GenerateNeighbours(TrackType.DiagonalForward, newDiagonalPos);
                        }
                        else if (junctionTrackCheck && shouldFinish == false )
                        {
                            newPosition = new Vector3(
                                (prevPosition.x + initialPosition.x),
                                prevPosition.y + initialPosition.y, 
                                (prevPosition.z + 2 * initialPosition.z)
                            );
                        
                            junctionTrackCount++;
                            Instantiate(upNeighbours[5], newPosition, Quaternion.identity, transform);
                            GenerateNeighbours(TrackType.JunctionTrack, newPosition);
                            
                        }
                        else if(shouldFinish)
                        {
                            //Finish Track
                            Instantiate(upNeighbours[6], newPosition, Quaternion.identity, transform);
                            finishPos = upNeighbours[6].transform.position;
                            
                            //Generate checkpoints list to be used for race positioning
                            checkpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint"));
                            checkpoints.Add(GameObject.FindGameObjectWithTag("Finish"));
                            //Debug.Log("TrackCount: " + trackCount);

                        }
                        else
                        {
                            Debug.Log("JunctionTrack Generation failed: ");
                            Instantiate(upNeighbours[0], newPosition, Quaternion.identity, transform);
                            GenerateNeighbours(TrackType.StraightForward, newPosition);
                        }
                        
                        break;
                        
                    }
                    
                }
                
                break;
            }
            
            case TrackType.StraightRight:
            {
                rightNeighbours = new GameObject[] {straightRightTrackObj, downCurvedRightTrackObj};
                
                randIndex = Random.Range(0, 2);
                
                initialPosition = new Vector3(scaleFactor, 0, 0);
                
                newPosition = new Vector3(
                    prevPosition.x + initialPosition.x, 
                    prevPosition.y + initialPosition.y, 
                    prevPosition.z + initialPosition.z
                );
                
                var boosterPosition = new Vector3(
                    newPosition.x + boosterPos.x,
                    newPosition.y + boosterPos.y,
                    newPosition.z + boosterPos.z
                );
                
                var trafficLightPosLeft = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z - 2 * trafficPos.z
                );

                var trafficLightPosRight = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z + 2 * trafficPos.z
                );
                
                // var curvedPositionOffsetX = scale * 1.2f;
                // var curvedPositionOffsetZ = scale * 0.34f;
                
                
                var downCurvedPosR = new Vector3(scale * 26.6f, 0, scale * 16f);
                
                GenerateTrack(TrackType.Checkpoint, newPosition, Quaternion.Euler(0,90,0));
                
                
                switch (randIndex)
                {
                    case 0:
                    {
                        //Generate the right Neighbour
                        Instantiate(rightNeighbours[0], newPosition, Quaternion.identity, transform);
                        
                        //Booster Track Generation
                        if ((trackCount > 0 && trackCount % 3 == 0)  && boosterRandomness == 1)
                        {
                            GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.Euler(0,-90,0));
                        }
                        
                        //Traffic Light Generation
                        if ((trackCount > 0 && trackCount % 4 == 0))
                        {
                            Instantiate(trafficLightObj, trafficLightPosLeft, Quaternion.Euler(0,0,0), transform);
                            Instantiate(trafficLightObj, trafficLightPosRight, Quaternion.Euler(0,0,0), transform);
                        }
                        
                        GenerateNeighbours(TrackType.StraightRight, newPosition);
                        
                        
                        break;
                    }

                    case 1:
                    {
                        newPosition = new Vector3(
                            prevPosition.x + downCurvedPosR.x, 
                            prevPosition.y + initialPosition.y, 
                            prevPosition.z + downCurvedPosR.z
                        );
                        
                        Instantiate(rightNeighbours[1], newPosition, Quaternion.identity, transform);
                        GenerateNeighbours(TrackType.DownCurvedRight, newPosition);
                        break;
                    }
                }
                break;
            }
            
            case TrackType.StraightLeft:
            {
                leftNeighbours = new GameObject[] { straightLeftTrackObj, downCurvedLeftTrackObj };
                
                randIndex = Random.Range(0, 2);
                
                initialPosition = new Vector3(-scaleFactor, 0, 0);
                
                // var curvedPositionOffsetX = scale * 1.2f;
                // var curvedPositionOffsetZ = scale * 0.34f;
                
                //var downCurvedPosL = new Vector3(scale * 11.24f, 0, scale * 0.38f);

                var downCurvedPosL = new Vector3(-(scale * 11.24f), 0, (scale * 0.38f));
                
                newPosition = new Vector3(
                    prevPosition.x + initialPosition.x, 
                    prevPosition.y + initialPosition.y, 
                    prevPosition.z + initialPosition.z
                );
                
                var boosterPosition = new Vector3(
                    newPosition.x + boosterPos.x,
                    newPosition.y + boosterPos.y,
                    newPosition.z + boosterPos.z
                );
                
                var trafficLightPosLeft = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z - 2 * trafficPos.z
                );

                var trafficLightPosRight = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z + 2 * trafficPos.z
                );
                
                GenerateTrack(TrackType.Checkpoint, newPosition, Quaternion.Euler(0,-90,0));
                
                switch (randIndex)
                {
                    case 0:
                    {
                        //Generate the left neighbour
                        Instantiate(leftNeighbours[0], newPosition, Quaternion.identity, transform);
                        
                        //Booster Track Generation
                        if ((trackCount > 0 && trackCount % 3 == 0)  && boosterRandomness == 1)
                        {
                            GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.Euler(0,-90,0));
                        }
                        
                        //Traffic Light Generation
                        if ((trackCount > 0 && trackCount % 4 == 0) )
                        {
                            Instantiate(trafficLightObj, trafficLightPosLeft, Quaternion.Euler(0,180,0), transform);
                            Instantiate(trafficLightObj, trafficLightPosRight, Quaternion.Euler(0,180,0), transform);
                        }
                        
                        GenerateNeighbours(TrackType.StraightLeft, newPosition);
                        
                        break;
                    }

                    case 1:
                    {
                        newPosition = new Vector3(
                            prevPosition.x + downCurvedPosL.x, 
                            prevPosition.y + initialPosition.y, 
                            prevPosition.z + downCurvedPosL.z
                        );
                        
                        Instantiate(leftNeighbours[1], newPosition, Quaternion.identity, transform);
                        GenerateNeighbours(TrackType.DownCurvedLeft, newPosition);
                        break;
                    }
                    
                    default:
                        break;
                }
                
                break;
            }

            case TrackType.UpCurvedLeft:
            {
                leftNeighbours = new GameObject[] { straightLeftTrackObj };
                //downNeighbours = new GameObject[] { straightForwardTrackObj };

                initialPosition = new Vector3(-(scale * 11.23f), 0, (scale * 0.37f));
                
                newPosition = new Vector3(
                    prevPosition.x + initialPosition.x, 
                    prevPosition.y + initialPosition.y, 
                    prevPosition.z + initialPosition.z
                );
                
                var boosterPosition = new Vector3(
                    newPosition.x + boosterPos.x,
                    newPosition.y + boosterPos.y,
                    newPosition.z + boosterPos.z
                );
               
                var trafficLightPosLeft = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z - 2 * trafficPos.z
                );

                var trafficLightPosRight = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z + 2 * trafficPos.z
                );
                
                GenerateTrack(TrackType.Checkpoint, newPosition, Quaternion.Euler(0,-90,0));
                
                //Booster Track Generation
                if ((trackCount > 0 && trackCount % 3 == 0)  && boosterRandomness == 1)
                {
                    GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.Euler(0,-90,0));
                }
                    
                //Traffic Light Generation
                if (trackCount % 4 == 0) 
                {
                    Instantiate(trafficLightObj, trafficLightPosLeft, Quaternion.Euler(0,180,0), transform);
                    Instantiate(trafficLightObj, trafficLightPosRight, Quaternion.Euler(0,180,0), transform);
                }
                
                
                Instantiate(leftNeighbours[0], newPosition, Quaternion.identity, transform);
                GenerateNeighbours(TrackType.StraightLeft, newPosition);
                break;
            }
            
            case TrackType.UpCurvedRight:
            {
                rightNeighbours = new GameObject[] { straightRightTrackObj };
                
                //initialPosition = new Vector3((scale * 26.58f), 0, (scale * 16f));
                
                initialPosition = new Vector3(scale * 26.53f, 0, scale * 16f);

                newPosition = new Vector3(
                    prevPosition.x + initialPosition.x, 
                    prevPosition.y + initialPosition.y, 
                    prevPosition.z + initialPosition.z
                );
                
                var boosterPosition = new Vector3(
                    newPosition.x + boosterPos.x,
                    newPosition.y + boosterPos.y,
                    newPosition.z + boosterPos.z
                );
                
                var trafficLightPosLeft = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z - 2 * trafficPos.z
                );

                var trafficLightPosRight = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z + 2 * trafficPos.z
                );
                
                GenerateTrack(TrackType.Checkpoint, newPosition, Quaternion.Euler(0,90,0));
                
                //Booster Track Generation
                if ((trackCount > 0 && trackCount % 3 == 0)  && boosterRandomness == 1)
                {
                    GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.Euler(0,-90,0));
                }
                    
                //Traffic Light Generation
                if ((trackCount > 0 && trackCount % 4 == 0))
                {
                    Instantiate(trafficLightObj, trafficLightPosLeft, Quaternion.Euler(0,0,0), transform);
                    Instantiate(trafficLightObj, trafficLightPosRight, Quaternion.Euler(0,0,0), transform);
                }
                
                Instantiate(rightNeighbours[0], newPosition, Quaternion.identity, transform);
                GenerateNeighbours(TrackType.StraightRight, newPosition);
                break;
            }
            
            case TrackType.DownCurvedLeft:
            {
                upNeighbours = new GameObject[] { straightForwardTrackObj};
                
                //initialPosition = new Vector3(-(scale * 16f), 0, (scale * 26.58f));
                
                initialPosition = new Vector3(-(scale * 16f), 0, (scale * 16.44f));
                
                //initialPosition = new Vector3(-(scale * 11.24f), 0, (scale * 0.38f));
                
                newPosition = new Vector3(
                    prevPosition.x + initialPosition.x, 
                    prevPosition.y + initialPosition.y, 
                    prevPosition.z + initialPosition.z
                );
                
                var boosterPosition = new Vector3(
                    newPosition.x + boosterPos.x,
                    newPosition.y + boosterPos.y,
                    newPosition.z + boosterPos.z
                );
                
                var trafficLightPosLeft = new Vector3(
                    newPosition.x - trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z + trafficPos.z
                );

                var trafficLightPosRight = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z + trafficPos.z
                );
                
                //Booster Track Generation
                if ((trackCount > 0 && trackCount % 3 == 0 && boosterRandomness == 1) )
                {
                    GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.identity);
                }
                
                GenerateTrack(TrackType.Checkpoint, newPosition, Quaternion.identity);
                
                
                //Traffic Light Generation
                if (trackCount % 4 == 0) 
                {
                    Instantiate(trafficLightObj, trafficLightPosLeft, Quaternion.Euler(0,270,0), transform);
                    Instantiate(trafficLightObj, trafficLightPosRight, Quaternion.Euler(0,270,0), transform);
                }
                
           
                Instantiate(upNeighbours[0], newPosition, Quaternion.identity, transform);
                
                if ((trackCount > 0 && trackCount % 7 == 0) )
                {
                    var enemyPos = new Vector3(newPosition.x, newPosition.y + 40f, newPosition.z);
                    Instantiate(enemyRobot2, enemyPos, Quaternion.identity);
                }
                
                GenerateNeighbours(TrackType.StraightForward, newPosition);
                break;
            }
            
            case TrackType.DownCurvedRight:
            {
                upNeighbours = new GameObject[] { straightForwardTrackObj };
                
                //initialPosition = new Vector3((scale * 16f), 0, (scale * 26.58f));
                
                //initialPosition = new Vector3((scale * 11.24f), 0, (scale * 0.38f));
                
                initialPosition = new Vector3(scale * 0.379f, 0, scale * 11.23f);
                
                newPosition = new Vector3(
                    prevPosition.x + initialPosition.x, 
                    prevPosition.y + initialPosition.y, 
                    prevPosition.z + initialPosition.z
                );
                
                var boosterPosition = new Vector3(
                    newPosition.x + boosterPos.x,
                    newPosition.y + boosterPos.y,
                    newPosition.z + boosterPos.z
                );
                
                var trafficLightPosLeft = new Vector3(
                    newPosition.x - trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z + trafficPos.z
                );

                var trafficLightPosRight = new Vector3(
                    newPosition.x + trafficPos.x,
                    newPosition.y + trafficPos.y,
                    newPosition.z + trafficPos.z
                );
                
                //Booster Track Generation
                if ((trackCount > 0 && trackCount % 3 == 0 && boosterRandomness == 1) )
                {
                    GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.identity);
                }
                
                GenerateTrack(TrackType.Checkpoint, newPosition, Quaternion.identity);
                    
                //Traffic Light Generation
                if ((trackCount > 0 && trackCount % 4 == 0))
                {
                    Instantiate(trafficLightObj, trafficLightPosLeft, Quaternion.Euler(0,270,0), transform);
                    Instantiate(trafficLightObj, trafficLightPosRight, Quaternion.Euler(0,270,0), transform);
                }
                
             
                Instantiate(upNeighbours[0], newPosition, Quaternion.identity, transform);
                
                if ((trackCount > 0 && trackCount % 7 == 0) )
                {
                    var enemyPos = new Vector3(newPosition.x, newPosition.y + 40f, newPosition.z);
                    Instantiate(enemyRobot2, enemyPos, Quaternion.identity);
                }
                
                GenerateNeighbours(TrackType.StraightForward, newPosition);
                break;
                
            }

            case TrackType.JunctionTrack:
            {
                upNeighbours = new GameObject[] { straightForwardTrackObj };
                leftNeighbours = new GameObject[] { straightLeftTrackObj };
                rightNeighbours = new GameObject[] { straightRightTrackObj };
                var initialStraightPosition = new Vector3(0, 0, (scaleFactor * 2));
                var initialLeftPos = new Vector3((-scaleFactor * 2), 0, 0);
                var initialRightPos = new Vector3((scaleFactor * 2), 0, 0);
                
                var newStraightPosition = new Vector3(
                    prevPosition.x + initialStraightPosition.x, 
                    prevPosition.y + initialStraightPosition.y, 
                    prevPosition.z + initialStraightPosition.z
                );
                
                var newLeftPos = new Vector3(
                    prevPosition.x + initialLeftPos.x, 
                    prevPosition.y + initialLeftPos.y, 
                    prevPosition.z + initialLeftPos.z
                );
                
                var newRightPos = new Vector3(
                    prevPosition.x + initialRightPos.x, 
                    prevPosition.y + initialRightPos.y, 
                    prevPosition.z + initialRightPos.z
                );

                
                Instantiate(upNeighbours[0], newStraightPosition, Quaternion.identity, transform);
                Instantiate(leftNeighbours[0], newLeftPos, Quaternion.identity, transform);
                Instantiate(rightNeighbours[0], newRightPos, Quaternion.identity, transform);
                // GenerateNeighbours(TrackType.JunctionTrack, newStraightPosition);

                //randIndex = 0;
                //var index = 1;

                
                Instantiate(upNeighbours[0], newStraightPosition + (initialStraightPosition / 2), Quaternion.identity, transform);
                GenerateNeighbours(TrackType.StraightForward, newStraightPosition + (initialStraightPosition / 2));
                
                for (int i = 1; i < 10; i++)
                {
                    Instantiate(leftNeighbours[0], newLeftPos + (initialLeftPos / 2) * i, Quaternion.identity, transform);
                    Instantiate(rightNeighbours[0], newRightPos  + (initialRightPos / 2) * i, Quaternion.identity, transform);
                }

                //Generate the two portal objects
                //Instantiate(portalObj, newLeftPos + (initialLeftPos / 2) * 9, Quaternion.Euler(0, 180, 0));
                Instantiate(portalObj, newRightPos  + (initialRightPos / 2) * 9, Quaternion.Euler(0, 180, 0), transform);
                //GenerateNavMesh();
                break;
                
            }

            case TrackType.ArchTrack:
            {
                upNeighbours = new GameObject[] { straightForwardTrackObj };
                initialPosition = new Vector3(0, 0, (scaleFactor * 2));
                
                var newPos = new Vector3(
                    prevPosition.x + initialPosition.x, 
                    prevPosition.y + initialPosition.y, 
                    prevPosition.z + initialPosition.z
                );
                
                Instantiate(upNeighbours[0], newPos + initialPosition * 1.55f, Quaternion.identity, transform);
                GenerateNeighbours(TrackType.StraightForward, newPos + initialPosition * 1.55f);
                
                break;
            }

            case TrackType.DiagonalForward:
            {
                var randomIndex = Random.Range(0, 4);
                upNeighbours = new GameObject[] { straightForwardTrackObj };
                initialPosition = new Vector3((scale * -0.014f), (scale * -4.273f), (scale * 16.31f));
                var straightPosD = new Vector3(0.008f * scale, -6.726f * scale, 6.71f * scale);
                var straightPosF = new Vector3(0f * scale, -3.44f * scale, 8.04f * scale);

                for (int i = 1; i <= randomIndex+1; i++)
                {
                    Instantiate(upNeighbours[0],  prevPosition + ((straightPosD) * i), Quaternion.Euler(45, 0, 0), transform);
                }

                var newPos = (prevPosition + ((straightPosD) * (randomIndex + 1)));
                Instantiate(upNeighbours[0],  newPos + straightPosF, Quaternion.identity, transform);
                Instantiate(angleShifterObj, newPos + straightPosF, Quaternion.identity, transform);
                GenerateNeighbours(TrackType.StraightForward, newPos + straightPosF);
                break;
            }
            
            case TrackType.DiagonalLeft:
            {
                leftNeighbours = new GameObject[] { straightLeftTrackObj };
                initialPosition = new Vector3((scale * 16.31f), (scale * -4.273f), (scale * 0.014f));
                break;
            }
            
            case TrackType.DiagonalRight:
            {
                rightNeighbours = new GameObject[]{ straightRightTrackObj };
                initialPosition = new Vector3((scale * 16.31f), (scale * -4.273f), (scale * -0.014f));
                break;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
