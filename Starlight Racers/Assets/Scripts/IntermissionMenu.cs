using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntermissionMenu : MonoBehaviour
{
    public static ComponentObj currentComponent;

    public ComponentObj[] components;
    
    // Start is called before the first frame update
    void Start()
    {
        
        currentComponent = components[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueSelected()
    {
        SceneManager.LoadScene("StarLightRacers_BetaTest");
    }
}
