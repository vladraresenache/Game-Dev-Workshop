using UnityEngine;
using TMPro; // For TextMeshPro components

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string predefinedCode = "1234"; // Predefined code to check against

    public string currentCode = ""; // Current code being built
    private SelectableObject selectedObject; // Currently selected object

    public Animator doorAnimator1; // Animator for the first door
    public Animator doorAnimator2; // Animator for the second door
    public AudioSource doorSound1; // AudioSource for the first door's sound
    public AudioSource doorSound2; // AudioSource for the second door's sound

    public TextMeshProUGUI successMessageText; // TMP Text for success messages
    public TextMeshProUGUI errorMessageText; // TMP Text for error messages

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep GameManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this method when an object is clicked
    public void SelectObject(SelectableObject obj)
    {
        selectedObject = obj;
    }

    // Call this method to append the code to the 5th object
    public void AppendCodeToFifthObject()
    {
        if (selectedObject != null)
        {
            if (currentCode.Length < 4)
            {
                string newCode = selectedObject.code;
                if (currentCode.Length + newCode.Length <= 4)
                {
                    currentCode += newCode;
                    selectedObject = null; // Clear selection

                    // Check if the code is complete
                    if (currentCode.Length == 4)
                    {
                        CheckCode();
                    }
                }
                else
                {
                    Debug.Log("Cannot add more codes. Maximum length is 4 characters.");
                }
            }
            else
            {
                Debug.Log("Code length is already at maximum of 4 characters.");
            }
        }
        else
        {
            Debug.Log("No object selected.");
        }
    }

    // Check if the current code matches the predefined code
    private void CheckCode()
    {
        if (currentCode == predefinedCode)
        {
            Debug.Log("Code matched!");
            OpenDoors(); // Open the doors when the code is correct
            DisplayMessage(successMessageText, "Code is correct! Head to the basement. "); // Display success message
        }
        else
        {
            Debug.Log("Code did not match. Try again.");
            DisplayMessage(errorMessageText, "Code is incorrect. Try again."); // Display error message
            // Reset the code
            currentCode = "";
        }
    }

    // Open the doors
    private void OpenDoors()
    {
        if (doorAnimator1 != null)
        {
            doorAnimator1.SetBool("isOpen", true);
            if (doorSound1 != null)
            {
                doorSound1.Play();
            }
        }

        if (doorAnimator2 != null)
        {
            doorAnimator2.SetBool("isOpen", true);
            if (doorSound2 != null)
            {
                doorSound2.Play();
            }
        }

        // Optionally, you can add code to disable further code entry if necessary
    }

    // Display a message on the screen
    private void DisplayMessage(TextMeshProUGUI textComponent, string message)
    {
        if (textComponent != null)
        {
            textComponent.text = message;
            textComponent.gameObject.SetActive(true);

            // Hide the message after a delay (optional)
            Invoke("HideMessage", 3f); // Hide after 3 seconds
        }
    }

    // Hide the message text
    private void HideMessage()
    {
        if (successMessageText != null)
        {
            successMessageText.gameObject.SetActive(false);
        }

        if (errorMessageText != null)
        {
            errorMessageText.gameObject.SetActive(false);
        }
    }

    // Retrieve the current code if needed
    public string GetCurrentCode()
    {
        return currentCode;
    }
}
