using System;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;


public class PlayerMultiplayer : NetworkBehaviour
{
    [SerializeField] Animator anim;
    CharacterController controller;

    [SerializeField] private Transform cam;
    

    [SerializeField] private PlayerController Controller;
    
    [SerializeField] private CinemachineFreeLook freeLook;
    
    [SerializeField] private CinemachineFreeLook freeLook1;
    [SerializeField] private CinemachineFreeLook freeLook2;
    public enum Character
    {
        Amy,
        Aj
    }

    public Character characterType;
    
    private bool _isPaused;
    //int isWalkingHash = Animator.StringToHash("isWalking");
    //int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int playerSpeed = Animator.StringToHash("speed");
    private readonly int isJumpingHash = Animator.StringToHash("isJumping");
    private readonly int isGroundedHash = Animator.StringToHash("isGrounded");

    public float jumpHeight = 3f;

    [SerializeField]
    private float walkSpeed = 4f;
    
    [SerializeField]
    private float runningSpeed = 10f;

    private float animSpeed;
    private readonly float walkAnimSpeed = 0.5f;
    private readonly float runAnimSpeed = 1;

    private Camera camera;
    
    
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity = 0.1f;

    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 initialVel;
    bool isGrounded;
    
    private float forwardInput;
    private float horizontalInput;

    private bool gravityCheck = true;

    private bool isRunning;
    private bool jumpNow;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();

        camera = cam.gameObject.GetComponent<Camera>();
        Controller = new PlayerController();
        Controller.Enable();
        
        Controller.IntermissionScene.Run.performed += _ => isRunning = true;
        Controller.IntermissionScene.Run.canceled += _ => isRunning = false;

        Controller.IntermissionScene.Jump.performed += _ => Jump();
        Controller.IntermissionScene.Jump.canceled += _ => Jump();
        
        // freeLook1 = GameObject.FindWithTag("CinemachineAmy").GetComponent<CinemachineFreeLook>();
        // freeLook2 = GameObject.FindWithTag("CinemachineAj").GetComponent<CinemachineFreeLook>();
        
        // switch (characterType)
        // {
        //     case Character.Amy:
        //     {
        //         break;
        //     }
        //
        //     case Character.Aj:
        //     {
        //         
        //         break;
        //     }
        // }
        
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            freeLook.Priority = 10;
        }
        else
        {
            freeLook.Priority = 0;
            camera.depth = -1;
        }
        
        // switch (characterType)
        // {
        //     case Character.Amy:
        //     {
        //         freeLook1.Follow = transform;
        //         freeLook1.LookAt = transform;
        //         break;
        //     }
        //
        //     case Character.Aj:
        //     {
        //         freeLook2.Follow = transform;
        //         freeLook2.LookAt = transform;
        //         break;
        //     }
        // }
        
        // freeLook.Follow = transform;
        // freeLook.LookAt = transform;
        
        anim.SetFloat(playerSpeed, 0);
    }
    
    private void OnDisable()
    {
        Controller.Disable();
    }

    
    void HandleInput()
    {
        if(!IsOwner) return;
        
        //Get input axes (Vertical)
        Controller.IntermissionScene.Movement.performed += context => forwardInput = context.ReadValue<float>();
        Controller.IntermissionScene.Movement.canceled += context => forwardInput = forwardInput = 0f;
        
        //Get input axes (Horizontal)
        Controller.IntermissionScene.Turn.performed += context => horizontalInput = context.ReadValue<float>();
        Controller.IntermissionScene.Turn.canceled += context => horizontalInput = horizontalInput = 0f;
    }
    
    void Update()
    {
        Movement();
    }
    
    void Movement()
    {
        HandleInput();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        anim.SetBool(isGroundedHash, isGrounded);

        float tempAnimSpeed = 0;
        
        
        if(isGrounded && initialVel.y < 0)
        {
            initialVel.y = -2f;
        }
        
        
        Vector3 dir = new Vector3(horizontalInput, 0f, forwardInput);

        if (dir.magnitude >= 0.1f && DialogueManager.inDialogue == false)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float turningAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, turningAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            

            if (isRunning)
            {
                animSpeed = runAnimSpeed;
                tempAnimSpeed = Mathf.Lerp(animSpeed, 0, Time.deltaTime);
                controller.Move(moveDir.normalized * (runningSpeed * Time.deltaTime));
                
                anim.SetFloat(playerSpeed, tempAnimSpeed);
                

            }
            else
            {
                animSpeed = walkAnimSpeed;
                tempAnimSpeed = Mathf.Lerp(animSpeed, 0, Time.deltaTime);
                controller.Move(moveDir.normalized * (walkSpeed * Time.deltaTime));
                
                anim.SetFloat(playerSpeed, tempAnimSpeed);
            }
        }
        else
        {
             tempAnimSpeed = Mathf.Lerp(0, animSpeed, Time.deltaTime);
             anim.SetFloat(playerSpeed, tempAnimSpeed);
        }

        if (gravityCheck)
        {
            initialVel.y += gravity * Time.deltaTime;
            controller.Move(initialVel * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if (isGrounded && BoosterShopkeeper.ActivateBoosterMenu == false && CoinShopKeeper.ActivateCoinMenu == false)
        {
            anim.SetTrigger(isJumpingHash);
            initialVel.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    
    
}
