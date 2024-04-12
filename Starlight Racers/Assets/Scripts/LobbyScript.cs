using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LobbyScript : MonoBehaviour
{
    [SerializeField] private Button createLobbyBtn;
    [SerializeField] private Button joinGameBtn;


    public void CreateGame()
    {
       RaceGameManagerMultiplayer.Instance.StartHost();
       NetworkManager.Singleton.SceneManager.LoadScene("VehicleSelectMultiplayer", LoadSceneMode.Single);
    }

    public void JoinGame()
    {
        RaceGameManagerMultiplayer.Instance.StartClient();
        
    }
}
