using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lollipop : MonoBehaviour
{
    public Material appleFlavor;
    public Material orangeFlavor;
    public Material lemonFlavor;
    public Material grapeFlavor;
    public Material strawberryFlavor;
    public Material cherryFlavor;
    
    public Material appleFlavorShade;
    public Material orangeFlavorShade;
    public Material lemonFlavorShade;
    public Material grapeFlavorShade;
    public Material strawberryFlavorShade;
    public Material cherryFlavorShade;

    private int randFlavor;

    private MeshRenderer MeshRenderer;

    private Material[] currentMaterials;
    
    // Start is called before the first frame update
    void Start()
    {
        randFlavor = Random.Range(0, 6);
        MeshRenderer = GetComponent<MeshRenderer>();
        currentMaterials = new Material[MeshRenderer.materials.Length];

        currentMaterials = MeshRenderer.materials;
        MeshRenderer.materials = currentMaterials;

        appleFlavor = currentMaterials[0];
        appleFlavorShade = currentMaterials[1];

        switch (randFlavor)
        {
            case 0:
            {
                currentMaterials[0] = cherryFlavor;
                currentMaterials[1] = cherryFlavorShade;
                MeshRenderer.materials = currentMaterials;
                break;
            }
            
            case 1:
            {
                currentMaterials[0] = orangeFlavor;
                currentMaterials[1] = orangeFlavorShade;
                MeshRenderer.materials = currentMaterials;
                break;
            }
            
            case 2:
            {
                currentMaterials[0] = lemonFlavor;
                currentMaterials[1] = lemonFlavorShade;
                MeshRenderer.materials = currentMaterials;
                break;
            }
            
            case 3:
            {
                currentMaterials[0] = grapeFlavor;
                currentMaterials[1] = grapeFlavorShade;
                MeshRenderer.materials = currentMaterials;
                break;
            }
            
            case 4:
            {
                currentMaterials[0] = strawberryFlavor;
                currentMaterials[1] = strawberryFlavorShade;
                MeshRenderer.materials = currentMaterials;
                break;
            }

            default:
            {
                currentMaterials[0] = appleFlavor;
                currentMaterials[1] = appleFlavorShade;
                MeshRenderer.materials = currentMaterials;
                break;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
