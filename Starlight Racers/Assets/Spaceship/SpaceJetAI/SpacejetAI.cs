using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SpacejetAI : MonoBehaviour
{
    # region SpaceJet Stats
    private Stat thrust;
    private float grip;
    
    [SerializeField] private Stat speed;
    
    public Stat shieldMax; //Health
    
    [SerializeField]
    private Stat laserDamage; //Defense
    
    [SerializeField]
    private Stat shieldRate; //Defense Stat
    
    # endregion
    
    private float lerpedSpeed = 4.0f;
    //private float decelSpeed = 3.0f;

    //private float turnSmoothTime = 0.1f;
    //private float turnSmoothVelocity = 0.1f;

    public GameObject lasersPrefab;
    public GameObject laserGun1;
    public GameObject laserGun2;

    private bool canStart;
    [SerializeField] private float laserSpeed = 20f;

    // public float responsiveness = 15f;
    //
    // private float responsiveFactor;
    
    private int boosterPadSpeed;

    [SerializeField] private int laserAmmoMax;
    private int laserAmmo;

    private Vector3 prevVelocity;
    private float horizontalInput;
    private float banking_angle;
    private bool isBoosting = false;
    private bool isAccelerating;

    float ammoRefillTimer;

    [SerializeField]
    private GameObject currentCheckpoint;
    
    [SerializeField]
    private int checkpointCount; //Counts the amount of times the racer has passed through the checkpoint
    public float currentRacerDistance;

    //This is the HP stat of the spaceJet
    private int currentShieldStat;
    [SerializeField] private int shieldStatMax;
    private bool isVulnerable;

    private bool takeDamage = true;
    

    private NavMeshAgent spaceJetAgent;
    
    private TrackGen currentTrack; //currentTrack
    private List<GameObject> currentTrackCheckpoints;
    
    private float finishTime = 0;
    private bool hasFinished = true;

    private float Ypos;
    [SerializeField] private int index;
    private Vector3 finishPos;
    private GameObject finishObj;
    
    private Rigidbody rb;

    private int turnAmount;
    RaycastHit raycastHit;

    private bool hasShiftAngle;

    private void Awake()
    {
        Ypos = transform.position.y;
    }
    // Start is called before the first frame update

    void Start()
    {
        InitializeStats();
        ammoRefillTimer = 0; //set our timer to 0
        checkpointCount = 0;
        isVulnerable = false;
        currentCheckpoint = new GameObject();
        currentShieldStat = shieldStatMax; //set the HP value to the max value
        RaceManager.GameStarted += OnGameStarted;
        // spaceJetAgent = new NavMeshAgent();
        // spaceJetAgent = gameObject.GetComponent<NavMeshAgent>();

        // if (spaceJetAgent != null)
        // {
        //     
        // }
        //
        // //spaceJetAgent.enabled = false;

        rb = gameObject.GetComponent<Rigidbody>();
        
        //Initialize the currentCheckpoint to be the first checkpoint in the array
        //currentCheckpoint = TrackGen.checkpoints[0];

        finishObj = GameObject.FindGameObjectWithTag("Finish");
        finishPos = finishObj.transform.position;
    }
    
    private void InitializeStats()
    {
        thrust = new Stat(MenuManager.enemySpaceJet.thrust);
        grip = MenuManager.enemySpaceJet.grip;
        speed = new Stat(MenuManager.enemySpaceJet.speed);
        shieldMax = new Stat(MenuManager.enemySpaceJet.shield);
        shieldRate = new Stat(MenuManager.enemySpaceJet.shieldRate);
        laserDamage = new Stat(MenuManager.enemySpaceJet.laserDamage);
        Debug.Log("Name of vehicle: " + MenuManager.enemySpaceJet.name);
    }

    void OnGameStarted()
    {
        laserAmmo = laserAmmoMax; //set laserAmmo to the max value
        isVulnerable = false;
        isAccelerating = true;
        takeDamage = false;
        hasFinished = false;
        //spaceJetAgent.enabled = true;
        //spaceJetAgent.updateUpAxis = false;
    }
    
    void FixedUpdate()
    {
        
        //If the jet is currently accelerating then allow user movement and update each second.
        if (isAccelerating)
        {
            rb.AddForce(Vector3.Lerp(Vector3.zero,(transform.forward * speed.trueValue), Time.deltaTime * thrust.trueValue));
            
            /*
             *  # Implement Banking as described:
            # https://www.cs.toronto.edu/~dt/siggraph97-course/cwr87/
            var temp_up = global_transform.basis.y.lerp(Vector3.UP + (acceleration * banking), delta * 5.0)
            look_at(global_transform.origin - vel.normalized(), temp_up)
             */
            var acceleration = (rb.velocity - prevVelocity) / Time.fixedDeltaTime;
            Movement(currentCheckpoint.transform.position);
            
            //Debug.Log("HorizontalInput: " + horizontalInput);
            
            //add in banking
            //banking_angle = rb.angularVelocity.z;
            //Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (acceleration * banking_angle), Time.deltaTime * 5.0f);
            //transform.LookAt(transform.position - rb.velocity.normalized,tempUp);
            prevVelocity = rb.velocity; //update our previous velocity
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        //if the player has not finished the race, start timer
        if (hasFinished == false)
        {
            finishTime += Time.deltaTime;
            
            ammoRefillTimer += Time.deltaTime;
            
            AmmoRefill(ammoRefillTimer);
            
            if (ammoRefillTimer >= 10f)
            {
                ammoRefillTimer = 0;
            }
        }
    }
    
    
    

    //Inspired by https://dawn-studio.de/tutorials/boids/
    private void Movement(Vector3 targetPos)
    {
        var initialTargetPos = new Vector3(targetPos.x, Ypos, targetPos.z);
        var nextCheckpoint = TrackGen.checkpoints[checkpointCount + 1].transform;
        var newTargetPos = TrackGen.checkpoints[checkpointCount+1].transform.position;
        
        if (Vector3.Distance(transform.position, initialTargetPos) < 0.1f)
        {
            currentCheckpoint = TrackGen.checkpoints[checkpointCount];
            newTargetPos = nextCheckpoint.position;
        }
        
        if (checkpointCount < TrackGen.checkpoints.Count - 1)
        {
            targetPos = new Vector3(newTargetPos.x, Ypos, newTargetPos.z);
            
            var detectPositionF = transform.position + transform.forward * 40f;
            
            if (Physics.Raycast(detectPositionF, transform.forward, out raycastHit, MenuManager.scaleLevel * 10,
                    LayerMask.GetMask("Wall")))
            {
                Debug.Log("Raycast Hit at: " + raycastHit);
                
                var nextCheckpointRotation = nextCheckpoint.rotation;
                var rotationDif = nextCheckpointRotation.eulerAngles.y - transform.rotation.eulerAngles.y;
                horizontalInput += 2 * Mathf.Sign(rotationDif);
                horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);
                
                rb.AddTorque(transform.up * (grip * horizontalInput), ForceMode.Acceleration);
                
                // transform.LookAt(TrackGen.checkpoints[checkpointCount+1].transform);
                //
                // //Check if the object matches direction of rotation of the next checkpoint
                // if ((transform.rotation.y > 0 && TrackGen.checkpoints[checkpointCount+1].transform.rotation.y > 0) &&
                //     (TrackGen.checkpoints[checkpointCount+1].transform.rotation.y - transform.rotation.y >= 5f || 
                //      TrackGen.checkpoints[checkpointCount+1].transform.rotation.y - transform.rotation.y <= -5f))
                // {
                //     var toTarget = (targetPos - transform.position).normalized;
                //     
                //     horizontalInput += 2 * Mathf.Sign(Vector3.Dot(toTarget, transform.right));
                //     horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);
                //     rb.AddTorque(transform.up * (grip * horizontalInput), ForceMode.Acceleration);
                //
                //     // if (TrackGen.checkpoints[checkpointCount + 1].transform.position.x - transform.position.x >= 30 || 
                //     //     TrackGen.checkpoints[checkpointCount + 1].transform.position.x - transform.position.x <= -30)
                //     // {
                //     //     transform.position += new Vector3(-grip * 20, 0, 0);
                //     // }
                // }
                // else if ((transform.rotation.y < 0 &&
                //           TrackGen.checkpoints[checkpointCount + 1].transform.rotation.y < 0) &&
                //          (TrackGen.checkpoints[checkpointCount + 1].transform.rotation.y - transform.rotation.y <= 5f ||
                //           TrackGen.checkpoints[checkpointCount + 1].transform.rotation.y - transform.rotation.y >= -5f))
                // {
                //     var toTarget = (targetPos - transform.position).normalized;
                //
                //     horizontalInput += 2 * Mathf.Sign(Vector3.Dot(toTarget, transform.right));
                //     horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);
                //     rb.AddTorque(transform.up * (grip * horizontalInput), ForceMode.Acceleration);
                //
                //     // if (TrackGen.checkpoints[checkpointCount + 1].transform.position.x - transform.position.x >= 30 || 
                //     //     TrackGen.checkpoints[checkpointCount + 1].transform.position.x - transform.position.x <= -30)
                //     // {
                //     //     transform.position += new Vector3(grip * 20, 0, 0);
                //     // }
                //     // }
                // }
            }
            else
            {
                    horizontalInput = 0;
            }
            
            
        }
    }
    
    private void ShiftAngle(float angleVal)
    {
        
        if (angleVal > 0 || angleVal < 0)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
        else if(angleVal == 0)
        {
            transform.position = new Vector3(transform.position.x, Ypos, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
        
        transform.rotation = Quaternion.Euler(angleVal, 0, 0);
        
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
            currentCheckpoint = other.gameObject;
            
            if (currentCheckpoint == TrackGen.checkpoints[checkpointCount])
            {
                checkpointCount++;
                turnAmount = 0;
            }
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
        
        if (other.gameObject.CompareTag("DownTrack"))
        {
            ShiftAngle(45);
        }
        
        if (other.gameObject.CompareTag("ForwardTrack"))
        {
            ShiftAngle(0);
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
