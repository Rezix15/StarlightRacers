using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Function to procedurally generate random buildings
public class Road : MonoBehaviour
{
    public GameObject building1;
    public GameObject building2;

    private int randIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(TrackGen.buildingSeed);

        randIndex = Random.Range(0, 3);

        switch (randIndex)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
