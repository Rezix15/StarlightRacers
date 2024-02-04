using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Inspired by https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/

public class Modifier
{
    public readonly float value;

    public Modifier(float modifierValue)
    {
        value = modifierValue;
    }
}
