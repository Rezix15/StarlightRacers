using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;

    public GameObject[] options;
    
    public static bool inDialogue;

    public static Dialogue currentDialogue;

    private bool firstEntry;

    public static int id;
    
    //private PlayerController Controller;
    
    // Start is called before the first frame update
    void Start()
    {
        firstEntry = true;
        inDialogue = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        dialogueUI.SetActive(inDialogue);
        
        if (currentDialogue != null)
        {
            SetDialogue();
        }
    }

    private void SetDialogue()
    {
        characterNameText.text = currentDialogue.GetCharacterName();
        dialogueText.text = currentDialogue.GetText();
        var dialogueType = currentDialogue.GetDialogueType();

        switch (dialogueType)
        {
            case Dialogue.DialogueType.Question:
            {
                foreach (var option in options)
                {
                    option.SetActive(true);
                }

                if (firstEntry)
                {
                    EventSystem.current.SetSelectedGameObject(options[1]);
                    firstEntry = false;
                }
                
                break;
            }

            case Dialogue.DialogueType.Text:
            {
                foreach (var option in options)
                {
                    option.SetActive(false);
                }
                break;
            }
        }
    }
    
    //Functions that are meant for the teleporter
    public void AcceptTeleport()
    {
        firstEntry = true;
        inDialogue = false;
        currentDialogue = null;
        

        switch (id)
        {
            case 0:
            {
                switch (MenuManager.currentStageId)
                {
                    case 0:
                    {
                        SceneManager.LoadScene("StarLightRacers_BetaTest");
                        break;
                    }

                    case 1:
                    {
                        SceneManager.LoadScene("CandyLand_BetaTest");
                        break;
                    }
                }
                break;
            }

            case 1:
            {
                switch (MenuManager.currentStageId)
                {
                    case 0:
                    {
                        SceneManager.LoadScene("BossScene");
                        break;
                    }

                    case 1:
                    {
                        SceneManager.LoadScene("BossScene(CandyLand)");
                        break;
                    }
                }

               
                break;
            }
        }
        //SceneManager.LoadScene("DesertRoad_BetaTest");
        //SceneManager.LoadScene("StarLightRacers_BetaTest(Multiplayer)");
    }

    public void DenyTeleport()
    {
        inDialogue = false;
        currentDialogue = null;
        firstEntry = true;
    }
    
    
}
