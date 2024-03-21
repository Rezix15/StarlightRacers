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

    public TextMeshProUGUI countdownTextP1;
    public TextMeshProUGUI countdownTextP2;
    
    
    private Spacejet SpaceJetPlayer;
    private SpacejetAI[] SpaceJetAis;

    private List<GameObject> racers;
    public Material spaceJetColor;
    public Material spaceJetGlow;
    public Material spaceJetGlow2;
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

    public GameObject player1AbsorberPrefab;
    public GameObject player1SpaceJetBeta;
    public GameObject player1GhostRiderPrefab;
    
    public GameObject player2AbsorberPrefab;
    public GameObject player2SpaceJetBeta;
    public GameObject player2GhostRiderPrefab;

    private int RaceCount;

    private int playerCount;

    public static List<GameObject> checkpoints;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

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
            new Color(0f, 1, 0.7f),
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
            Color.red,
            new Color(1, (191/255f), 0, 255), //orange
            Color.yellow,
            Color.green,
            new Color(0f, 1, 0.7f),
            Color.cyan,
            Color.blue,
            Color.magenta,
            new Color((127/255f), 0, 1), //violet
            Color.white
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
        playerCount = GameObject.FindObjectsOfType<Spacejet>().Length;
        Debug.Log("Current Vehicle: " + MenuManager.currentSpaceJet.name);
        gameStart = false;
        
        Cursor.visible = false;
        
        switch (MenuManager.currentSpaceJet.name)
        {
            case "Absorber":
            {
                player1AbsorberPrefab.SetActive(true);
                player1SpaceJetBeta.SetActive(false);
                player1GhostRiderPrefab.SetActive(false);
                var randomColor = Random.Range(0, absorberColors.Length);
                // absorberRenderer = absorberPrefab.GetComponent<Renderer>();
                spaceJetGlow.SetColor(EmissionColor, absorberColors[randomColor]);
                break;
            }

            case "Bolt Glider":
            {
                player1AbsorberPrefab.SetActive(false);
                player1SpaceJetBeta.SetActive(true);
                player1GhostRiderPrefab.SetActive(false);
                var randomColor = Random.Range(0, boltColors.Length);
                // spacejetRenderer = spaceJetBeta.GetComponent<Renderer>();
                spaceJetColor.color = boltColors[randomColor];
                break;
            }
            
            case "UFO":
            {
                player1AbsorberPrefab.SetActive(false);
                player1SpaceJetBeta.SetActive(true);
                player1GhostRiderPrefab.SetActive(false);
                // spacejetRenderer = spaceJetBeta.GetComponent<Renderer>();
                var randomColor = Random.Range(0, ufoColors.Length);
                spaceJetColor.color = ufoColors[randomColor];
                break;
            }
            
            case "Ghost Rider":
            {
                player1AbsorberPrefab.SetActive(false);
                player1SpaceJetBeta.SetActive(false);
                player1GhostRiderPrefab.SetActive(true);
                // spacejetRenderer = spaceJetBeta.GetComponent<Renderer>();
                var randomColor = Random.Range(0, ghostColors.Length);
                spaceJetGlow.SetColor(EmissionColor, ghostColors[randomColor]);
                break;
            }
        }
        
        switch (MenuManager.enemySpaceJet.name)
        {
            case "Absorber":
            {
                player2AbsorberPrefab.SetActive(true);
                player2SpaceJetBeta.SetActive(false);
                player2GhostRiderPrefab.SetActive(false);
                var randomColor = Random.Range(0, absorberColors.Length);
                // absorberRenderer = absorberPrefab.GetComponent<Renderer>();
                spaceJetGlow.SetColor(EmissionColor, absorberColors[randomColor]);
                break;
            }

            case "Bolt Glider":
            {
                player2AbsorberPrefab.SetActive(false);
                player2SpaceJetBeta.SetActive(true);
                player2GhostRiderPrefab.SetActive(false);
                var randomColor = Random.Range(0, boltColors.Length);
                // spacejetRenderer = spaceJetBeta.GetComponent<Renderer>();
                spaceJetColor.color = boltColors[randomColor];
                break;
            }
            
            case "UFO":
            {
                player2AbsorberPrefab.SetActive(false);
                player2SpaceJetBeta.SetActive(true);
                player2GhostRiderPrefab.SetActive(false);
                // spacejetRenderer = spaceJetBeta.GetComponent<Renderer>();
                var randomColor = Random.Range(0, ufoColors.Length);
                spaceJetColor.color = ufoColors[randomColor];
                break;
            }
            
            case "Ghost Rider":
            {
                player2AbsorberPrefab.SetActive(false);
                player2SpaceJetBeta.SetActive(false);
                player2GhostRiderPrefab.SetActive(true);
                // spacejetRenderer = spaceJetBeta.GetComponent<Renderer>();
                var randomColor = Random.Range(0, ghostColors.Length);
                //spaceJetColor.color = ghostColors[randomColor];
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

        if (playerCount > 1)
        {
            yield return new WaitForSeconds(2);
            countdownTextP1.text = "3";
            countdownTextP2.text = "3";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "";
            countdownTextP2.text = "";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "2";
            countdownTextP2.text = "2";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "";
            countdownTextP2.text = "";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "1";
            countdownTextP2.text = "1";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "";
            countdownTextP2.text = "";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "GO!";
            countdownTextP2.text = "GO!";
            yield return new WaitForSeconds(0.4f);
            GameStarted?.Invoke();
            countdownTextP1.gameObject.SetActive(false);
            countdownTextP2.gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(2);
            countdownTextP1.text = "3";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "2";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "1";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "";
            yield return new WaitForSeconds(0.5f);
            countdownTextP1.text = "GO!";
            yield return new WaitForSeconds(0.4f);
            GameStarted?.Invoke();
            countdownTextP1.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
