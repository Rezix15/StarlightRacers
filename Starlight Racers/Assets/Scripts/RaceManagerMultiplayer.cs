using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RaceManagerMultiplayer : MonoBehaviour
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

    public static List<int> spawnPositions;
    private void Awake()
    {
        //instance = this;
        //SpaceJetPlayer = GameObject.FindGameObjectWithTag("Player");
        //SpaceJetAis = GameObject.FindObjectsOfType<SpacejetAI>();
        //

        spawnPositions = new List<int>();
        
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
}
