using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class State 
{
    public virtual IEnumerator Start()
    {
        yield break;
    }
    
    public virtual IEnumerator Shoot()
    {
        yield break;
    }
    
    public virtual IEnumerator Overtake()
    {
        yield break;
    }
}
