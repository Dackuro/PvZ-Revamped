using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenuController : MonoBehaviour
{
    public bool gameOver;
    public bool pause;
    public bool onMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private EndGame endGame;

    void Start()
    {
        gameOver = false;
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        gameOver = endGame.gameOver;
        PauseMenu();
        if (gameOver)
        {
            onMenu = true;
        }
    }

    public void PauseMenu()
    {
        if((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !onMenu)
        {
            if (pause == false)
            {
                pause = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pause = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }       
    }
    public void OnMenuBool(bool confirmation)
    {
        onMenu = confirmation;
    }
    public void ContinueButton()
    {
        pause = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void VolverMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
