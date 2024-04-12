using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinShopKeeper : MonoBehaviour
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
        Camera = Camera.main; //Obtain main camera from scene
        if (Camera != null) CinemachineBrain = Camera.gameObject.GetComponent<CinemachineBrain>(); //Deactivate cinemachine
        dialogue1 = "Yo, player ya want I've got? Make sure you got that $$$ on ya!";
    }

    // Update is called once per frame
    void Update()
    {
        //Set the visibility of the objects depending on specific factors
        coinShopMenu.SetActive(ActivateCoinMenu);
        CinemachineBrain.enabled = !ActivateCoinMenu;
        
        if (DialogueManager.inDialogue && isTalking && CoinManager.inShop && isTouching && BoosterShopkeeper.ActivateBoosterMenu == false)
        {
            ActivateCoinMenu = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CoinManager.inShop == false)
        {
            isTouching = true;
            DialogueManager.inDialogue = true;
            DialogueManager.currentDialogue = new Dialogue("Cleric2", dialogue1, Dialogue.DialogueType.Text);
            EventSystem.current.SetSelectedGameObject(item1);
            CoinManager.inShop = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isTouching = false;
        DialogueManager.inDialogue = false;
        DialogueManager.currentDialogue = null;
    }
}
