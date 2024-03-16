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
        StartCoroutine(SpawnLaser(spawnPos));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnLaser(Vector3 spawnPos)
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(laserPrefab, spawnPos, Quaternion.identity, transform);
    }
}
