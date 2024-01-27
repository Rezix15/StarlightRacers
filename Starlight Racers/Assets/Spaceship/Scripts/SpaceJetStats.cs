using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//Inspired by https://www.youtube.com/watch?v=e8GmfoaOB4Y -> Brackeys RPG Stat tutorial
[System.Serializable]
public class SpaceJetStats : MonoBehaviour
{
      //public Stat Thrust;
      //public Stat Grip;
      public Stat baseShieldMax; //Health Stat
      public Stat baseShieldRate; //Defense Stat
      public Stat baseLaserDamage; 
      public Stat baseSpeed; //Speed Stat
}
