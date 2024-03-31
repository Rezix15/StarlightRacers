using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private PlayerController Controller;
    public GameObject pauseMenuObj;

    public GameObject[] pauseOptions;
    public GameObject[] pauseIndicators;

    private bool isActionPressed;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuObj.SetActive(false);
    }

    private void Awake()
    {
        Controller = new PlayerController();
        Controller.PauseMenu.Unpause.performed += _ => UnpauseGame();
        Controller.PauseMenu.Unpause.canceled += _ => UnpauseGame();
    }

    private void OnEnable()
    {
        Controller.Enable();
    }

    private void OnDisable()
    {
        Controller.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        pauseMenuObj.SetActive(CanvasManager.gamePaused);
        ToggleThroughMenu();
    }

    private void ToggleThroughMenu()
    {
        GameObject selectedOpt = EventSystem.current.currentSelectedGameObject;
        
        for (int i = 0; i < pauseOptions.Length; i++)
        {
            pauseIndicators[i].SetActive(pauseOptions[i] == selectedOpt);
        }
    }

    public void UnpauseGame()
    {
        CanvasManager.gamePaused = false;
    }

    public void RestartLevel(int id)
    {
        switch (id)
        {
            case 0:
            {
                SceneManager.LoadScene("StarLightRacers_BetaTest");
                break;
            }

            case 1:
            {
                SceneManager.LoadScene("CandyLand_BetaTest");
                break;
            }

            default:
            {
                SceneManager.LoadScene("CandyLand_BetaTest");
                break;
            }
        }
    }


    public void ExitLevel(int id)
    {
        switch (id)
        {
            case 0:
            {
                SceneManager.LoadScene("IntermissionScene");
                break;
            }

            case 1:
            {
                SceneManager.LoadScene("IntermissionScene(CandyLand)");
                break;
            }

            default:
            {
                SceneManager.LoadScene("IntermissionScene(CandyLand)");
                break;
            }
        }
    }
}
