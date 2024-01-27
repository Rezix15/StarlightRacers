using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    private Color[] materialColors;
    private Renderer racerRenderer;
    private Image racerIcon;
    private TrailRenderer racerTrail;
    
    public delegate void GameStart();

    public static GameStart GameStarted;

    private void Awake()
    {
        instance = this;
        SpaceJetPlayer = GameObject.FindObjectOfType<Spacejet>();
        SpaceJetAis = GameObject.FindObjectsOfType<SpacejetAI>();
        
        materialColors = new[]
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

        racers = new List<GameObject> { SpaceJetPlayer.gameObject };


        foreach (var spaceJetAi in SpaceJetAis)
        {
            racers.Add(spaceJetAi.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
        gameStart = false;
        
        var randomColor = 0;
        var usedNumbers = new List<int>();
        
        foreach (var racer in racers)
        {
            while (usedNumbers.Contains(randomColor))
            {
                randomColor = Random.Range(0, 9);
            }
            
            racerRenderer = racer.GetComponent<Renderer>();
            racerRenderer.material.color = materialColors[randomColor];

            racerTrail = racer.GetComponent<TrailRenderer>();
            racerTrail.startColor = materialColors[randomColor];
            racerTrail.endColor = materialColors[randomColor];

            racerIcon = racer.GetComponentInChildren<Image>();
            racerIcon.color = materialColors[randomColor];

            usedNumbers.Add(randomColor);
        }

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
