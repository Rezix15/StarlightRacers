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
        MeshRenderer = gameObject.GetComponent<MeshRenderer>();
        Materials = new Material[MeshRenderer.materials.Length];

        stateIndex = 0;

        for (int i = 0; i < Materials.Length; i++)
        {
            MeshRenderer.material = MeshRenderer.materials[2];
        }

        Materials[0] = offState;
        Materials[1] = stopState;
        Materials[2] = waitState;
        Materials[3] = goState;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(TransitionState());
        stateIndex++;
    }

    IEnumerator TransitionState()
    {
        ToggleState(stateIndex);
        yield return new WaitForSeconds(20f);
    }

    private void ToggleState(int state)
    {
        switch (state)
        {
            //If state is green
            case 0:
            {
                MeshRenderer.materials[0] = goState;
                MeshRenderer.materials[2] = offState;
                MeshRenderer.materials[3] = offState;
                break;
            }
            
            case 1:
            {
                MeshRenderer.materials[0] = offState;
                MeshRenderer.materials[2] = waitState;
                MeshRenderer.materials[3] = offState;
                break;
            }
            
            case 2:
            {
                MeshRenderer.materials[0] = offState;
                MeshRenderer.materials[2] = offState;
                MeshRenderer.materials[3] = stopState;
                break;
            }

            default:
            {
                break;
            }
        }
        
    }
}
