using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public Material goState;
    public Material waitState;
    public Material stopState;

    public Material offState;

    private MeshRenderer MeshRenderer;
    

    private Material[] Materials;

    private int stateIndex;
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        Materials = new Material[MeshRenderer.materials.Length];

        Debug.Log("Mat Count: " + Materials.Length);
        stateIndex = 0;

        Materials = MeshRenderer.materials;
        MeshRenderer.materials = Materials;

        StartCoroutine(TransitionState());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator TransitionState()
    {
        while (true)
        {
            ToggleState(stateIndex);
            yield return new WaitForSeconds(10f);
            stateIndex++;
            stateIndex %= MeshRenderer.materials.Length;
        }
    }

    private void ToggleState(int state)
    {
        switch (state)
        {
            //If state is green
            case 0:
            {
                Materials[0] = goState;
                Materials[2] = offState;
                Materials[3] = offState;
                MeshRenderer.materials = Materials;
                break;
            }
            
            case 1:
            {
                Materials[0] = offState;
                Materials[2] = waitState;
                Materials[3] = offState;
                MeshRenderer.materials = Materials;
                break;
            }
            
            case 2:
            {
                Materials[0] = offState;
                Materials[2] = offState;
                Materials[3] = stopState;
                MeshRenderer.materials = Materials;
                break;
            }

            default:
            {
                break;
            }
        }
        
    }
}