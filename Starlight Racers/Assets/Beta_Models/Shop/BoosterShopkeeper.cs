using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BoosterShopkeeper : MonoBehaviour
{
    public static bool ActivateBoosterMenu;

    public static int successfulAttempts = 0;

    public GameObject boosterCardMenu;

    private Camera Camera;
    private CinemachineBrain CinemachineBrain;
    
    // Start is called before the first frame update
    void Start()
    {
        ActivateBoosterMenu = false;
        Camera = Camera.main;
        if (Camera != null) CinemachineBrain = Camera.gameObject.GetComponent<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        boosterCardMenu.SetActive(ActivateBoosterMenu);
        CinemachineBrain.enabled = !ActivateBoosterMenu;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && successfulAttempts == 0)
        {
            ActivateBoosterMenu = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
    }
}
