using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortal : MonoBehaviour
{
    public GameObject[] ghostCannons;

    private int randIndex;
    // Start is called before the first frame update
    void Start()
    {
        randIndex = Random.Range(0, ghostCannons.Length);
        var position = transform.position;
        var spawnPos = new Vector3(position.x - 20, position.y, position.z - 20);
        var ghost = Instantiate(ghostCannons[randIndex], spawnPos, Quaternion.identity);
        StartCoroutine(DestroyObj(ghost));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyObj(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        Destroy(obj);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
