using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance { get; set; } 
    public bool gameStart;

    public TextMeshProUGUI countdownText;
    
    private Spacejet SpaceJetPlayer;
    private SpacejetAI[] SpaceJetAis;

    private List<GameObject> racers;
    public Material spaceJetColor;
    public Material spaceJetGlow;
    private Color[] absorberColors;
    private Color[] boltColors;
    private Color[] ufoColors;
    private Color[] ghostColors;
    private Renderer absorberRenderer;
    private Renderer spacejetRenderer;
    private Image racerIcon;
    private TrailRenderer racerTrail;
    
    public delegate void GameStart();

    public static GameStart GameStarted;

    public GameObject absorberPrefab;
    public GameObject spaceJetBeta;

    private int RaceCount;

    public static List<GameObject> checkpoints;

    private void Awake()
    {
        instance = this;
        //SpaceJetPlayer = GameObject.FindGameObjectWithTag("Player");
        //SpaceJetAis = GameObject.FindObjectsOfType<SpacejetAI>();
        //
        absorberColors = new[]
        {
            Color.red,
            new Color(1, (191/255f), 0, 255), //orange
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            Color.magenta,
            new Color((127/255f), 0, 1), //violet
            Color.white
        };
        
        boltColors = new[]
        {
            Color.yellow,
            new Color(1, (191/255f), 0, 255), //orange
            Color.green,
            Color.cyan,
            new Color((127/255f), 0, 1), //violet
        };
        
        ufoColors = new[]
        {
            Color.red,
            new Color(0.5f, 1, 0.5f),
            new Color(0.1f, 1, 0.1f),
            new Color(0f, 1, 0.7f),
            Color.blue,
            Color.magenta,
        };
        
        ghostColors = new[]
        {
            Color.white,
            Color.gray
        };
        //
        // racers = new List<GameObject> { SpaceJetPlayer.gameObject };
        //
        //
        // foreach (var spaceJetAi in SpaceJetAis)
        // {
        //     racers.Add(spaceJetAi.gameObject);
        // }

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Current Vehicle: " + MenuManager.currentSpaceJet.name);
        gameStart = false;
        
        Cursor.visible = false;
        
        switch (MenuManager.currentSpaceJet.name)
        {
            case "Absorber":
            {
                absorberPrefab.SetActive(true);
                spaceJetBeta.SetActive(false);
                var randomColor = Random.Range(0, absorberColors.Length);
                // absorberRenderer = absorberPrefab.GetComponent<Renderer>();
                spaceJetGlow.SetColor("_EmissionColor", absorberColors[randomColor]);
                break;
            }

            case "Bolt Glider":
            {
                absorberPrefab.SetActive(false);
                spaceJetBeta.SetActive(true);
                var randomColor = Random.Range(0, boltColors.Length);
                // spacejetRenderer = spaceJetBeta.GetComponent<Renderer>();
                spaceJetColor.color = boltColors[randomColor];
                break;
            }
            
            case "UFO":
            {
                absorberPrefab.SetActive(false);
                spaceJetBeta.SetActive(true);
                // spacejetRenderer = spaceJetBeta.GetComponent<Renderer>();
                var randomColor = Random.Range(0, ufoColors.Length);
                spaceJetColor.color = ufoColors[randomColor];
                break;
            }
            
            case "Ghost Rider":
            {
                absorberPrefab.SetActive(false);
                spaceJetBeta.SetActive(true);
                // spacejetRenderer = spaceJetBeta.GetComponent<Renderer>();
                var randomColor = Random.Range(0, ghostColors.Length);
                spaceJetColor.color = ghostColors[randomColor];
                break;
            }
        }
        // var randomColor = 0;
        // var usedNumbers = new List<int>();
        //
        // foreach (var racer in racers)
        // {
        //     while (usedNumbers.Contains(randomColor))
        //     {
        //         randomColor = Random.Range(0, 9);
        //     }
        //     
        //     racerRenderer = racer.GetComponent<Renderer>();
        //     racerRenderer.material.color = materialColors[randomColor];
        //
        //     racerTrail = racer.GetComponent<TrailRenderer>();
        //     racerTrail.startColor = materialColors[randomColor];
        //     racerTrail.endColor = materialColors[randomColor];
        //
        //     racerIcon = racer.GetComponentInChildren<Image>();
        //     racerIcon.color = materialColors[randomColor];
        //
        //     usedNumbers.Add(randomColor);
        // }

        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        
        yield return new WaitForSeconds(2);
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.4f);
        GameStarted?.Invoke();
        countdownText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
