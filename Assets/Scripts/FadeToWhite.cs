using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image whiteImage;     // Reference to the white overlay image
    public Image blackImage;     // Reference to the black overlay image
    public float fadeDuration = 1.0f;  // Duration of each fade (to white and back to clear)
    public float waitDuration = 1.0f;  // Duration to wait before fading back to clear

    private float fadeTimer;
    private bool isFadingToWhite;
    private bool isFadingToClear;
    private bool isWaiting;
    private bool isFadingFromBlack;

    void Start()
    {
        // Set the initial alpha of the white and black images
        Color whiteColor = whiteImage.color;
        whiteColor.a = 0f;
        whiteImage.color = whiteColor;

        Color blackColor = blackImage.color;
        blackColor.a = 0f;
        blackImage.color = blackColor;

        // Optionally, start any fade sequences here
        // StartCoroutine(FadeSequence());  // Example call
    }

    void Update()
    {
        // Fade to white
        if (isFadingToWhite)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);

            Color color = whiteImage.color;
            color.a = alpha;
            whiteImage.color = color;

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

            Color color = whiteImage.color;
            color.a = alpha;
            whiteImage.color = color;

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

            Color color = blackImage.color;
            color.a = alpha;
            blackImage.color = color;

            // Stop fading once fully clear
            if (alpha <= 0f)
            {
                isFadingFromBlack = false;
            }
        }
    }

    // Coroutine for the full fade sequence: to white, wait, then to clear
    IEnumerator FadeSequence()
    {
        StartFadeToWhite();
        yield return new WaitUntil(() => !isFadingToWhite);

        yield return new WaitForSeconds(waitDuration);

        StartFadeToClear();
        yield return new WaitUntil(() => !isFadingToClear);
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
}
