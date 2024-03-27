using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieBoss : MonoBehaviour
{
    //HP stat of the boss
    [SerializeField]
    private float maxHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        SetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetHealth()
    {
        switch (MenuManager.difficultyLevel)
        {
            case 0:
            {
                maxHealth = 4000;
                break;
            }

            case 1:
            {
                maxHealth = 6000;
                break;
            }

            case 2:
            {
                maxHealth = 9000;
                break;
            }

            default:
            {
                maxHealth = 6000;
                break;
            }
        }
    }
}
