using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    private int scaleMax = 14;

    private int scale = 1;

    public float shieldModifierBonus;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScale();
    }

    void UpdateScale()
    {
        if (scale < scaleMax)
        {
            scale++;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
    
    
}
