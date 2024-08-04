using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private CanvasGroup uiCanvasGroup;
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
            yield break;

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
}
