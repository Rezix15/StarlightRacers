using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    private string characterName;
    private string text;
    private DialogueType dialogueVersion;

    public enum DialogueType
    {
        Question,
        Text
    }

    public string GetCharacterName()
    {
        return characterName;
    }
    
    public string GetText()
    {
        return text;
    }
    
    public Dialogue(string dialogueName, string dialogueText, DialogueType dialogueType )
    {
        this.characterName = dialogueName;
        this.text = dialogueText;
        this.dialogueVersion = dialogueType;
    }

    public DialogueType GetDialogueType()
    {
        return dialogueVersion;
    }
}
