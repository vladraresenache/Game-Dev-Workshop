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
        
    }

    public void SetVolume()
    {
        float volume = soundSlider.value;
        Debug.Log("Volume set to: " + volume);
        // Convert the slider value to a logarithmic scale if your slider is in linear scale (0 to 1)
        // If the slider is already in dB (-80 to 0), this step is unnecessary
        float dbVolume = Mathf.Log10(volume) * 20;

        audioMixer.SetFloat("MasterVolume", dbVolume);
    }

    public void SetGraphicsQuality()
    {
        int qualityIndex = graphicsDropdown.value;
        QualitySettings.SetQualityLevel(qualityIndex);

        // Close the settings menu after setting graphics quality
        BackToPauseMenu();
    }

    public void BackToPauseMenu()
    {
        settingsMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }
}
