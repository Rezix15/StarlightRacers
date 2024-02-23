using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteRim : MonoBehaviour
{
    private int perimeter = 30000;

    public GameObject meteorite;
    
    // Start is called before the first frame update
    void Start()
    {
        var centerPos = new Vector3((perimeter / 2f) - 5000, 0, (perimeter / 2f) + 2000);
        
        //Generate asteroid belt
        for (int i = 0; i < 100; i++)
        {
            var radians = (Mathf.PI * 2) / 100 * i;

            var v = Mathf.Sin(radians);
            var h = Mathf.Cos(radians);

            var asteroidDir = new Vector3(h, 0, v);
            var asteroidPos = centerPos + asteroidDir * 6000;
            Instantiate(meteorite, asteroidPos, Quaternion.identity);
            // var asteroidAng = i * (360 / 5);
            // var asteroidPos = new Vector3(Mathf.Cos(asteroidAng * Mathf.Deg2Rad) + 30000, 35, 30000 + Mathf.Sin((asteroidAng * Mathf.Deg2Rad)));
            // Instantiate(meteorite, asteroidPos, Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
