using UnityEngine;
using UnityEngine.UI;  // For UI Image
using System.Collections;

public class FadeFromBlack : MonoBehaviour
{
    public Image fadeImage;           // UI Image used for fading
    public float fadeDuration = 2f;   // Duration of the fade effect

    void Start()
    {
        // Start the fade-in process
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        // Ensure the fadeImage is black and fully opaque initially
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 1); // Fully opaque black
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); // Fade from black to clear

            if (fadeImage != null)
            {
                fadeImage.color = new Color(0, 0, 0, alpha);
            }

            yield return null;
        }

        // Ensure the fadeImage is fully transparent at the end
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0); // Fully transparent
        }
    }
}
