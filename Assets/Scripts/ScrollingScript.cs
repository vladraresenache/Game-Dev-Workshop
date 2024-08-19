using UnityEngine;
using TMPro;
using UnityEngine.UI;  // For UI Image
using System.Collections;
using UnityEngine.SceneManagement;

public class GradualTextReveal : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;       // Main text display
    public TextMeshProUGUI skipText;          // "Press Space to Skip" message
    public TextMeshProUGUI continueText;      // "Press Enter to Continue" message
    public Image fadeImage;                   // UI Image for fade to black
    public float wordRevealDelay = 0.5f;      // Delay between revealing each word
    public float startDelay = 2f;             // Initial delay before revealing starts
    public float paragraphDelay = 2f;         // Delay between paragraphs
    public float fadeDuration = 5f;           // Duration of the fade to black

    private string fullText;
    private Coroutine revealCoroutine;
    private bool isSkipping = false;          // Flag to check if the text is being skipped
    private bool isTextFullyRevealed = false; // Flag to check if the entire text is revealed

    void Start()
    {
        // Store the full text and make the text initially invisible
        fullText = textMeshPro.text;
        textMeshPro.text = "";  // Hide the full text initially

        // Set the skipText to be visible
        if (skipText != null)
        {
            skipText.text = "Press Space to Skip";  // Set skip message
        }

        // Hide the "Press Enter to Continue" message and fade image initially
        if (continueText != null)
        {
            continueText.text = "";
        }

        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0); // Transparent black
        }

        // Start the coroutine to reveal the text
        revealCoroutine = StartCoroutine(RevealText());
    }

    void Update()
    {
        // Check if the player presses the spacebar and if the text is not already being skipped
        if (Input.GetKeyDown(KeyCode.Space) && !isSkipping)
        {
            isSkipping = true;

            // Stop the gradual reveal coroutine
            if (revealCoroutine != null)
            {
                StopCoroutine(revealCoroutine);
            }

            // Reveal the entire text immediately
            textMeshPro.text = fullText;
            isTextFullyRevealed = true;

            // Hide the "Press Space to Skip" message
            if (skipText != null)
            {
                skipText.text = "";
            }

            // Start the fade process
            StartCoroutine(FadeToBlackAndShowContinue());
        }

        // Check if the player presses Enter to continue to the next scene
        if (isTextFullyRevealed && Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextSceneInBuildOrder();
        }
    }

    IEnumerator RevealText()
    {
        // Initial start delay
        yield return new WaitForSeconds(startDelay);

        // Split the full text into paragraphs
        string[] paragraphs = fullText.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);

        foreach (string paragraph in paragraphs)
        {
            // Split the paragraph into words
            string[] words = paragraph.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                // If skipping is triggered, break out of the loop
                if (isSkipping)
                {
                    yield break;
                }

                // Append the next word to the text
                textMeshPro.text += words[i] + " ";
                yield return new WaitForSeconds(wordRevealDelay);
            }

            // Add a new line for the next paragraph if it's not the last paragraph
            if (paragraph != paragraphs[paragraphs.Length - 1])
            {
                textMeshPro.text += "\n\n";
                // Delay before revealing the next paragraph
                yield return new WaitForSeconds(paragraphDelay);
            }
        }

        // When all text is revealed, set the flag and start the fade process
        isTextFullyRevealed = true;
        StartCoroutine(FadeToBlackAndShowContinue());
    }

    IEnumerator FadeToBlackAndShowContinue()
    {
        // Wait for 3 seconds before starting the fade
        yield return new WaitForSeconds(3f);

        // Fade to black
        float elapsedTime = 0f;

        // Move the "Press Enter to Continue" text above the fade image (optional)
        if (continueText != null)
        {
            continueText.transform.SetAsLastSibling();
            continueText.text = "Press Enter to Continue";
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            if (fadeImage != null)
            {
                fadeImage.color = new Color(0, 0, 0, alpha);
            }

            yield return null;
        }

        // After the fade, only the "Press Enter to Continue" text should be visible
        if (skipText != null)
        {
            skipText.text = "";
        }

        if (textMeshPro != null)
        {
            textMeshPro.text = "";
        }
    }

    void LoadNextSceneInBuildOrder()
    {
        // Load the next scene in the build order
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes in build order!");
            // Optionally, you can reload the current scene or do something else
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
