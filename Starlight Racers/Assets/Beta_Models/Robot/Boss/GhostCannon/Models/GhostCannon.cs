using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCannon : MonoBehaviour
{
    public GameObject laserPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        var position = transform.position;
        var spawnPos = new Vector3(position.x, position.y, position.z - 20);
        Instantiate(laserPrefab, spawnPos, Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
