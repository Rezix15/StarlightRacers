using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class LevelObj : ScriptableObject
{
   public int id;
   public string stageName;
   public string bossName;
   public int highScore;
   public string fastestTime;
}
