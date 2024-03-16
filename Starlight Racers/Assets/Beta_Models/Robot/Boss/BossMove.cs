using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float xTarget;
    public float rate;
    private Boss Boss;

    private Material desperationMat;
    private Material desperationEye;
    private MeshRenderer Renderer;
    
    void Start()
    {
        Boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        Renderer = GetComponent<MeshRenderer>();
        desperationMat = new Material(Renderer.materials[0]);
        desperationEye = new Material(Renderer.materials[3]);
        desperationMat.color = new Color(0, 0, 1);
        desperationEye.color = new Color(0.9f, 0.9f, 1f);
    }

    private void OnEnable()
    {
        StartCoroutine(OpenCoreAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss.ReturnHealthRatio() <= 0.05)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator OpenCoreAnimation()
    {
        
        yield return new WaitForSeconds(0.5f);

        if (xTarget == 0)
        {
            for (float i = transform.position.x; i >= xTarget; i-=(0.03125f * rate))
            {
                transform.position = new Vector3(i, transform.position.y, transform.position.z);
            }
        }
        else
        {
            for (float i = transform.position.x; i <= xTarget; i-=(0.03125f * rate))
            {
                transform.position = new Vector3(i, transform.position.y, transform.position.z);
            }
        }
        
        yield return new WaitForSeconds(0.5f);
        var materials = Renderer.materials;
        materials[0] = desperationMat;
        materials[3] = desperationEye;
        Renderer.materials = materials;
        yield return new WaitForSeconds(0.5f);
        //Renderer.materials[]
    }
}
