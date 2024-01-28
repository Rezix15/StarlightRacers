using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Material spaceJetColor;

    private Color[] materialColors;

    public List<Transform> menus;

    private int menu;

    private bool isActionPressed;

    private PlayerController Controller;

    private bool hasBeenPressed = false;

    public TextMeshProUGUI descriptiveText1;

    public static SpaceJetObj currentSpaceJet;

    public SpaceJetObj[] spaceJets;

    public TextMeshProUGUI spaceJetNameText;

    public GameObject speedStat;
    public GameObject shieldStat;
    public GameObject shieldRateStat;
    public GameObject gripStat;
    public GameObject thrustStat;
    public GameObject laserStat;

    private Image[] speedStatImg;
    private Image[] shieldStatImg;
    private Image[] shieldRateStatImg;
    private Image[] gripStatImg;
    private Image[] thrustStatImg;
    private Image[] laserStatImg;

    //Indicator to show what slide is current on
    private int currentId = 0;

    public GameObject lButton;
    public GameObject rButton;

    public GameObject prefabSpaceJetHolder;

    private GameObject[] prefabSpaceJets;

    private void Awake()
    {
        Controller = new PlayerController();

        Debug.Log("Controller: " + Controller);

        Controller.Player.Accelerate.performed += _ => isActionPressed = true;
        Controller.Player.Accelerate.canceled += _ => isActionPressed = false;
    }

    private void OnEnable()
    {
        Controller.Enable();
    }

    private void OnDisable()
    {
        Controller.Disable();
    }
    // Start is called before the first frame update



    // Start is called before the first frame update
    void Start()
    {
        ToggleMenu(0);

        // speedStatImg = new Image[10];
        // shieldStatImg = new Image[10];
        // shieldRateStatImg = new Image[10];
        // gripStatImg = new Image[10];
        // thrustStatImg = new Image[10];
        // laserStatImg = new Image[10];

        speedStatImg = speedStat.GetComponentsInChildren<Image>();
        shieldStatImg = shieldStat.GetComponentsInChildren<Image>();
        shieldRateStatImg = shieldRateStat.GetComponentsInChildren<Image>();
        gripStatImg = gripStat.GetComponentsInChildren<Image>();
        thrustStatImg = thrustStat.GetComponentsInChildren<Image>();
        laserStatImg = laserStat.GetComponentsInChildren<Image>();

        currentSpaceJet = spaceJets[0];

        var childCount = prefabSpaceJetHolder.transform.childCount;

        prefabSpaceJets = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            prefabSpaceJets[i] = prefabSpaceJetHolder.transform.GetChild(i).gameObject;

            if (i != currentId)
            {
                prefabSpaceJets[i].SetActive(false);
            }
            else
            {
                prefabSpaceJets[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        WaitForKeyPress();
    }

    private void WaitForKeyPress()
    {
        if (isActionPressed && !hasBeenPressed || Input.anyKey && !hasBeenPressed)
        {
            ToggleMenu(1);
            hasBeenPressed = true;
        }
    }

    public void HoverButton(int id)
    {
        switch (id)
        {
            case 1:
            {
                descriptiveText1.text = "Race against opponents in a single-player Grand Prix.";
                break;
            }

            case 2:
            {
                descriptiveText1.text = "Purchase upgrades for your space-jets.";
                break;
            }

            case 3:
            {
                descriptiveText1.text = "Need certain fixes for your experience?";
                break;
            }

            case 4:
            {
                descriptiveText1.text = "Need to quit?";
                break;
            }

            default:
            {
                descriptiveText1.text = "";
                break;
            }
        }
    }

    public void HoverRotate()
    {
        Debug.Log("You are hovering over object");
    }

    public void HoverRotateExit()
    {
        Debug.Log("You are not hovering over object");
    }

    public void OnSelectVehicle()
    {
        //ToggleMenu(3);
        SceneManager.LoadScene("StarLightRacers_BetaTest");
    }

    public void ToggleLeft()
    {
        if (currentId > 0)
        {
            currentId--;
        }

        if (currentId < spaceJets.Length - 1)
        {
            rButton.SetActive(true);
        }

        if (currentId <= 0)
        {
            lButton.SetActive(false);
            rButton.SetActive(true);
        }

        DisplayStats();
    }

    public void ToggleRight()
    {
        if (currentId < spaceJets.Length)
        {
            currentId++;
        }

        if (currentId > 0)
        {
            lButton.SetActive(true);
        }

        if (currentId == spaceJets.Length - 1)
        {
            rButton.SetActive(false);
        }

        DisplayStats();
    }

    private void DisplayStats()
    {
        currentSpaceJet = spaceJets[currentId];

        for (int i = 0; i < prefabSpaceJets.Length; i++)
        {
            if (i != currentSpaceJet.referenceIndex)
            {
                prefabSpaceJets[i].SetActive(false);
            }
            else
            {
                prefabSpaceJets[i].SetActive(true);
            }
        }

        spaceJetNameText.text = currentSpaceJet.name;


        //Calculate the speedStat UI Display
        var speedVal = (currentSpaceJet.speed / 30000f) * speedStatImg.Length;

        for (int i = 0; i < speedStatImg.Length; i++)
        {
            speedStatImg[i].color = i < (int)speedVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }

        var shieldVal = (currentSpaceJet.shield / 500f) * shieldStatImg.Length;

        for (int i = 0; i < shieldStatImg.Length; i++)
        {
            shieldStatImg[i].color = i < (int)shieldVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }

        var shieldRateVal = (currentSpaceJet.shieldRate / 50f) * shieldRateStatImg.Length;

        for (int i = 0; i < shieldRateStatImg.Length; i++)
        {
            shieldRateStatImg[i].color =
                i < (int)shieldRateVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }

        var gripVal = (currentSpaceJet.grip / 10f) * gripStatImg.Length;

        for (int i = 0; i < gripStatImg.Length; i++)
        {
            gripStatImg[i].color = i < (int)gripVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }

        var thrustVal = (currentSpaceJet.thrust / 300f) * thrustStatImg.Length;

        for (int i = 0; i < thrustStatImg.Length; i++)
        {
            thrustStatImg[i].color = i < (int)thrustVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }

        var laserVal = (currentSpaceJet.laserDamage / 50f) * laserStatImg.Length;

        for (int i = 0; i < laserStatImg.Length; i++)
        {
            laserStatImg[i].color = i < (int)laserVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }
    }

    void ToggleMenu(int position)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].gameObject.SetActive(i == position);
        }
    }

    public void OnPlayClicked()
    {
        //SceneManager.LoadScene("StarLightRacers_BetaTest");
        ToggleMenu(2);
        rButton.SetActive(true);
        DisplayStats();
    }
}
