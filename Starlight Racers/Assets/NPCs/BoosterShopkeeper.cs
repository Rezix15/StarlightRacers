using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BoosterShopkeeper : MonoBehaviour
{
    public static bool ActivateBoosterMenu;
    
    public static int successfulAttempts;

    public GameObject boosterCardMenu;
    public GameObject teleporter;

    private Camera Camera;
    private CinemachineBrain CinemachineBrain;

    private string dialogue1;
    private PlayerController Controller;

    private bool isTalking;

    private void Awake()
    {
        Controller = new PlayerController();
        Controller.IntermissionScene.Talk.performed += _ => isTalking = true;
        Controller.IntermissionScene.Talk.canceled += _ => isTalking = false;
    }

    private void OnEnable()
    {
        Controller.Enable();
    }

    private void OnDisable()
    {
        Controller.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Initialize variables
        successfulAttempts = 0; //Counter that tracks, how many times the user has done a successful attempt (Chosen a card)
        ActivateBoosterMenu = false;
        Camera = Camera.main; //Obtain main camera from scene
        if (Camera != null) CinemachineBrain = Camera.gameObject.GetComponent<CinemachineBrain>(); //Deactivate cinemachine
        dialogue1 = "Hello, player would you like to check what I have in store for you? You never know these can be very useful for you";
    }

    // Update is called once per frame
    void Update()
    {
        //Set the visibility of the objects depending on specific factors
        boosterCardMenu.SetActive(ActivateBoosterMenu); 

        teleporter.SetActive(successfulAttempts > 0);

        CinemachineBrain.enabled = !ActivateBoosterMenu;

        if (DialogueManager.inDialogue && isTalking && successfulAttempts == 0)
        {
            ActivateBoosterMenu = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && successfulAttempts == 0)
        {
            DialogueManager.inDialogue = true;
            DialogueManager.currentDialogue = new Dialogue("Cleric1", dialogue1, Dialogue.DialogueType.Text);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DialogueManager.inDialogue = false;
        DialogueManager.currentDialogue = null;
    }

}
