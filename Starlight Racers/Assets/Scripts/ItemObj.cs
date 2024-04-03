using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObj
{
    public enum Type
    {
        Time,
        Additive,
        Effect
    }
    
    public string ItemName; //Name of the item
    public Sprite icon; //The icon that will be displayed
    public string description;
    public Type ItemType; //Type of item
    public int cost; //How much coins the item will cost

    public ItemObj(string itemName, Sprite icon, string description, Type itemType, int cost)
    {
        ItemName = itemName;
        this.icon = icon;
        this.description = description;
        ItemType = itemType;
        this.cost = cost;
    }

    public void ItemEffect(int val, int typeID)
    {
        switch (ItemType)
        {
            case Type.Time:
            {
                //The type of timer to be effected
                switch (typeID)
                {
                    //Type 0: Race timer
                    case 0:
                    {
                        GameDataManager.timerVal += val; //Adds on 'X' amount of time to the timer
                        break;
                    }

                    //Type 1: Ammo Refill Timer
                    case 1:
                    {
                        if (GameDataManager.refillSeconds > 4)
                        {
                            GameDataManager.refillSeconds -= val; //Adds on 'X' amount of time to the timer
                        }
                        
                        break;
                    }

                    case 2:
                    {
                        if (GameDataManager.abilityGaugeMax > 30)
                        {
                            GameDataManager.abilityGaugeMax -= val; //Adds on 'X' amount of time to the timer
                        }
                       
                        break;
                    }
                }
                break;
            }

            case Type.Additive:
            {
                switch (typeID)
                {
                    //Type 0: LaserDamage
                    case 0:
                    {
                        MenuManager.currentSpaceJet.laserDamage += val; //Adds on 'X' amount of time to the timer
                        break;
                    }

                    //Type 1: Shield
                    case 1:
                    {
                        MenuManager.currentSpaceJet.shield += val; //Adds on 'X' amount of time to the timer
                        break;
                    }

                    //Type 2: ShieldRate
                    case 2:
                    {
                        MenuManager.currentSpaceJet.shieldRate += val; //Adds on 'X' amount of time to the timer
                        break;
                    }

                    //Type 3: Speed
                    case 3:
                    {
                        MenuManager.currentSpaceJet.speed += val;
                        break;
                    }
                    
                    //Type 4: Thrust
                    case 4:
                    {
                        MenuManager.currentSpaceJet.thrust += val;
                        break;
                    }
                    
                    //Type 4: Grip
                    case 5:
                    {
                        MenuManager.currentSpaceJet.grip += val;
                        break;
                    }

                    case 6:
                    {
                        GameDataManager.laserAmmoMax += val;
                        break;
                    }
                }
                break;
            }

            case Type.Effect:
            {
                switch (typeID)
                {
                    //Effect to reduce the laser ammo cost by a half
                    case 0:
                    {
                        GameDataManager.halfTime = true;
                        break;
                    }

                    //Effect to revive player by 1
                    case 1:
                    {
                        GameDataManager.reviveCount += val;
                        break;
                    }

                    //Effect to restore user health
                    case 2:
                    {
                        GameDataManager.restoreShield = true;
                        break;
                    }

                    case 3:
                    {
                        break;
                    }
                }
                
                break;
            }
            
            
            
        }
    }

}

