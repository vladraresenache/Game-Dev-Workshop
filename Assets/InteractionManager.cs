using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    public Image blackImage;              // Reference to the black overlay image
    public Image whiteImage;              // Reference to the white overlay image
    public TextMeshProUGUI interactionText; // Reference to the interaction text
    public float fadeDuration = 1.0f;     // Duration of each fade (in and out)
    public float interactionDistance = 3.0f; // Distance at which the interaction text appears

    private bool isNearObject = false;
    private bool isFadingFromBlack = false;
    private bool isFadingToWhite = false;
    private bool isTextVisible = false;
    private bool hasSceneLoaded = false;
    private float fadeTimer;

    void Start()
    {
        // Initialize the alpha of images and text
        SetAlpha(blackImage, 1f); // Start fully black
        SetAlpha(whiteImage, 0f); // Start fully clear
        SetAlpha(interactionText, 0f); // Text starts invisible

        // Begin fading from black to clear
        isFadingFromBlack = true;
    }

    void Update()
    {
        HandleFadeEffects();
        HandleInteraction();
    }

    private void HandleFadeEffects()
    {
        // Fade from black to clear
        if (isFadingFromBlack)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(1.0f - (fadeTimer / fadeDuration));
            SetAlpha(blackImage, alpha);

            if (alpha <= 0f)
            {
                isFadingFromBlack = false;
                fadeTimer = 0f;
            }
        }

        // Fade from clear to white
        if (isFadingToWhite)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            SetAlpha(whiteImage, alpha);

            if (alpha >= 1f && !hasSceneLoaded)
            {
                hasSceneLoaded = true;
                LoadNextScene();
            }
        }
    }

    private void HandleInteraction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");  // Assuming the player has a "Player" tag
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= interactionDistance && !isTextVisible)
            {
                ShowInteractionText(true);
            }
            else if (distanceToPlayer > interactionDistance && isTextVisible)
            {
                ShowInteractionText(false);
            }

            if (isNearObject && Input.GetKeyDown(KeyCode.E))
            {
                StartFadeToWhite();
            }
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure the player object is tagged as 'Player'.");
        }
    }

    private void ShowInteractionText(bool visible)
    {
        isTextVisible = visible;
        isNearObject = visible;
        SetAlpha(interactionText, visible ? 1f : 0f);
    }

    private void StartFadeToWhite()
    {
        fadeTimer = 0f;
        isFadingToWhite = true;
    }

    private void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes in the build order!");
        }
    }

    // Method to set the alpha of any UI element
    private void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic != null)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }
}
