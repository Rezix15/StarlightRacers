using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieBossPortal : MonoBehaviour
{
    public GameObject[] cakeCannons;

    private int randIndex;
    // Start is called before the first frame update
    void Start()
    {
        randIndex = Random.Range(0, cakeCannons.Length);
        var position = transform.position;
        //var xFactor = position.x / Mathf.Abs(position.x);
        var spawnPos = new Vector3(position.x , position.y, position.z - 20);
        var cake = Instantiate(cakeCannons[randIndex], spawnPos, Quaternion.Euler(0,270,90));
        StartCoroutine(DestroyObj(cake));
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
