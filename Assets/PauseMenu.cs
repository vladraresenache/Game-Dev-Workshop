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
        /*
        // Toggle pause menu with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("hehe");
            if (pauseMenuCanvas.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }*/
        /*
        // Toggle index menu with Tab key if the game is not paused
        if (Input.GetKeyDown(KeyCode.Tab) && !pauseMenuCanvas.activeSelf)
        {
            Debug.Log("tabMenu");
            if (indexMenuCanvas.activeSelf)
            {
                CloseIndexMenu();
            }
            else
            {
                OpenIndexMenu();
            }
        }*/
    }

    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f; // Freeze the game

        // Ensure other menus are hidden when the pause menu is opened
        if (indexMenuCanvas != null) indexMenuCanvas.SetActive(false);
        if (settingsMenuCanvas != null) settingsMenuCanvas.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        indexMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f; // Unfreeze the game
    }

    public void OpenIndexMenu()
    {
        pauseMenuCanvas.SetActive(false);
        indexMenuCanvas.SetActive(true);
        
        Debug.Log(pauseMenuCanvas.activeSelf);

        // Ensure settings menu is hidden when index menu is opened
        if (settingsMenuCanvas != null) settingsMenuCanvas.SetActive(false);
    }

    public void CloseIndexMenu()
    {
        indexMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }

    public void OpenSettings()
    {
        pauseMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(true);
        Debug.Log(settingsMenuCanvas.activeSelf);
        // Ensure index menu is hidden when settings menu is opened
        if (indexMenuCanvas != null) indexMenuCanvas.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        ResumeGame(); // Ensure the game is resumed before quitting
        Application.Quit();
        Debug.Log("Game is exiting"); // This won't appear in a built game, but it's useful for testing in the editor
    }
}
