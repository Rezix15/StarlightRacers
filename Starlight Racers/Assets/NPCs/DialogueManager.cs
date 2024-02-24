using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;

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
        
    }
}
