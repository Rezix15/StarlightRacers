using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavorBeam : MonoBehaviour
{
    private int count;

    private GameObject player;

    public Material[] flavorMaterials;

    private Material[] currentMaterial;

    private MeshRenderer MeshRenderer;

    [SerializeField]
    private int randIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        currentMaterial = new Material[MeshRenderer.materials.Length];
        currentMaterial = MeshRenderer.materials;
        MeshRenderer.materials = currentMaterial;
        player = GameObject.FindGameObjectWithTag("PlayerRacer");
        StartCoroutine(MoveBeam());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveBeam()
    {
        while (count < 200)
        {
            randIndex = Random.Range(0, flavorMaterials.Length);
            currentMaterial[0] = flavorMaterials[randIndex];
            MeshRenderer.materials = currentMaterial;
            // var position = player.transform.position;
            // var transform1 = transform;
            // transform1.position = new Vector3(position.x, transform1.position.y, position.z);
            yield return new WaitForSeconds(3f);
            count++;
        }
    }
}
