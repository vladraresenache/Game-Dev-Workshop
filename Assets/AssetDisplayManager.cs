using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssetDisplayManager : MonoBehaviour
{
    public List<string> texts = new List<string> { };
    public TextMeshProUGUI displayText; // TextMeshProUGUI component to display the text
    public Button[] buttons; // Array of buttons for switching texts
    public TextMeshProUGUI[] buttonLabels; // Array of TextMeshProUGUI components for button labels

    private bool[] textUnlocked; // Array to track which texts are unlocked
    private int currentTextIndex = -1; // Index of the currently displayed text

    void Start()
    {
        textUnlocked = new bool[texts.Count];
        for (int i = 0; i < textUnlocked.Length; i++)
        {
            textUnlocked[i] = false; // Lock all texts initially
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Capture the index in a local variable
            buttons[i].onClick.AddListener(() => DisplayText(index));
            UpdateButtonLabel(index);
        }

        if (displayText != null)
        {
            displayText.gameObject.SetActive(false); // Hide the text display initially
        }
    }

    public void DisplayText(int index)
    {
        if (index < 0 || index >= texts.Count) return;

        if (!textUnlocked[index])
        {
            Debug.Log("Text is locked and cannot be displayed yet.");
            return;
        }

        string selectedText = texts[index].Replace(@"\n", "\n");
        if (displayText != null)
        {
            displayText.text = selectedText; // Set the text to display
            displayText.gameObject.SetActive(true); // Show the text display
        }

        currentTextIndex = index; // Update the current text index
    }

    public void UnlockText(int index)
    {
        if (index >= 0 && index < textUnlocked.Length)
        {
            if (!textUnlocked[index]) // Only unlock if it's not already unlocked
            {
                textUnlocked[index] = true;
                UpdateButtonLabel(index); // Update the button label when a text is unlocked
            }
        }
    }

    public bool IsTextUnlocked(int index)
    {
        return index >= 0 && index < textUnlocked.Length && textUnlocked[index];
    }

    private void UpdateButtonLabel(int index)
    {
        if (index >= 0 && index < buttonLabels.Length)
        {
            if (textUnlocked[index])
            {
                buttonLabels[index].text = "Read"; // Change button text to "Read" when unlocked
            }
            else
            {
                buttonLabels[index].text = "Locked"; // Change button text to "Locked" when locked
            }
        }
    }

}
