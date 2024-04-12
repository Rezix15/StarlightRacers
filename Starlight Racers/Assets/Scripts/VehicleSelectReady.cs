using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


//Code heavily borrowed from Code Monkey: https://www.youtube.com/watch?v=7glCsF9fv3s
public class VehicleSelectReady : NetworkBehaviour
{
    public static VehicleSelectReady Instance { get; private set; }
    private Dictionary<ulong, bool> readyPlayers;
    [SerializeField] private GameObject readyIcon;
    
    private void Awake()
    {
        Instance = this;
        readyPlayers = new FlexibleDictionary<ulong, bool>();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        readyPlayers[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        
        foreach (var clientsId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!readyPlayers.ContainsKey(clientsId) || !readyPlayers[clientsId])
            {
                //readyIcon.SetActive(true);
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("IntermissionSceneMultiplayer", LoadSceneMode.Single);

            // switch (MenuManager.currentStageId)
            // {
            //     case 0:
            //     {
            //         
            //         break;
            //     }
            //
            //     case 1:
            //     {
            //         break;
            //     }
            //
            //     default:
            //     {
            //         NetworkManager.Singleton.SceneManager.LoadScene("StarLightRacers_BetaTest(Multiplayer)", LoadSceneMode.Single);
            //         break;
            //     }
            // }
            //availableSpaceJets.Remove(currentSpaceJet); //Remove current spaceJet

            // switch (MenuManager.currentStageId)
            // {
            //     case 0:
            //     {
            //         NetworkManager.Singleton.SceneManager.LoadScene("IntermissionScene", LoadSceneMode.Single);
            //         break;
            //     }
            //
            //     case 1:
            //     {
            //         NetworkManager.Singleton.SceneManager.LoadScene("IntermissionScene(CandyLand)", LoadSceneMode.Single);
            //         break;
            //     }
            // }
        }
    }
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
