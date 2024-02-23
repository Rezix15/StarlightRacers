using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject trainObj;
    // Start is called before the first frame update

    private void Awake()
    {
        var trainScale = MenuManager.scaleLevel / 1.5f;
        trainObj.transform.localScale = new Vector3(trainScale, trainScale, trainScale);
    }

    void Start()
    {
        StartCoroutine(SpawnTrain());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnTrain()
    {
        while (true)
        {
            var position = transform.position;
            var trainPos = new Vector3(position.x, MenuManager.scaleLevel * 5, position.z - 5);
            var train = Instantiate(trainObj, trainPos, Quaternion.Euler(0, 90, 0));
            yield return new WaitForSeconds(10);
            Destroy(train);
            yield return new WaitForSeconds(5);
        }
        
    }
}
