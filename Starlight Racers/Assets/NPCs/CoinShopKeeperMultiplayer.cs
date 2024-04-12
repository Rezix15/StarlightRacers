using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinShopKeeperMultiplayer : NetworkBehaviour
{
    public GameObject coinShopMenu;
    private Camera Camera;
    private CinemachineBrain CinemachineBrain;

    private string dialogue1;
    private PlayerController Controller;
    
    public static bool ActivateCoinMenu;
    
    private bool isTalking;

    public GameObject item1;

    private bool isTouching;
    
    
    
    
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
        ActivateCoinMenu = false;
        
        dialogue1 = "Yo, player ya want I've got? Make sure you got that $$$ on ya!";
    }

    // Update is called once per frame
    void Update()
    {
        //Set the visibility of the objects depending on specific factors
        if (IsOwner)
        {
            coinShopMenu.SetActive(ActivateCoinMenu);
        
            if (DialogueManager.inDialogue && isTalking && CoinManager.inShop && isTouching && BoosterShopkeeper.ActivateBoosterMenu == false)
            {
                CinemachineBrain.enabled = !ActivateCoinMenu;
                ActivateCoinMenu = true;
            }
        }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CoinManager.inShop == false && IsOwner)
        {
            isTouching = true;
            DialogueManager.inDialogue = true;
            DialogueManager.currentDialogue = new Dialogue("Cleric2", dialogue1, Dialogue.DialogueType.Text);
            EventSystem.current.SetSelectedGameObject(item1);
            CinemachineBrain = other.GetComponentInChildren<CinemachineBrain>();
            CoinManager.inShop = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsOwner)
        {
            isTouching = false;
            DialogueManager.inDialogue = false;
            DialogueManager.currentDialogue = null;
        }
        
    }
}
