using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    public int scaleMax;

    private float originalScale;

    private GameObject laserBeam;

    public GameObject laserBeamPrefab;
    // Start is called before the first frame update
    void Start()
    {
        laserBeam = Instantiate(laserBeamPrefab, transform.position, Quaternion.Euler(90, 0, 0));
        
        StartCoroutine(DestroyObj());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 100; i < scaleMax; i+=100)
        {
            laserBeam.transform.localScale = new Vector3(10f, i, 10f);
            laserBeam.transform.position = new Vector3(transform.position.x, transform.position.y, -i - 50);
        }
    }

    IEnumerator DestroyObj()
    {
        Debug.Log("Time to destroy..");
        yield return new WaitForSeconds(3f);
        Destroy(laserBeam);
        Destroy(gameObject);
        Debug.Log("Destroy all");
    }
}
