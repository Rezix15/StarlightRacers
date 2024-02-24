using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;

    public GameObject[] options;
    
    public static bool inDialogue;

    public static Dialogue currentDialogue;
    // Start is called before the first frame update
    void Start()
    {
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
        
        //SceneManager.LoadScene("StarLightRacers_BetaTest");

        switch (dialogueType)
        {
            case Dialogue.DialogueType.Question:
            {
                foreach (var option in options)
                {
                    option.SetActive(true);
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
        SceneManager.LoadScene("StarLightRacers_BetaTest");
        inDialogue = false;
    }

    public void DenyTeleport()
    {
        inDialogue = false;
        currentDialogue = null;
    }
    
    
}
