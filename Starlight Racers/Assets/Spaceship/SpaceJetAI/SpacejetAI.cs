using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpacejetAI : MonoBehaviour
{
    public float thrust = 10000.0f;
    private float lerpedSpeed = 4.0f;
    //private float decelSpeed = 3.0f;

    private float horizontalInput;
    private float verticalInput;

    private bool isAccelerating = false;

    //private float turnSmoothTime = 0.1f;
    //private float turnSmoothVelocity = 0.1f;

    public GameObject lasersPrefab;
    public GameObject laserGun1;
    public GameObject laserGun2;

    private bool canStart;
    [SerializeField] private float laserSpeed = 20f;

    public float responsiveness = 15f;

    private float responsiveFactor;

    [SerializeField] private int laserAmmoMax;
    private int laserAmmo;

    private Vector3 prevVelocity;

    private float banking_angle;

    private bool isBoosting = false;

    float ammoRefillTimer;

    private GameObject currentCheckpoint;
    public float currentRacerDistance;

    //This is the HP stat of the spaceJet
    private int currentShieldStat;
    [SerializeField] private int shieldStatMax;
    private bool isVulnerable;

    private bool takeDamage = true;
    private int checkpointCount; //Counts the amount of times the racer has passed through the checkpoint

    private NavMeshAgent spaceJetAgent;
    
    private TrackGen currentTrack; //currentTrack
    private List<GameObject> currentTrackCheckpoints;
    
    private float finishTime = 0;
    private bool hasFinished = true;

    private float Ypos;
    [SerializeField] private int index = 0;
    private Vector3 finishPos;
    private GameObject finishObj;


    private void Awake()
    {
        Ypos = transform.position.y;
    }
    // Start is called before the first frame update

    void Start()
    {
        ammoRefillTimer = 0; //set our timer to 0
        checkpointCount = 0;
        isVulnerable = false;
        currentCheckpoint = new GameObject();
        currentShieldStat = shieldStatMax; //set the HP value to the max value
        RaceManager.GameStarted += OnGameStarted;
        spaceJetAgent = new NavMeshAgent();
        spaceJetAgent = gameObject.GetComponent<NavMeshAgent>();

        currentTrack = FindObjectOfType<TrackGen>();
        currentTrackCheckpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint"));

        finishObj = GameObject.FindGameObjectWithTag("Finish");
        finishPos = finishObj.transform.position;


    }

    void OnGameStarted()
    {
        laserAmmo = laserAmmoMax; //set laserAmmo to the max value
        isVulnerable = false;
        isAccelerating = true;
        takeDamage = false;
        hasFinished = false;
        spaceJetAgent.enabled = true;
        spaceJetAgent.updateUpAxis = false;
    }

    void FixedUpdate()
    {
        
        // //If the jet is currently accelerating then allow user movement and update each second.
        // if (isAccelerating)
        // {
        //     rb.AddForce(Vector3.Lerp(Vector3.zero,(transform.forward * thrust), Time.deltaTime * 200f));
        //     
        //     /*
        //      *  # Implement Banking as described:
        //     # https://www.cs.toronto.edu/~dt/siggraph97-course/cwr87/
        //     var temp_up = global_transform.basis.y.lerp(Vector3.UP + (acceleration * banking), delta * 5.0)
        //     look_at(global_transform.origin - vel.normalized(), temp_up)
        //      */
        //     var acceleration = (rb.velocity - prevVelocity) / Time.fixedDeltaTime;
        //     rb.AddTorque(transform.up * (0.8f * horizontalInput), ForceMode.Acceleration);
        //     
        //     //add in banking
        //     //banking_angle = rb.angularVelocity.z;
        //     //Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (acceleration * banking_angle), Time.deltaTime * 5.0f);
        //     //transform.LookAt(transform.position - rb.velocity.normalized,tempUp);
        //     prevVelocity = rb.velocity; //update our previous velocity
        // }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        //if the player has not finished the race, start timer
        if (hasFinished == false)
        {
            finishTime += Time.deltaTime;
            
            Ypos = transform.position.y;
            
            if (index < currentTrackCheckpoints.Count)
            {
                var position = currentTrackCheckpoints[index].transform.position;
                
                var destination = new Vector3(position.x, Ypos, position.z);
                Debug.Log("Destination Path: " + destination);
                spaceJetAgent.SetDestination(destination);
                
                ammoRefillTimer += Time.deltaTime;
                
                AmmoRefill(ammoRefillTimer);
            
                if (ammoRefillTimer >= 10f)
                {
                    ammoRefillTimer = 0;
                }
            
                if (Vector3.Distance(transform.position, destination) <= 10f)
                {
                    index++;
                }
            }
            else
            {
                var finishDestination = new Vector3(finishPos.x, Ypos, finishPos.z);
                spaceJetAgent.SetDestination(finishDestination);
            }
        }
    }
    
    private void AmmoRefill(float ammoTimer)
    {
        if(laserAmmo < laserAmmoMax && ammoTimer >= 10f)
        {
            laserAmmo++;
        }
    }
    
    public void FireLaser()
    {
        var offset = new Vector3(0, 0, 10);
        GameObject laser1 = Instantiate(lasersPrefab, laserGun1.transform.position + offset, laserGun1.transform.rotation);
        GameObject laser2 = Instantiate(lasersPrefab, laserGun2.transform.position + offset, laserGun2.transform.rotation);
        
        Debug.Log("Lasers shot");
        //laser1.gameObject.GetComponent<Rigidbody>().velocity += laserGun1.transform.forward * laserSpeed  + rb.velocity;
        //laser2.gameObject.GetComponent<Rigidbody>().velocity += laserGun2.transform.forward * laserSpeed  + rb.velocity;

        laserAmmo--;
    }
    
    //IEnumerator function to apply a temporary speed boost to player for 2 seconds
    IEnumerator ApplyBoost()
    {
        isBoosting = true;
        
        //rb.AddForce(transform.forward * (1.5f * thrust), ForceMode.Impulse);
        
        yield return new WaitForSeconds(2f);
        
        isBoosting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckHealthValue();
        
        if (other.gameObject.CompareTag("Booster") && isBoosting == false)
        {
            StartCoroutine(ApplyBoost());
        }
        
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            checkpointCount++;
            currentCheckpoint = other.gameObject;
        }
        
        //If user makes contact with a laser, it should take a certain amount of damage
        if (other.gameObject.CompareTag("Laser") && takeDamage == false)
        {
           currentShieldStat -= 5;
             takeDamage = true;
             StartCoroutine(Damaged());
        }
        
        //if the racer touches the finish track
        if (other.CompareTag("Finish"))
        {
            hasFinished = true;
        }
    }

    public int ReturnCurrentCheckpointCount()
    {
        return checkpointCount;
    }
    
    public GameObject ReturnCurrentCheckpoint()
    {
        return currentCheckpoint;
    }

    IEnumerator Damaged()
    {
        if (takeDamage)
        {
            yield return new WaitForSeconds(10);
            takeDamage = false;
        }
    }
    
    private void CheckHealthValue()
    {
        if (currentShieldStat == 0)
        {
            Debug.Log("Shield is empty. Warning beware of damage");
            isVulnerable = true;
        }
        else if (isVulnerable && currentShieldStat < 0)
        {
            Destroy(gameObject);
            Debug.Log(gameObject.name + " has been eliminated from the race");
        }
    }


}
