using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Inspired by https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/

[System.Serializable]
public class Stat
{
    public float baseValue;
    private readonly List<float> modifiers;


    public Stat(float value)
    {
        this.baseValue = value;
        modifiers = new List<float>();
    }

    public float GetValue()
    {
        return baseValue;
    }

    public void SetValue(float value)
    {
        baseValue = value;
    }

    public void AddModifier(float modifierVal)
    {
        modifiers.Add(modifierVal);
    }

    public void RemoveModifier(float modifierVal)
    {
        modifiers.Remove(modifierVal);
    }

}
