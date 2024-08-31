using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Required for TextMeshProUGUI
using System.Collections;
using UnityEngine.SceneManagement;  // Required for scene management

public class FadeManager : MonoBehaviour
{
    public Image whiteImage;              // Reference to the white overlay image
    public Image blackImage;              // Reference to the black overlay image
    public TextMeshProUGUI textElement1;  // Reference to the first TextMeshProUGUI element
    public TextMeshProUGUI textElement2;  // Reference to the second TextMeshProUGUI element
    public Image additionalImage;         // Reference to the additional image
    public float fadeDuration = 1.0f;     // Duration of each fade (in and out)
    public float waitDuration = 1.0f;     // Duration to wait before fading back to clear

    private float fadeTimer;
    private bool isFadingToWhite;
    private bool isFadingToClear;
    private bool isFadingFromBlack;

    void Start()
    {
        // Set the initial alpha of the images and text elements
        SetAlpha(whiteImage, 0f);
        SetAlpha(blackImage, 0f);
        SetAlpha(textElement1, 0f);
        SetAlpha(textElement2, 0f);
        SetAlpha(additionalImage, 0f);
    }

    void Update()
    {
        // Fade to white
        if (isFadingToWhite)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            SetAlpha(whiteImage, alpha);

            // If fully white, stop fading to white and start waiting to fade back to clear
            if (alpha >= 1f)
            {
                isFadingToWhite = false;
                StartCoroutine(WaitBeforeFadingToClear());
            }
        }

        // Fade back to clear
        if (isFadingToClear)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(1.0f - (fadeTimer / fadeDuration));
            SetAlpha(whiteImage, alpha);

            // Stop fading once fully clear
            if (alpha <= 0f)
            {
                isFadingToClear = false;
            }
        }

        // Fade from black
        if (isFadingFromBlack)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(1.0f - (fadeTimer / fadeDuration));
            SetAlpha(blackImage, alpha);

            // Stop fading once fully clear
            if (alpha <= 0f)
            {
                isFadingFromBlack = false;
            }
        }
    }

    // Method to set the alpha of an Image or TextMeshProUGUI
    private void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic != null)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }

    // Method to fade in and out two TextMeshProUGUI elements
    public void FadeTexts()
    {
        StartCoroutine(FadeTextsCoroutine(textElement1));
    }

    private IEnumerator FadeTextsCoroutine(TextMeshProUGUI text1)
    {
        // Fade in both texts
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            SetAlpha(text1, Mathf.Lerp(0f, 1f, t / fadeDuration));
            yield return null;
        }

        // Fade out both texts
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            SetAlpha(text1, Mathf.Lerp(1f, 0f, t / fadeDuration));
            yield return null;
        }
    }

    // Method to fade in and out an image and a TextMeshProUGUI element together
    public void FadeImageAndText()
    {
        StartCoroutine(FadeImageAndTextCoroutine(additionalImage, textElement2));
    }

    private IEnumerator FadeImageAndTextCoroutine(Image image, TextMeshProUGUI text)
    {
        // Fade in both the image and the text
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            SetAlpha(image, Mathf.Lerp(0f, 1f, t / fadeDuration));
            SetAlpha(text, Mathf.Lerp(0f, 1f, t / fadeDuration));
            yield return null;
        }

        // Fade out both the image and the text
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            SetAlpha(image, Mathf.Lerp(1f, 0f, t / fadeDuration));
            SetAlpha(text, Mathf.Lerp(1f, 0f, t / fadeDuration));
            yield return null;
        }
    }

    // Method to start the fade to white effect
    public void StartFadeToWhite()
    {
        fadeTimer = 0f;
        isFadingToWhite = true;
        isFadingToClear = false;
        isFadingFromBlack = false;
    }

    // Method to start the fade back to clear effect
    public void StartFadeToClear()
    {
        fadeTimer = 0f;
        isFadingToClear = true;
    }

    // Method to start the fade from black effect
    public void StartFadeFromBlack()
    {
        fadeTimer = 0f;
        isFadingFromBlack = true;
        isFadingToWhite = false;
        isFadingToClear = false;
    }

    // Coroutine to wait before fading back to clear
    IEnumerator WaitBeforeFadingToClear()
    {
        yield return new WaitForSeconds(waitDuration);
        StartFadeToClear();
    }

    // Method to load the first scene
    public void ReturnToFirstScene()
    {
        StartCoroutine(TransitionToFirstSceneCoroutine());
    }

    private IEnumerator TransitionToFirstSceneCoroutine()
    {
        // Fade to white
        StartFadeToWhite();

        // Wait for the fade to complete
        yield return new WaitUntil(() => !isFadingToWhite && !isFadingToClear);

        // Load the first scene
        SceneManager.LoadScene(0); // Assuming the first scene has index 0

        // Optionally, fade in from black after loading the new scene
        // StartFadeFromBlack();
    }
}
