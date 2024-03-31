using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieBoss : MonoBehaviour
{
    //HP stat of the boss
    [SerializeField]
    private float maxHealth;

    public GameObject staff;
    
    public static float currentHealth;
    
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
                currentHealth = maxHealth;
                break;
            }

            case 1:
            {
                maxHealth = 6000;
                currentHealth = maxHealth;
                break;
            }

            case 2:
            {
                maxHealth = 9000;
                currentHealth = maxHealth;
                break;
            }

            default:
            {
                maxHealth = 6000;
                currentHealth = maxHealth;
                break;
            }
        }
    }
}
