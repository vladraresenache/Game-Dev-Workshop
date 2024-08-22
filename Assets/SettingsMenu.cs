using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuCanvas;
    public GameObject pauseMenuCanvas;

    public Slider soundSlider;
    public TMP_Dropdown graphicsDropdown;

    public AudioMixer audioMixer;

    private void Start()
    {
        settingsMenuCanvas.SetActive(false);

        // Set the slider value to match the current audio level
        float volume;
        audioMixer.GetFloat("MasterVolume", out volume);
        soundSlider.value = volume;

        // Set the graphics dropdown to the current quality level
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToPauseMenu();
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void BackToPauseMenu()
    {
        settingsMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }
}
