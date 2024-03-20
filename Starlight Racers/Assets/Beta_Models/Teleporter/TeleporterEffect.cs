using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterEffect : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var upDir = speed * Time.deltaTime;
        transform.position += new Vector3(0, upDir, 0);
    }
}
