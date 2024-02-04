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
    private readonly List<Modifier> modifiers;

    public float trueValue => CalculateFinalValue();

    public Stat(float value)
    {
        baseValue = value;
        modifiers = new List<Modifier>();
    }

    public float GetValue()
    {
        return baseValue;
    }

    public void SetValue(float value)
    {
        baseValue = value;
    }

    private float CalculateFinalValue()
    {
        float finalValue = baseValue;

        for (int i = 0; i < modifiers.Count; i++)
        {
            finalValue *= 1 + modifiers[i].value;
        }

        return (float)Math.Round(finalValue, 4);
    }

    public void AddModifier(Modifier mod)
    {
        modifiers.Add(mod);
    }

    public void RemoveModifier(Modifier mod)
    {
        modifiers.Remove(mod);
    }

}
