using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadLeaderboard : MonoBehaviour
{
    private PlayerBoss player;
    public TextMeshProUGUI countdownText;

    [SerializeField]
    private bool hasFinished;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRacer").GetComponent<PlayerBoss>();
        countdownText.text = "";
        //hasFinished = player.hasFinished;
    }

    private void OnEnable()
    {
        StartCoroutine(WinningMessage());
    }

    // Update is called once per frame
    void Update()
    {
        hasFinished = player.hasFinished;
    }
    
    IEnumerator WinningMessage()
    {
        print("Finishing..");
        yield return new WaitForSeconds(2f);
        countdownText.text = "YOU WIN!";
        yield return new WaitForSeconds(5f);
        countdownText.text = "";
    }
}
