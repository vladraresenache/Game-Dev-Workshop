using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Import TMPro namespace for TMP_Dropdown
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;    // The options menu canvas or panel
    [SerializeField] private GameObject mainMenu;        // The main menu canvas or panel
    [SerializeField] private Button optionsButton;      // The button to open the options menu
    [SerializeField] private Button backButton;         // The button to go back to the main menu
    [SerializeField] private Slider soundSlider;        // Slider for sound settings
    [SerializeField] private TMP_Dropdown graphicsDropdown; // Dropdown for graphics quality settings (TextMeshPro version)

    private void Start()
    {
        // Initially hide the options menu
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(false);
        }

        // Assign button listeners
        if (optionsButton != null)
        {
            optionsButton.onClick.AddListener(ShowOptionsMenu);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(HideOptionsMenu);
        }

        // Initialize settings
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        if (soundSlider != null)
        {
            float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1.0f); // Default volume 1.0
            soundSlider.value = soundVolume;
            // Apply sound setting
            AudioListener.volume = soundVolume; // Set the global audio volume
        }

        if (graphicsDropdown != null)
        {
            int graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 2); // Default value 2
            graphicsDropdown.value = graphicsQuality;
            QualitySettings.SetQualityLevel(graphicsQuality);
        }
    }

    public void ApplySettings()
    {
        if (soundSlider != null)
        {
            float soundVolume = soundSlider.value;
            PlayerPrefs.SetFloat("SoundVolume", soundVolume);
            AudioListener.volume = soundVolume; // Set the global audio volume
        }

        if (graphicsDropdown != null)
        {
            int graphicsQuality = graphicsDropdown.value;
            PlayerPrefs.SetInt("GraphicsQuality", graphicsQuality);
            QualitySettings.SetQualityLevel(graphicsQuality);
        }

        PlayerPrefs.Save(); // Save settings
    }

    public void ShowOptionsMenu()
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(true); // Show the options menu
        }

        if (mainMenu != null)
        {
            mainMenu.SetActive(false); // Hide the main menu
        }
    }

    public void HideOptionsMenu()
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(false); // Hide the options menu
        }

        if (mainMenu != null)
        {
            mainMenu.SetActive(true); // Show the main menu
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name or index
    }
}
