using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public GameObject indexMenuCanvas;
    public GameObject settingsMenuCanvas;

    void Start()
    {
        // Ensure all menus are hidden initially
        pauseMenuCanvas.SetActive(false);
        indexMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(false);
    }

    void Update()
    {
        // Toggle pause menu with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuCanvas.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        indexMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void OpenIndexMenu()
    {
        // Ensure only the index menu is active
        pauseMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(false);
        indexMenuCanvas.SetActive(true);
    }

    public void CloseIndexMenu()
    {
        // Return to the pause menu from the index menu
        indexMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }

    public void OpenSettings()
    {
        // Ensure only the settings menu is active
        pauseMenuCanvas.SetActive(false);
        indexMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        // Return to the pause menu from the settings menu
        settingsMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        // Resume the game before quitting to ensure a proper exit
        ResumeGame();
        Application.Quit();
        Debug.Log("Game is exiting"); // This won't appear in a built game, but it's useful for testing in the editor
    }
}
