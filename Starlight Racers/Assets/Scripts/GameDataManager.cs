using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static int RaceCount;
    public static int reviveCount;
    public static float refillSeconds;
    public static int abilityGaugeMax;
    public static int timerVal;
    public static int laserAmmoMax;
    public static bool halfTime;
    public static bool restoreShield;
    public static bool GameOver;
    
    // Start is called before the first frame update
    void Start()
    {
        RaceCount = 1;
        refillSeconds = 10f;
        laserAmmoMax = 20;
        reviveCount = 0;
        timerVal = 0;
        abilityGaugeMax = 120;
        restoreShield = false;
        halfTime = false;
        GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
