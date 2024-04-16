using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static int coinCount;
    public TextMeshProUGUI coinText;

    public TextMeshProUGUI coinTextShop;

    public GameObject purchasePrompt;
    public TextMeshProUGUI purchaseText;

    public TextMeshProUGUI[] itemNames;
    public TextMeshProUGUI[] itemTypes;
    public TextMeshProUGUI[] itemDescriptions;
    public Image[] itemIcons;
    public TextMeshProUGUI[] itemCostText;

    public static bool inShop;

    public GameObject[] unavailableImages;

    private int randIndexType; //Random int for the itemType

    private int index;

    private int randIndexVal; //Random int for the itemValue

    private int randIndexID; //Random int for the itemID;

    private List<ItemObj> itemShopList;

    private ItemObj item;

    private int currentIndex;

    private List<int> fullTypes;

    private List<bool>unavailableItems;
    public int itemSeed = 0;

    public Sprite reviveImage;
    public Sprite timerImage;
    public Sprite ammoImage;
    public Sprite abilityImage;
    public Sprite shieldRestoreImage;
    public Sprite halfTimeImage;

    private List<int> invalidEffectIndexes;
    private List<int> invalidAdditiveIndexes;
    private List<int> invalidIndexes;
    private int itemCount;

    public GameObject[] itemSelectedState;

    private int randStart;

    private int randEnd;
    
    private ItemObj currentItem;

    public GameObject confirmButton;

    private GameObject lastItemSelected;

    private bool hasPurchased;

    private string dialogue1;

    private string dialogue2;

    private int currentItemPos;
    
    public GameObject teleporter;
    
    public bool generateRandomSeed = true;
    

    private void Awake()
    {
        GenerateItemSeed();
    }

    // Start is called before the first frame update
    void Start()
    {
        hasPurchased = false;
        lastItemSelected = new GameObject();
        coinText.text = coinCount.ToString();
        randStart = 1;
        randEnd = 4;
        currentIndex = 0;
        itemShopList = new List<ItemObj>();
        invalidAdditiveIndexes = new List<int>();
        invalidEffectIndexes = new List<int>();
        invalidIndexes = new List<int>();
        fullTypes = new List<int>();
        unavailableItems = new List<bool>();


        for (int i = 0; i < 9; i++)
        {
            unavailableItems.Add(false);
        }
        
        Debug.Log("UnavailableItems: " + string.Join(", ", unavailableItems.ToString()));
        
        foreach (var unavailableImage in unavailableImages)
        {
            unavailableImage.SetActive(false);
        }
        
        Debug.Log("UnavailableItemSize: " + unavailableItems.Count);
        
        CreateItems();
        DisplayItems();
        purchasePrompt.SetActive(false);
        hasPurchased = false;
        dialogue1 = "Thanks for the $$$, businezz is back and booming";
        dialogue2 = "What, not going to buy anything? Thanks for wasting my time. Your $$$ must feel disgusted at you poor self.";
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = coinCount.ToString();
        coinTextShop.text = coinCount.ToString();
    }

    private void InitializeIndexState()
    {
        randIndexType = Random.Range(randStart, randEnd);
        
        //Lengthy code to check for duplicates if pool of random types is full of duplicates
        if(fullTypes.Contains(1))
        {
            randStart = 2;
            randIndexType = Random.Range(randStart, randEnd);
        }
            
        if (fullTypes.Contains(3))
        {
            randEnd = 3;
            randIndexType = Random.Range(randStart, randEnd);
        }

        if (fullTypes.Contains(2))
        {
            var randIndexType2 = Random.Range(0, 2);

            if (randIndexType2 == 0)
            {
                randIndexType = randStart;
            }
            else
            {
                randIndexType = randEnd;
            }
        }
    }
    
    
    private void GenerateItemSeed()
    {
        if (generateRandomSeed)
        {
            itemSeed = (int)System.DateTime.Now.Ticks;
        }
        
        Random.InitState(itemSeed);
    }

    //Function to create items for the shop to be displayed
    private void CreateItems()
    {
        for (int i = 0; i < 9; i++)
        {
            InitializeIndexState();
            
            SpawnItemData();
            
            itemShopList.Add(item);
        }
        
        // CoinShopKeeper.ActivateCoinMenu = false;
        
    }

    private void SpawnItemData()
    {
        switch (randIndexType)
        {
            //Time 
            case 1:
            {
                randIndexVal = Random.Range(0, 3);

                while (invalidIndexes.Contains(randIndexVal) && invalidIndexes.Count <= 2)
                {
                    randIndexVal = Random.Range(0, 3);
                }

                //Code to check for duplicate randIndex values
                if (invalidIndexes.Count == 2)
                {
                    fullTypes.Add(randIndexType);
                    InitializeIndexState();
                    SpawnItemData();
                }
                
                switch (randIndexVal)
                {
                    case 0:
                    {
                        item = new ItemObj("Stop watch", timerImage, "Adds an extra 30 seconds to Timer", ItemObj.Type.Time, 250, 30, randIndexVal);
                        break;
                    }

                    case 1:
                    {
                        item = new ItemObj("Laser Watch", timerImage, "Reduces ammo refill time by 2 seconds", ItemObj.Type.Time, 300, 2, randIndexVal);
                        break;
                    }

                    case 2:
                    {
                        item = new ItemObj("Jet O'Clock", timerImage, "Reduces the ability gauge by 50", ItemObj.Type.Time, 600, 50, randIndexVal);
                        break;
                    }
                }
                
                invalidIndexes.Add(randIndexVal);
                break;
            }

            //Additive
            case 2:
            {
                randIndexVal = Random.Range(0, 7);

                while (invalidAdditiveIndexes.Contains(randIndexVal) && invalidAdditiveIndexes.Count <= 6)
                {
                    randIndexVal = Random.Range(0, 7);
                }

                if (invalidAdditiveIndexes.Count == 6)
                {
                    fullTypes.Add(randIndexType);
                    InitializeIndexState();
                    SpawnItemData();
                }

                switch (randIndexVal)
                {
                    case 0:
                    {
                        item = new ItemObj("Laser Bullet+", ammoImage, "Adds +10 to laserDamage", ItemObj.Type.Additive, 200, 10, randIndexVal);
                        break;
                    }

                    case 1:
                    {
                        item = new ItemObj("Energy Component+", ammoImage, "Adds +10 to shield", ItemObj.Type.Additive, 200, 10, randIndexVal);
                        break;
                    }

                    case 2:
                    {
                        item = new ItemObj("Defense Shields+", ammoImage, "Adds +10 to shieldRate", ItemObj.Type.Additive, 200, 10, randIndexVal);
                        break;
                    }

                    case 3:
                    {
                        item = new ItemObj("Turbo Engine+", ammoImage, "Adds +10 to speed", ItemObj.Type.Additive, 50, 10, randIndexVal);
                        break;
                    }

                    case 4:
                    {
                        item = new ItemObj("High-Energy Thrusters+", ammoImage, "Adds +10 to thrust", ItemObj.Type.Additive, 50, 10, randIndexVal);
                        break;
                    }

                    case 5:
                    {
                        item = new ItemObj("Space Locks+", ammoImage, "Adds +10 to grip", ItemObj.Type.Additive, 20, 10, randIndexVal);
                        break;
                    }

                    case 6:
                    {
                        item = new ItemObj("Ammo storage+", ammoImage, "Adds +10 to maximum ammo count", ItemObj.Type.Additive, 200, 10, randIndexVal);
                        break;
                    }
                }
                
                invalidAdditiveIndexes.Add(randIndexVal);
                break;
            }

            //Effect
            case 3:
            {
                randIndexVal = Random.Range(0, 3);

                while (invalidEffectIndexes.Contains(randIndexVal) && invalidEffectIndexes.Count <= 2)
                {
                    randIndexVal = Random.Range(0, 3);
                }

                if (invalidEffectIndexes.Count == 2)
                {
                    //effectFull = true;
                    fullTypes.Add(randIndexType);
                    InitializeIndexState();
                    SpawnItemData();
                }
                
                
                switch (randIndexVal)
                {
                    case 0:
                    {
                        item = new ItemObj("Half-Time", halfTimeImage, "Laser ammo cost is reduced by a half", ItemObj.Type.Effect, 600, 0, randIndexVal);
                        break;
                    }

                    case 1:
                    {
                        item = new ItemObj("Revival Gem", reviveImage, "Grants a one-time revive", ItemObj.Type.Effect, 800, 0, randIndexVal);
                        break;
                    }

                    case 2:
                    {
                        item = new ItemObj("Shield System+", shieldRestoreImage, "Grants the ability to auto repair damaged shield by 5%", ItemObj.Type.Effect, 700, 0, randIndexVal);
                        break;
                    }
                }
                
                invalidEffectIndexes.Add(randIndexVal);
                break;
            }
        }
    }

    public void DisplaySelectedState(int id)
    {
        for (int i = 0; i < itemSelectedState.Length; i++)
        {
            itemSelectedState[i].SetActive((i + 1) == id);
        }
    }

    public void ClickShopItem(int id)
    {
        lastItemSelected = EventSystem.current.currentSelectedGameObject;

        if (!unavailableItems[id + currentIndex])
        {
            currentItem = itemShopList[id + currentIndex];
            
            purchasePrompt.SetActive(true);
        
            purchaseText.text = "Would you like to purchase " + currentItem.ItemName + " for " + currentItem.cost +
                                " coins";
        
            EventSystem.current.SetSelectedGameObject(confirmButton);

            currentItemPos = id + currentIndex;
        }
        
    }


    public void DenyPurchase()
    {
        purchasePrompt.SetActive(false);
        EventSystem.current.SetSelectedGameObject(lastItemSelected);
    }

    public void QuitShop()
    {
        purchasePrompt.SetActive(false);
        CoinShopKeeper.ActivateCoinMenu = false;
        inShop = false;
        Debug.Log("Quit Selected: " + CoinShopKeeper.ActivateCoinMenu);

        if (hasPurchased)
        {
            StartCoroutine(EndingMessage(dialogue1));
        }
        else
        {
            StartCoroutine(EndingMessage(dialogue2));
        }
        
    }

    public void ConfirmPurchase()
    {
        if (coinCount >= currentItem.cost)
        {
            coinCount -= currentItem.cost;
            Debug.Log("Purchase confirmed");
            purchasePrompt.SetActive(false);
            EventSystem.current.SetSelectedGameObject(lastItemSelected);
            hasPurchased = true;
            currentItem.ItemEffect();
            DisplayItems();
            teleporter.SetActive(true);
        }
        else
        {
            Debug.Log("Purchase failed, not enough cash");
        }
    }
    
    //Function to display the items
    private void DisplayItems()
    {
        for (int i = currentIndex; i < (currentIndex + 3); i++)
        {
            itemNames[i % 3].text = itemShopList[i].ItemName;
            itemTypes[i % 3].text = "Item Type: " + itemShopList[i].ItemType;
            itemDescriptions[i % 3].text = itemShopList[i].description;
            itemIcons[i % 3].sprite = itemShopList[i].icon;
            itemCostText[i % 3].text = itemShopList[i].cost.ToString();

            if (itemShopList[i].ItemName == "Shield System+" && GameDataManager.restoreShield)
            {
                unavailableImages[i % 3].SetActive(true);
                unavailableItems[i] = true;
            }
            else
            {
                unavailableImages[i % 3].SetActive(false);
            }
            
            if (itemShopList[i].ItemName == "Half-Time" && GameDataManager.halfTime)
            {
                unavailableImages[i % 3].SetActive(true);
                unavailableItems[i] = true;
            }
            else
            {
                unavailableImages[i % 3].SetActive(false);
            }
            
            itemCostText[i % 3].color = itemShopList[i].cost > coinCount ? Color.red : Color.black;
        }
    }
    
    IEnumerator EndingMessage(string dialogue)
    {
        DialogueManager.currentDialogue = new Dialogue("Cleric2", dialogue, Dialogue.DialogueType.Text);
        yield return new WaitForSeconds(2);
        DialogueManager.inDialogue = false;
    }

    public void ShiftIndex(int position)
    {
        // Debug.Log("currentIndexVal: " + currentIndex);
        
        if (currentIndex >= itemShopList.Count)
        {
            currentIndex = 0;
        }
        
        switch (position)
        {
            case -1:
            {
                if (currentIndex > 0)
                {
                    currentIndex -= 3;
                    DisplayItems();
                }
                
                
                break;
            }
        
            case 1:
            {
                if (currentIndex < itemShopList.Count - 3)
                {
                    currentIndex += 3;
                    DisplayItems();
                }
                
                break;
            }
        
            default:
            {
                break;
            }
        }
    }
}
