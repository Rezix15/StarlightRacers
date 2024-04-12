using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceIntermissionManager : NetworkBehaviour
{
    public static RaceIntermissionManager Instance { get; private set; }
    
    private Dictionary<ulong, bool> readyPlayers;

    [SerializeField] private Transform[] playerObj;

    private int index = 0;
     
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManagerOnOnLoadEventCompleted;
        }
    }

    private void SceneManagerOnOnLoadEventCompleted(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
    {
        foreach (var clientsID in NetworkManager.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerObj[index]);
            
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientsID, true);

            index++;
        }
    }
}
