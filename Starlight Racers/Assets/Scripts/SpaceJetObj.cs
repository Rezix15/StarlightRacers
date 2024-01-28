using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Space Jet", menuName = "SpaceJet")]
public class SpaceJetObj : ScriptableObject
{
    public new string name;
    public int speed;
    public int shield;
    public int shieldRate;
    public int grip;
    public int thrust;
    public int laserDamage;
    public int referenceIndex; //The model to reference when showing on the menu
}
