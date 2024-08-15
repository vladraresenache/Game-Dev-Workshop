using UnityEngine;
using UnityEngine.UI;

public class FadeToWhite : MonoBehaviour
{
    public Image whiteImage;  // Reference to the white overlay image
    public float fadeDuration = 1.0f;  // Duration of each fade (to white and back to clear)
    public float waitDuration = 1.0f;  // Duration to wait before fading back to clear

    private float fadeTimer;
    private bool isFadingToWhite;
    private bool isFadingToClear;

    void Start()
    {
        // Set the initial alpha of the white image to 0 (completely transparent)
        Color color = whiteImage.color;
        color.a = 0f;
        whiteImage.color = color;

        // Start the fade to white as soon as the scene begins
  
    }

    void Update()
    {
        // Fade to white
        if (isFadingToWhite)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);

            // Update the image's alpha value
            Color color = whiteImage.color;
            color.a = alpha;
            whiteImage.color = color;

            // If fully white, stop fading to white and start waiting to fade back to clear
            if (alpha >= 1f)
            {
                isFadingToWhite = false;
            }
        }

        // Fade back to clear
        if (isFadingToClear)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(1.0f - (fadeTimer / fadeDuration));

            // Update the image's alpha value
            Color color = whiteImage.color;
            color.a = alpha;
            whiteImage.color = color;

            // Stop fading once fully clear
            if (alpha <= 0f)
            {
                isFadingToClear = false;
            }
        }
    }

    // Method to start the fade to white effect
    public void StartFadeToWhite()
    {
        fadeTimer = 0f;
        isFadingToWhite = true;
        isFadingToClear = false;
    }

    // Method to start the fade back to clear effect
    public void StartFadeToClear()
    {
        fadeTimer = 0f;
        isFadingToClear = true;
    }
}
