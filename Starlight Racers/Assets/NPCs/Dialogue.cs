using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    private string characterName;
    private string text;

    public string GetCharacterName()
    {
        return characterName;
    }
    
    public string GetText()
    {
        return text;
    }
    
    public Dialogue(string dialogueName, string dialogueText)
    {
        this.characterName = dialogueName;
        this.text = dialogueText;
    }
}
