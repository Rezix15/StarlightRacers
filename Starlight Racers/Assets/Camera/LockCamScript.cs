using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockCamScript : MonoBehaviour
{
    public CinemachineFreeLook camFreeLook;

    public Transform lookAtObj;
    
    // Start is called before the first frame update
    void Start()
    {
        //camFreeLook = GetComponent<CinemachineFreeLook>();
        camFreeLook.ForceCameraPosition(lookAtObj.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
