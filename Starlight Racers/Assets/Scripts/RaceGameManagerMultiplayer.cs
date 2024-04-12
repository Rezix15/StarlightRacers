using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

//Code heavily inspired by Code Monkey: https://www.youtube.com/watch?v=7glCsF9fv3s
public class RaceGameManagerMultiplayer : NetworkBehaviour
{
    public NetworkVariable<int> raceCount;
    public int reviveCount;
    public float refillSeconds;
    public int abilityGaugeMax;
    public int timerVal;
    public int laserAmmoMax;
    public bool halfTime;
    public bool restoreShield;
    public bool GameOver;

    public static RaceGameManagerMultiplayer Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        InitializeData();
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManagerConnectionApprove;
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    private void InitializeData()
    {
        raceCount.Value = 1;
        refillSeconds = 10f;
        laserAmmoMax = 20;
        reviveCount = 0;
        timerVal = 0;
        abilityGaugeMax = 120;
        restoreShield = false;
        halfTime = false;
        GameOver = false;
    }

    public SpaceJetObj currentSpaceJet;
    
    
    private void NetworkManagerConnectionApprove(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        connectionApprovalResponse.Approved = true;
        // if (RaceGameManagerMultiplayer.Instance)
        // {
        //     connectionApprovalResponse.Approved = true;
        //     connectionApprovalResponse.CreatePlayerObject = true;
        // }
        // else
        // {
        //     connectionApprovalResponse.Approved = false;
        // }
    }
}

