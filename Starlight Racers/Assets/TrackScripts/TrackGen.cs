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
    private NavMeshSurface meshSurface;
    public enum TrackType
    {
        StartTrack = 0,
        FinishTrack = 10,
        StraightForward = 1,
        StraightLeft = 2,
        StraightRight = 3,
        UpCurvedLeft = 4,
        UpCurvedRight = 5,
        DownCurvedLeft = 6,
        DownCurvedRight = 7,
        JunctionTrack = 8,
        BoosterTrack = 9,
        Checkpoint = 11,
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
    public GameObject checkpointObj;

    //Unique Obj
    public GameObject boosterObj;
    private Vector3 boosterPos;
    
    public GameObject trafficLightObj;
    private Vector3 trafficPos;

    #endregion
   
    
    //Generate the Start positions
    private Vector3 startPos;

    //Generate the initial position offset
    private Vector3 initialPos;
    
    //Generate the End positions
    private Vector3 finishPos;

    [SerializeField]
    private int scale;
    
    //modifier value for the scale. Primarily used in regards to setting the correct positions for the neighbours
    private int scaleFactor;

    //Unused variable
    //private int count = 0;

    public int reachLimit;

    public int junctionTrackCountLimit;
    int junctionTrackCount = 0;

    private int trackCount = 0;

    private bool stopGeneration = false;
    
    private void Awake()
    {
        //Initialize objects respective local scales
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
        boosterObj.transform.localScale = new Vector3(scale, scale, scale);
        trafficLightObj.transform.localScale = new Vector3(scale * 66.66f, scale * 66.66f, scale * 66.66f);
        meshSurface = GetComponent<NavMeshSurface>();
    }

    private void GenerateNavMesh()
    {
        meshSurface.BuildNavMesh();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3(0, 0, 0); //Starting position should start at 0,0,0
        GenerateTrack(TrackType.StartTrack, startPos, Quaternion.identity);

        //Set a scaleFactor
        scaleFactor = scale * 10;
        boosterPos = new Vector3(0, -0.6f * scale, 0);
        trafficPos = new Vector3(scale * 3, 0.75f * scale, scale * 1.5f);
        

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
        
        GenerateTrack(TrackType.Checkpoint, prevPosition, Quaternion.identity);
        
        if (stopGeneration == false)
        {
            trackCount++;
            
            switch (trackType)
            {
                case TrackType.StraightForward:
                {
                    upNeighbours = new GameObject[] {straightForwardTrackObj, upCurvedLeftTrackObj, upCurvedRightTrackObj, junctionTrackObj, finishTrackObj};
                    
                    //upNeighbours = new GameObject[] {straightForwardTrackObj, upCurvedLeftTrackObj, upCurvedRightTrackObj, finishTrackObj};
                    
                    var curvedPositionOffsetX = scale * 0.34f;
                    var curvedPositionOffsetZ = scale * 1.2f;
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
                    


                    // if (junctionTrackCount >= junctionTrackCountLimit)
                    // {
                    //     randIndex = Random.Range(0, 3);
                    // }
                    // else
                    // {
                        randIndex = Random.Range(0, 3);
                    //}
                    
                    //If the newPosition track has reached the 9,000 barrier on the z axis, allow the possible generation of
                    //the finish Track. If the position exceeds 15,000 force generation of finishTrack.
                    if ((newPosition.z >= (reachLimit * 0.9f) && newPosition.z < reachLimit) || (Mathf.Abs(newPosition.x) >= (reachLimit * 0.9f) && Mathf.Abs(newPosition.x) < reachLimit))
                    {
                        randIndex = Random.Range(0, 4);
                    }
                    else if(newPosition.z >= reachLimit || Mathf.Abs(newPosition.x) >= reachLimit)
                    {
                        randIndex = 3;
                    }
                    
                    switch (randIndex)
                    {
                        case 0:
                        {
                            //Generate Up Neighbours
                            Instantiate(upNeighbours[0], newPosition, Quaternion.identity, transform);
                            
                            //Booster Track Generation
                            if ((trackCount > 0 && trackCount % 4 == 0 && boosterRandomness == 1) )
                            {
                                GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.identity);
                            }
                            
                            //Traffic Light Generation
                            if (trackCount % 5 == 0) 
                            {
                                Instantiate(trafficLightObj, trafficLightPosLeft, Quaternion.Euler(0,270,0), transform);
                                Instantiate(trafficLightObj, trafficLightPosRight, Quaternion.Euler(0,270,0), transform);
                            }
                           
                            GenerateNeighbours(TrackType.StraightForward, newPosition);
                            
                            
                            break;
                        }

                        case 1:
                        {
                            newPosition = new Vector3(
                                (prevPosition.x + initialPosition.x) - curvedPositionOffsetX, 
                                prevPosition.y + initialPosition.y, 
                                (prevPosition.z + initialPosition.z) + curvedPositionOffsetZ
                            );
                            
                            Instantiate(upNeighbours[1], newPosition, Quaternion.identity, transform);
                            GenerateNeighbours(TrackType.UpCurvedLeft, newPosition);
                            break;
                        }

                        case 2:
                        {
                            newPosition = new Vector3(
                                (prevPosition.x + initialPosition.x) + curvedPositionOffsetX, 
                                prevPosition.y + initialPosition.y, 
                                (prevPosition.z + initialPosition.z) + curvedPositionOffsetZ
                            );
                            
                            Instantiate(upNeighbours[2], newPosition, Quaternion.identity, transform);
                            GenerateNeighbours(TrackType.UpCurvedRight, newPosition);
                            break;
                        }
                        
                        //Finish Track/JunctionTrack generation
                        case 3:
                        {
                            // if (junctionTrackCount < junctionTrackCountLimit && (trackCount % 4 == 0 || trackCount % 4 == 1) && shouldFinish == false)
                            // {
                            //     newPosition = new Vector3(
                            //         (prevPosition.x + initialPosition.x),
                            //         prevPosition.y + initialPosition.y, 
                            //         (prevPosition.z + 2 * initialPosition.z)
                            //     );
                            //
                            //     Instantiate(upNeighbours[3], newPosition, Quaternion.identity, transform);
                            //     GenerateNeighbours(TrackType.JunctionTrack, newPosition);
                            //     junctionTrackCount++;
                            // }
                            // else
                            // {
                            
                            
                            //Finish Track
                            Instantiate(upNeighbours[4], newPosition, Quaternion.identity, transform);
                            finishPos = upNeighbours[4].transform.position;
                            stopGeneration = true;
                            GenerateNavMesh();
                            //}
                            
                            Debug.Log("TrackCount: " + trackCount);
                            
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
                    
                    var curvedPositionOffsetX = scale * 1.2f;
                    var curvedPositionOffsetZ = scale * 0.34f;
                    
                    
                    switch (randIndex)
                    {
                        case 0:
                        {
                            //Generate the right Neighbour
                            Instantiate(rightNeighbours[0], newPosition, Quaternion.identity, transform);
                            
                            //Booster Track Generation
                            if ((trackCount > 0 && trackCount % 4 == 0)  && boosterRandomness == 1)
                            {
                                GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.Euler(0,-90,0));
                            }
                            
                            //Traffic Light Generation
                            if ((trackCount > 0 && trackCount % 5 == 0))
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
                                (prevPosition.x + initialPosition.x) + curvedPositionOffsetX, 
                                prevPosition.y + initialPosition.y, 
                                (prevPosition.z + initialPosition.z) + curvedPositionOffsetZ
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
                    
                    var curvedPositionOffsetX = scale * 1.2f;
                    var curvedPositionOffsetZ = scale * 0.34f;
                    
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
                    
                    switch (randIndex)
                    {
                        case 0:
                        {
                            //Generate the left neighbour
                            Instantiate(leftNeighbours[0], newPosition, Quaternion.identity, transform);
                            
                            //Booster Track Generation
                            if ((trackCount > 0 && trackCount % 4 == 0)  && boosterRandomness == 1)
                            {
                                GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.Euler(0,-90,0));
                            }
                            
                            //Traffic Light Generation
                            if ((trackCount > 0 && trackCount % 5 == 0) )
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
                                (prevPosition.x + initialPosition.x) + curvedPositionOffsetX, 
                                prevPosition.y + initialPosition.y, 
                                (prevPosition.z + initialPosition.z) + curvedPositionOffsetZ
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
                    
                    initialPosition = new Vector3(-(scale * 26.58f), 0, (scale * 16f));
                    
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
                    
                    //Booster Track Generation
                    if ((trackCount > 0 && trackCount % 4 == 0)  && boosterRandomness == 1)
                    {
                        GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.Euler(0,-90,0));
                    }
                        
                    //Traffic Light Generation
                    if (trackCount % 5 == 0) 
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
                    
                    initialPosition = new Vector3((scale * 26.58f), 0, (scale * 16f));

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
                    
                    //Booster Track Generation
                    if ((trackCount > 0 && trackCount % 4 == 0)  && boosterRandomness == 1)
                    {
                        GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.Euler(0,-90,0));
                    }
                        
                    //Traffic Light Generation
                    if ((trackCount > 0 && trackCount % 5 == 0))
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
                    
                    initialPosition = new Vector3(-(scale * 16f), 0, (scale * 26.58f));
                    
                    
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
                    if ((trackCount > 0 && trackCount % 4 == 0 && boosterRandomness == 1) )
                    {
                        GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.identity);
                    }
                    
                    
                    //Traffic Light Generation
                    if (trackCount % 5 == 0) 
                    {
                        Instantiate(trafficLightObj, trafficLightPosLeft, Quaternion.Euler(0,270,0), transform);
                        Instantiate(trafficLightObj, trafficLightPosRight, Quaternion.Euler(0,270,0), transform);
                    }
                    
               
                    Instantiate(upNeighbours[0], newPosition, Quaternion.identity, transform);
                    GenerateNeighbours(TrackType.StraightForward, newPosition);
                    break;
                }
                
                case TrackType.DownCurvedRight:
                {
                    upNeighbours = new GameObject[] { straightForwardTrackObj };
                    
                    initialPosition = new Vector3((scale * 16f), 0, (scale * 26.58f));
                    
                    
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
                    if ((trackCount > 0 && trackCount % 4 == 0 && boosterRandomness == 1) )
                    {
                        GenerateTrack(TrackType.BoosterTrack, boosterPosition, Quaternion.identity);
                    }
                    
                        
                    //Traffic Light Generation
                    if ((trackCount > 0 && trackCount % 5 == 0))
                    {
                        Instantiate(trafficLightObj, trafficLightPosLeft, Quaternion.Euler(0,270,0), transform);
                        Instantiate(trafficLightObj, trafficLightPosRight, Quaternion.Euler(0,270,0), transform);
                    }
                    
                 
                    Instantiate(upNeighbours[0], newPosition, Quaternion.identity, transform);
                    GenerateNeighbours(TrackType.StraightForward, newPosition);
                    break;
                    
                }

                case TrackType.JunctionTrack:
                {
                    upNeighbours = new GameObject[] { straightForwardTrackObj };
                    leftNeighbours = new GameObject[] { straightLeftTrackObj };
                    rightNeighbours = new GameObject[] { straightRightTrackObj };
                    initialPosition = new Vector3(0, 0, (scaleFactor * 2));

                    randIndex = Random.Range(0, 3);
                    
                    newPosition = new Vector3(
                        prevPosition.x + initialPosition.x, 
                        prevPosition.y + initialPosition.y, 
                        prevPosition.z + initialPosition.z
                    );

                    switch (randIndex)
                    {
                        case 0:
                        {
                            Instantiate(upNeighbours[0], newPosition, Quaternion.identity, transform);
                            GenerateNeighbours(TrackType.StraightForward, newPosition);
                            break;
                        }

                        case 1:
                        {
                            initialPosition = new Vector3((-scaleFactor * 2), 0, 0);
                    
                            newPosition = new Vector3(
                                prevPosition.x + initialPosition.x, 
                                prevPosition.y + initialPosition.y, 
                                prevPosition.z + initialPosition.z
                            );
                            
                            Instantiate(leftNeighbours[0], newPosition, Quaternion.identity, transform);
                            GenerateNeighbours(TrackType.StraightLeft, newPosition);

                            break;
                        }

                        case 2:
                        {
                            initialPosition = new Vector3((scaleFactor * 2), 0, 0);
                    
                            newPosition = new Vector3(
                                prevPosition.x + initialPosition.x, 
                                prevPosition.y + initialPosition.y, 
                                prevPosition.z + initialPosition.z
                            );

                            Instantiate(rightNeighbours[0], newPosition, Quaternion.identity, transform);
                            GenerateNeighbours(TrackType.StraightRight, newPosition);
                            break;
                        }
                    }
                    
                    break;
                    
                }
                
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
