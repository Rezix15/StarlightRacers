using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Spacejet : MonoBehaviour
{
    # region grounded
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    private bool isGrounded;
    public LayerMask groundMask;
    # endregion
    
    private Stat thrust;
    private float grip;
    // private float spaceJetSpeed;
    // private float spaceJetShieldRate;
    // private float spaceJetShieldMax;
    // private float spaceJetLaserDmg;

    public SpaceJetStats spaceJetStat;

    private int boosterPadSpeed;
    
    // = new Stat(0)

    [SerializeField] private Stat speed;
    
    public Stat shieldMax; //Health
    
    [SerializeField]
    private Stat laserDamage; //LaserDamage
    
    [SerializeField]
    private Stat shieldRate; //Defense Stat

    private float lerpedSpeed = 4.0f;
    
    private PlayerController Controller;
    private Vector3 playerMovement;
    private float forwardInput;
    private float horizontalInput;
    private bool isAccelerating;
    private Rigidbody rb;


    public bool canMove = false;
    public GameObject lasersPrefab;
    public GameObject laserGun1;
    public GameObject laserGun2;

    [SerializeField] private float laserSpeed = 20f;

    // public float responsiveness = 15f;
    // private float responsiveFactor;

    float timer;
    private float shieldTimer;
    private float laserAmmo;

    private bool isBoosting = false;

    private Vector3 prevVelocity;

    private float banking_angle;
    
    private GameObject currentCheckpoint;
    public float currentRacerDistance;
    public GameObject[] pauseOptions;
    
    //This is the HP stat of the spaceJet
    [SerializeField]
    private float currentShieldStat;

    private bool isVulnerable = false;

    private bool takeDamage = true; //Bool to check, if the user can be damaged

    private int checkpointCount; //Counts the amount of times the racer has passed through the checkpoint

    private bool shieldBoostPressed; //checks the input for the shieldBoost
    [SerializeField] private float finishTime = 0; //player finish time
    public bool hasFinished;

    private float distToNextCheckpoint;

    private Modifier componentModifier;

    //Ability gauge that is needed to use the special abiility
    private int abilityGauge = 0;

    #region CreationAbilityRegion
    
    private CreationAbility creationAbility;
    public GameObject shieldEffect;
    public GameObject bomb;
    private bool isShieldActive = false;
    private Modifier shieldPowerUp = new Modifier(1);
    private bool wasShieldActive = false;

    private bool abilityActive = false;
    #endregion
    
    float boostTimer = 0;
    
    private GhostAbility GhostAbility;
    
    public bool isPlayer1;
    public bool isPlayer2;
    
    private void Awake()
    {
        Controller = new PlayerController();
        InitializeStats();
        
        if (IntermissionMenu.currentComponent != null)
        {
            UpgradeComponents();
        }

        if (isPlayer1)
        {
            Controller.Player1.Accelerate.performed += _ => isAccelerating = true;
            Controller.Player1.Accelerate.canceled += _ => isAccelerating = false;

            Controller.Player1.ShootLeft.performed += _ => FireLaserLeft();
            Controller.Player1.ShootLeft.canceled += _ => FireLaserLeft();
        
            Controller.Player1.ShootRight.performed += _ => FireLaserRight();
            Controller.Player1.ShootLeft.canceled += _ => FireLaserRight();

            Controller.Player1.Boost.performed += _ => shieldBoostPressed = true;
            Controller.Player1.Boost.canceled += _ => shieldBoostPressed = false;
        
            Controller.Player1.SpecialAbility.performed += _ => UseAbility();
            Controller.Player1.SpecialAbility.canceled += _ => UseAbility();

            Controller.Player.Pause.performed += _ => PauseGame();
            Controller.Player.Pause.canceled += _ => PauseGame();
            
            
        }
        else if(isPlayer2)
        {
            Controller.Player2.Accelerate.performed += _ => isAccelerating = true;
            Controller.Player2.Accelerate.canceled += _ => isAccelerating = false;

            Controller.Player2.ShootLeft.performed += _ => FireLaserLeft();
            Controller.Player2.ShootLeft.canceled += _ => FireLaserLeft();
        
            Controller.Player2.ShootRight.performed += _ => FireLaserRight();
            Controller.Player2.ShootLeft.canceled += _ => FireLaserRight();

            Controller.Player2.Boost.performed += _ => shieldBoostPressed = true;
            Controller.Player2.Boost.canceled += _ => shieldBoostPressed = false;
        
            Controller.Player2.SpecialAbility.performed += _ => UseAbility();
            Controller.Player2.SpecialAbility.canceled += _ => UseAbility();
        }
        else
        {
            Controller.Player.Accelerate.performed += _ => isAccelerating = true;
            Controller.Player.Accelerate.canceled += _ => isAccelerating = false;

            Controller.Player.ShootLeft.performed += _ => FireLaserLeft();
            Controller.Player.ShootLeft.canceled += _ => FireLaserLeft();
        
            Controller.Player.ShootRight.performed += _ => FireLaserRight();
            Controller.Player.ShootLeft.canceled += _ => FireLaserRight();

            Controller.Player.Boost.performed += _ => shieldBoostPressed = true;
            Controller.Player.Boost.canceled += _ => shieldBoostPressed = false;
        
            Controller.Player.SpecialAbility.performed += _ => UseAbility();
            Controller.Player.SpecialAbility.canceled += _ => UseAbility();
            
            Controller.Player.Pause.performed += _ => PauseGame();
        }
        

    }

    private void OnEnable()
    {
        Controller.Enable();
    }

    private void PauseGame()
    {
        CanvasManager.gamePaused = !CanvasManager.gamePaused;
        switch (CanvasManager.gamePaused)
        {
            case true:
            {
                Time.timeScale = 0;
                EventSystem.current.SetSelectedGameObject(pauseOptions[0]);
                break;
            }
            case false:
            {
                Time.timeScale = 1;
                break;
            }
        }
    }

    private void OnDisable()
    {
        Controller.Disable();
    }
    // Start is called before the first frame update

    //Initialize the stats of the spaceJet using the current vehicle that was chosen in the menu.
    private void InitializeStats()
    {
        if(isPlayer2)
        {
            thrust = new Stat(MenuManager.enemySpaceJet.thrust);
            grip = MenuManager.enemySpaceJet.grip;
            speed = new Stat(MenuManager.enemySpaceJet.speed);
            shieldMax = new Stat(MenuManager.enemySpaceJet.shield);
            shieldRate = new Stat(MenuManager.enemySpaceJet.shieldRate);
            laserDamage = new Stat(MenuManager.enemySpaceJet.laserDamage);
        }
        else
        {
            thrust = new Stat(MenuManager.currentSpaceJet.thrust);
            grip = MenuManager.currentSpaceJet.grip;
            speed = new Stat(MenuManager.currentSpaceJet.speed);
            shieldMax = new Stat(MenuManager.currentSpaceJet.shield);
            shieldRate = new Stat(MenuManager.currentSpaceJet.shieldRate);
            laserDamage = new Stat(MenuManager.currentSpaceJet.laserDamage);
        }
        
    }
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        isVulnerable = false;
        timer = 0; //set our timer to 0
        checkpointCount = 0;
        currentCheckpoint = new GameObject();
        hasFinished = false;
        RaceManager.GameStarted += OnGameStart;
        boosterPadSpeed = 25000;

        
        if(isPlayer2)
        {
            switch (MenuManager.enemySpaceJet.name)
            {
                case "Absorber":
                {
                    creationAbility = gameObject.AddComponent<CreationAbility>();
                
                    creationAbility.Initialize("Transmogrifier", 30f, 
                        SpecialAbility.AbilityTypes.Effect, shieldEffect, bomb);
                    break;
                }
            
                case "UFO": 
                {
                    break;
                }
            
                case "Bolt Glider": 
                {
                    break;
                }
            
                case "Ghost Rider": 
                {
                    break;
                }
            }
        }
        else
        {
            switch (MenuManager.currentSpaceJet.name)
            {
                case "Absorber":
                {
                    creationAbility = gameObject.AddComponent<CreationAbility>();
                
                    creationAbility.Initialize("Transmogrifier", 30f, 
                        SpecialAbility.AbilityTypes.Effect, shieldEffect, bomb);
                    break;
                }
            
                case "UFO": 
                {
                    break;
                }
            
                case "Bolt Glider": 
                {
                    break;
                }
            
                case "Ghost Rider": 
                {
                    break;
                }
            }
        }
        
    }

    void OnGameStart()
    {
        canMove = true;
        laserAmmo = GameDataManager.laserAmmoMax; //set laserAmmo to the max value
        currentShieldStat = shieldMax.trueValue; //set the HP value to the max value
        takeDamage = false;
    }

    

    // void UpdateGravity()
    // {
    //     isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    //
    //     var gravity = -3000f;
    //
    //     if (isGrounded && rb.velocity.y < 0)
    //     {
    //         var velocity = rb.velocity;
    //         velocity = new Vector3(velocity.x, -2f, velocity.z);
    //         rb.velocity = velocity;
    //     }
    //
    //     rb.velocity += Vector3.up * (gravity * Time.deltaTime);
    // }
    void Update()
    {
        //UpdateGravity();

        // if (isPlayer1)
        // {
        //     Debug.Log("P1: HorizontalInput" + horizontalInput);
        // }
        // else if(isPlayer2)
        // {
        //     Debug.Log("P2: HorizontalInput" + horizontalInput);
        // }
        
        if (abilityGauge < GameDataManager.abilityGaugeMax)
        {
            abilityGauge++;
        }
        
        //If the current Laser Ammo is below the max, start a timer for every 10 seconds to refill ammo
        if (laserAmmo < GameDataManager.laserAmmoMax)
        {
            timer += Time.deltaTime;
            
            AmmoRefill(timer);

            if (timer >= GameDataManager.refillSeconds)
            {
                timer = 0;
            }

        }
        
        
        if (currentShieldStat < shieldMax.trueValue && !shieldBoostPressed)
        {
            shieldTimer += Time.deltaTime;
            
            ShieldRefill(shieldTimer);

            if (shieldTimer >= GameDataManager.refillSeconds * 3)
            {
                shieldTimer = 0;
            }
        }
        
        AddShieldPowerUp();

        //if the player has not finished the race, start timer
        if (hasFinished == false && canMove)
        {
            finishTime += Time.deltaTime;
            
            // if (timer >= MenuManager.timelimitVal)
            // {
            //     switch (MenuManager.currentStageId)
            //     {
            //         case 0:
            //         {
            //             SceneManager.LoadScene("IntermissionScene");
            //             break;
            //         }
            //
            //         case 1:
            //         {
            //             SceneManager.LoadScene("IntermissionScene(CandyLand)");
            //             break;
            //         }
            //
            //         default:
            //         {
            //             SceneManager.LoadScene("IntermissionScene");
            //             break;
            //         }
            //     }
            // }
        }
        
    }

    private void AddShieldPowerUp()
    {
        var shield = FindObjectOfType<ShieldEffect>();

        if (shield != null && isShieldActive == false && wasShieldActive == false)
        {
            shieldPowerUp = new Modifier(shield.shieldModifierBonus);
            shieldRate.AddModifier(shieldPowerUp);
            isShieldActive = true;
            abilityActive = true;
            wasShieldActive = true;
        }
        else if(shield == null && wasShieldActive)
        {
            shieldRate.RemoveModifier(shieldPowerUp);
            isShieldActive = false;
            abilityActive = false;
            wasShieldActive =false;
        }
    }

    // void Gravity()
    // {
    //     var mass = rb.mass;
    //     rb.AddForce(Physics.gravity * (mass * mass));
    // }

    void HandleInput()
    {
        if (!canMove)
            return;

        if (isPlayer1)
        {
            Controller.Player1.Movement.performed += context => forwardInput = context.ReadValue<float>();
            Controller.Player1.Movement.canceled += context => forwardInput = forwardInput = 0f;
        
            //Get input axes
            Controller.Player1.Turn.performed += context => horizontalInput = context.ReadValue<float>();
            Controller.Player1.Turn.canceled += context => horizontalInput = horizontalInput = 0f;
        }
        else if (isPlayer2)
        {
            Controller.Player2.Movement.performed += context => forwardInput = context.ReadValue<float>();
            Controller.Player2.Movement.canceled += context => forwardInput = forwardInput = 0f;
        
            //Get input axes
            Controller.Player2.Turn.performed += context => horizontalInput = context.ReadValue<float>();
            Controller.Player2.Turn.canceled += context => horizontalInput = horizontalInput = 0f;
        }
        else
        {
            Controller.Player.Movement.performed += context => forwardInput = context.ReadValue<float>();
            Controller.Player.Movement.canceled += context => forwardInput = forwardInput = 0f;
        
            //Get input axes
            Controller.Player.Turn.performed += context => horizontalInput = context.ReadValue<float>();
            Controller.Player.Turn.canceled += context => horizontalInput = horizontalInput = 0f;
        }
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Gravity();

        if (!CanvasManager.gamePaused)
        {
            HandleInput();
            //If the jet is currently accelerating then allow user movement and update each second.
            if (isAccelerating && forwardInput > 0)
            {
                rb.AddForce(Vector3.Lerp(Vector3.zero,(transform.forward * speed.trueValue), Time.deltaTime * thrust.trueValue));
            }
        
            //If the user is still pressing the acceleration button but also holding the stick in the negative direction, reverse:
            else if (isAccelerating && forwardInput < 0 )
            {
                rb.AddForce(Vector3.Lerp(Vector3.zero,(-transform.forward * speed.trueValue), Time.deltaTime * thrust.trueValue));
            }
        
            //var acceleration = (rb.velocity - prevVelocity) / Time.fixedDeltaTime;
            rb.AddTorque(transform.up * (grip * horizontalInput ), ForceMode.Acceleration);
            
            //If the user turns either left or right, the gameobject should tilt 45 degrees to the corresponding direction
            /*
             *  # Implement Banking as described:
            # https://www.cs.toronto.edu/~dt/siggraph97-course/cwr87/
            var temp_up = global_transform.basis.y.lerp(Vector3.UP + (acceleration * banking), delta * 5.0)
            look_at(global_transform.origin - vel.normalized(), temp_up)
             */

            //add in banking
            //banking_angle = rb.angularVelocity.z;
            //Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (acceleration * banking_angle), Time.deltaTime * 5.0f);
            //transform.LookAt(transform.position - rb.velocity.normalized,tempUp);
            
            prevVelocity = rb.velocity; //update our previous velocity
        
            ShieldBoost();
        }
        
        
    }
    
    //Function to switch the angle of the player
    private void ShiftAngle(float angleVal)
    {
        transform.rotation = Quaternion.Euler(angleVal, 0, 0);
    }

    //Function that sacrifices the shield gauge to increase speed. If user shield expires when boosting, destroy player
    private void ShieldBoost()
    {
        // Debug.Log("shieldRate: " + shieldRate.trueValue);
        
        if (currentShieldStat > 0 && shieldBoostPressed)
        {
            rb.AddForce(transform.forward * (2.5f * speed.trueValue), ForceMode.Force);
            //currentShieldStat-= (1 - (0.4f * shieldRate.trueValue));

            //currentShieldStat -= 0.4f * (1 - (Mathf.Clamp(shieldRate.trueValue, 0f, 4f) / 100));

            currentShieldStat -= 0.4f * (1 - Mathf.Clamp(shieldRate.trueValue / 100, 0f, 0.8f));
        }
        else if(currentShieldStat <= 0 && boostTimer <= 5f && shieldBoostPressed)
        {
            boostTimer += Time.deltaTime;
        }
        else if(currentShieldStat <= 0 && shieldBoostPressed)
        {
            //Debug.Log("Player has been eliminated from the race");
            Destroy(gameObject);
        }
    }

    private void UpgradeComponents()
    {
        if (MenuManager.componentBoosts != null)
        {
            foreach (var componentBoost in MenuManager.componentBoosts)
            {
                componentModifier = new Modifier(componentBoost.statModifierVal);
                
                // Debug.Log("Components Count: " + MenuManager.componentBoosts.Count);
                // Debug.Log("Component Name: " + componentBoost.name);
                // Debug.Log("Component TargetedStat: " + componentBoost.targetStat);
                // Debug.Log("Components ModifierVal: " + componentBoost.statModifierVal);
                
                switch (componentBoost.targetStat)
                {
                    case ComponentObj.StatSkillType.Speed:
                    {
                        speed.AddModifier(componentModifier);
                        break;
                    }

                    case ComponentObj.StatSkillType.ShieldRate:
                    {
                        shieldRate.AddModifier(componentModifier);
                        break;
                    }
            
                    case ComponentObj.StatSkillType.Shield:
                    {
                        shieldMax.AddModifier(componentModifier);
                        break;
                    }

                    case ComponentObj.StatSkillType.Grip:
                    {
                        break;
                    }

                    case ComponentObj.StatSkillType.Thrust:
                    {
                        thrust.AddModifier(componentModifier);
                        break;
                    }

                    case ComponentObj.StatSkillType.LaserDamage:
                    {
                        laserDamage.AddModifier(componentModifier);
                        break;
                    }
            

                    default:
                    {
                        break;
                    }
                }
            }
        }
    }
    
    //Function to shoot Laser when q is pressed
    private void FireLaserLeft()
    {
        if (laserAmmo != 0 && !CanvasManager.gamePaused)
        {
            var offset = new Vector3(0, 0, 10);
            GameObject laser1 = Instantiate(lasersPrefab, laserGun1.transform.position + offset, laserGun1.transform.rotation);
            laser1.gameObject.GetComponent<Rigidbody>().velocity += laserGun1.transform.forward * laserSpeed  + rb.velocity;

            if (GameDataManager.halfTime)
            {
                laserAmmo -= 0.5f;
            }
            else
            {
                laserAmmo--;
            }
        }
    }

    private void FireLaserRight()
    {
        if (laserAmmo != 0 && !CanvasManager.gamePaused)
        {
            var offset = new Vector3(0, 0, 10);
            GameObject laser2 = Instantiate(lasersPrefab, laserGun2.transform.position + offset, laserGun2.transform.rotation);
            laser2.gameObject.GetComponent<Rigidbody>().velocity += laserGun2.transform.forward * laserSpeed  + rb.velocity;
            
            if (GameDataManager.halfTime)
            {
                laserAmmo -= 0.5f;
            }
            else
            {
                laserAmmo--;
            }
        }
    }

    //IEnumerator function to apply a temporary speed boost to player for 2 seconds. Used for boost pads present
    //on the tracks
    IEnumerator ApplyBoost()
    {
        if (!CanvasManager.gamePaused)
        {
            isBoosting = true;
        
            rb.AddForce(transform.forward * (1.5f * boosterPadSpeed), ForceMode.Impulse);
        
            yield return new WaitForSeconds(2f);
        
            isBoosting = false;
        }
        
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
            
            //If the order of the checkpoint is correct..
            if (currentCheckpoint == TrackGen.checkpoints[checkpointCount])
            {
                checkpointCount++;
            }
        }
        
        if (other.gameObject.CompareTag("DownTrack"))
        {
            ShiftAngle(45);
        }
        
        if (other.gameObject.CompareTag("ForwardTrack"))
        {
            ShiftAngle(0);
        }

        //If user makes contact with a laser, it should take a certain amount of damage
        if (other.gameObject.CompareTag("Laser") && takeDamage == false)
        {
            currentShieldStat -= 5 * (1 - (shieldRate.trueValue / 100));
            takeDamage = true;
            StartCoroutine(Damaged());
        }

        if (other.CompareTag("Finish"))
        {
            hasFinished = true;
        }
    }

    //if user collides with the wall
    public void OnCollisionEnter(Collision other)
    {
        CheckHealthValue();
        
        if (other.gameObject.layer == 7)
        {
            currentShieldStat -= 0.4f * (1 - Mathf.Clamp(shieldRate.trueValue / 100, 0f, 0.8f));
        }

        if (other.gameObject.CompareTag("Gate"))
        {
            currentShieldStat -= (0.4f * (1 - Mathf.Clamp(shieldRate.trueValue / 100, 0f, 0.8f)) * 10);
        }

        if (other.gameObject.CompareTag("RobotEnemy"))
        {
            currentShieldStat -= 0.4f * (5 - Mathf.Clamp(shieldRate.trueValue / 100, 0f, 0.8f));
        }
    }

    //Auto refills the lasers amount by 1 when a specific amount of time has passed
    private void AmmoRefill(float ammoTimer)
    {
        if(laserAmmo < GameDataManager.laserAmmoMax && ammoTimer >= GameDataManager.refillSeconds)
        {
            laserAmmo++;

            if (laserAmmo > GameDataManager.laserAmmoMax)
            {
                laserAmmo = GameDataManager.laserAmmoMax;
            }
        }
    }

    private void ShieldRefill(float ammoTimer)
    {

        if (currentShieldStat < shieldMax.trueValue && ammoTimer >= (GameDataManager.refillSeconds * 3) &&
            GameDataManager.restoreShield)
        {
            currentShieldStat += (shieldMax.trueValue * 0.05f);
        }
    }

    //Return the current ammo count 
    //Return the current ammo count 
    public float GetLaserAmmoCount()
    {
        return laserAmmo;
    }

    public int ReturnCurrentCheckpointCount()
    {
        return checkpointCount;
    }

    private void UseAbility()
    {
        if (abilityGauge >= GameDataManager.abilityGaugeMax && creationAbility != null && abilityActive == false && !CanvasManager.gamePaused)
        {
            abilityActive = true;
            creationAbility.UseAbility();
            abilityActive = false;
            abilityGauge = 0;
        }
        else if(abilityGauge < GameDataManager.abilityGaugeMax && creationAbility != null && !CanvasManager.gamePaused)
        {
            Debug.Log("Ability is not ready time remaining: " + abilityGauge);
        }
    }
    
    public float GetCurrentShieldStat()
    {
        return currentShieldStat;
    }

    public GameObject ReturnCurrentCheckpoint()
    {
        return currentCheckpoint;
    }

    public float ReturnFinishTime()
    {
        return finishTime;
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
        if (currentShieldStat == 0 && takeDamage)
        {
            Debug.Log("Player shield is empty. Warning beware of damage");
            isVulnerable = true;
        }
        else if (isVulnerable && takeDamage && currentShieldStat < 0)
        {
            Destroy(gameObject);
            Debug.Log("Player has been eliminated from the race");
        }
    }
    

}
