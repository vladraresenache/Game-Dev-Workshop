using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private CanvasGroup uiCanvasGroup;
    [SerializeField] private GameObject optionsMenu;  // Reference to the options menu canvas or panel
    [SerializeField] private GameObject mainMenu;      // Reference to the main menu canvas or panel
    [SerializeField] private Button optionsButton;    // Button to open the options menu
    [SerializeField] private Button backButton;       // Button to close the options menu
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float waitTime = 2f; // Time to wait after fade to black

    private void Start()
    {
        if (blackScreen != null)
        {
            Color color = blackScreen.color;
            color.a = 0;
            blackScreen.color = color; // Start with transparent black screen
        }

        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 1; // Start with full visibility
        }

       

        if (optionsButton != null)
        {
            optionsButton.onClick.AddListener(ShowOptionsMenu);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(HideOptionsMenu);
        }

        Debug.Log("MainMenu script initialized.");
    }

    public void PlayGame()
    {
        StartCoroutine(FadeOutAndLoadScene(1));
    }

    public void QuitGame()
    {
        StartCoroutine(FadeOutAndQuit());
    }

    private IEnumerator FadeOutAndLoadScene(int sceneIndex)
    {
        yield return StartCoroutine(FadeToBlackAndHideUI());
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator FadeOutAndQuit()
    {
        yield return StartCoroutine(FadeToBlackAndHideUI());
        yield return new WaitForSeconds(waitTime);
        Application.Quit();
    }

    private IEnumerator FadeToBlackAndHideUI()
    {
        if (blackScreen == null || uiCanvasGroup == null)
        {
            Debug.LogWarning("BlackScreen or UICanvasGroup is missing.");
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeDuration;

            // Fade black screen from 0 to 1
            Color blackColor = blackScreen.color;
            blackColor.a = Mathf.Clamp01(normalizedTime);
            blackScreen.color = blackColor;

            // Fade UI elements from 1 to 0
            uiCanvasGroup.alpha = Mathf.Clamp01(1 - normalizedTime);

            yield return null;
        }

        // Ensure final states
        Color finalBlackColor = blackScreen.color;
        finalBlackColor.a = 1;
        blackScreen.color = finalBlackColor;

        uiCanvasGroup.alpha = 0;
    }

    public void ShowOptionsMenu()
    {
        Debug.Log("ShowOptionsMenu called.");

        if (optionsMenu != null)
        {
            optionsMenu.SetActive(true); // Show the options menu
            Debug.Log("Options menu activated.");
        }
        else
        {
            Debug.LogWarning("OptionsMenu GameObject is not assigned.");
        }

        if (mainMenu != null)
        {
            mainMenu.SetActive(false); // Hide the main menu
            Debug.Log("Main menu deactivated.");
        }
        else
        {
            Debug.LogWarning("MainMenu GameObject is not assigned.");
        }
    }

    public void HideOptionsMenu()
    {
        Debug.Log("HideOptionsMenu called.");

        if (optionsMenu != null)
        {
            optionsMenu.SetActive(false); // Hide the options menu
            Debug.Log("Options menu deactivated.");
        }
        else
        {
            Debug.LogWarning("OptionsMenu GameObject is not assigned.");
        }

        if (mainMenu != null)
        {
            mainMenu.SetActive(true); // Show the main menu
            Debug.Log("Main menu activated.");
        }
        else
        {
            Debug.LogWarning("MainMenu GameObject is not assigned.");
        }
    }
}
