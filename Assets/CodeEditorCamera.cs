using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeEditorCamera : MonoBehaviour
{
    public Camera codeEditorCamera; // The camera for the code editor
    public Camera puzzleCamera; // The camera for the puzzle
    public GameObject codeEditorUI; // UI for the code editor
    public CodeManager codeManager; // Reference to the CodeManager
    public GameObject player; // Reference to the player object
    public float interactionDistance = 3f; // Distance within which the player can interact with the computer
    public KeyCode interactionKey = KeyCode.E; // Key to press to interact with the computer
    public KeyCode exitKey = KeyCode.E; // Key to exit the code editor view

    public TMP_InputField codeInputField; // UI TMP_InputField for player to enter code
    public TextMeshProUGUI feedbackText; // UI TextMeshProUGUI to display feedback
    public Button submitButton; // Button to submit the code
    public Button resetButton; // Button to reset the code input field to the original text

    public Animator doorAnimator1; // Animator for the first door object
    public Animator doorAnimator2; // Animator for the second door object
    public AudioSource doorSound1; // AudioSource for the first door object's sound
    public AudioSource doorSound2; // AudioSource for the second door object's sound

    public TextMeshProUGUI interactMessage; // UI TextMeshProUGUI to display the interaction message
    public TextMeshProUGUI puzzleHintMessage; // UI TextMeshProUGUI to display the hint message
    public TextMeshProUGUI bathroomMessage; // UI TextMeshProUGUI to display the bathroom message

    private bool isNearComputer = false; // Whether the player is near the computer
    private bool isCameraSwitched = false; // Whether the camera is currently switched
    private bool isDoorOpen = false; // To prevent reopening doors once they're open
    private CharacterController characterController; // Reference to the CharacterController

    // Store the correct code as a constant string
    private const string correctCode = @"
public class SumCalculator
{
    public int CalculateSum(int n)
    {
        if (n <= 0)
        {
            return 0;
        }
        int sum = 0;
        for (int i = 1; i <= n; i++)
        {
            sum += i;
        }
        return sum;
    }
}
";

    // Store the wrong code as a constant string
    private const string wrongCode = @"
public class SumCalculator
{
    public int CalculateSum(int n)
    {
        if (n <= 0)
        {
            return 0
        }
        int sum = 0;
        for (int i = 1; i < n; i++)
        {
            sum += i;
        }
        return i;
    }
}
";

    void Start()
    {
        // Find the character controller on the player object
        characterController = player.GetComponent<CharacterController>();

        // Ensure the puzzle camera is initially the main camera
        if (puzzleCamera != null)
        {
            puzzleCamera.tag = "MainCamera";
            puzzleCamera.enabled = true;
        }

        // Ensure the code editor camera is not the main camera at the start
        if (codeEditorCamera != null)
        {
            codeEditorCamera.tag = "Untagged";
            codeEditorCamera.enabled = false;
        }

        // Ensure the code editor UI is initially inactive
        if (codeEditorUI != null)
        {
            codeEditorUI.SetActive(false);
        }

        // Ensure the feedback text is initially empty
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        // Set up the submit button
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(SubmitCode);
        }

        // Set up the reset button
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetCodeInputField);
        }

        // Set the initial text in the input field to the wrong code
        if (codeInputField != null)
        {
            codeInputField.text = wrongCode;
        }

        // Ensure the interaction message is initially hidden
        if (interactMessage != null)
        {
            interactMessage.gameObject.SetActive(false);
        }

        // Ensure the puzzle hint message is initially hidden
        if (puzzleHintMessage != null)
        {
            puzzleHintMessage.gameObject.SetActive(false);
        }

        // Ensure the bathroom message is initially hidden
        if (bathroomMessage != null)
        {
            bathroomMessage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Calculate the distance between the player and the computer
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Check if the player is within the interaction distance
        if (distance <= interactionDistance)
        {
            // Show the interaction message
            if (interactMessage != null)
            {
                interactMessage.text = isCameraSwitched ? "Go to the bookshelf first" : "Press E to inspect";
                interactMessage.gameObject.SetActive(true);
            }

            // Check if the player presses the interaction key
            if (Input.GetKeyDown(interactionKey))
            {
                if (isCameraSwitched)
                {
                    SwitchToPuzzleCamera();
                }
                else if (codeManager.IsFibonacciSequence(codeManager.boolVector))
                {
                    SwitchToCodeEditorCamera();
                }
                else
                {
                    ShowPuzzleHintMessage();
                }
            }
        }
        else
        {
            // Hide the interaction message if the player is too far away
            if (interactMessage != null)
            {
                interactMessage.gameObject.SetActive(false);
            }
        }
    }

    private void SwitchToCodeEditorCamera()
    {
        // Disable the puzzle camera
        if (puzzleCamera != null)
        {
            puzzleCamera.enabled = false;
            puzzleCamera.tag = "Untagged";
        }

        // Enable the code editor camera
        if (codeEditorCamera != null)
        {
            codeEditorCamera.enabled = true;
            codeEditorCamera.tag = "MainCamera";
        }

        // Show the code editor UI
        if (codeEditorUI != null)
        {
            codeEditorUI.SetActive(true);
            // Disable the character controller when the UI is active
            if (characterController != null)
            {
                characterController.enabled = false;
            }
        }

        // Make the cursor visible and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isCameraSwitched = true;
    }

    private void SwitchToPuzzleCamera()
    {
        // Disable the code editor camera
        if (codeEditorCamera != null)
        {
            codeEditorCamera.enabled = false;
            codeEditorCamera.tag = "Untagged";
        }

        // Enable the puzzle camera
        if (puzzleCamera != null)
        {
            puzzleCamera.enabled = true;
            puzzleCamera.tag = "MainCamera";
        }

        // Hide the code editor UI
        if (codeEditorUI != null)
        {
            codeEditorUI.SetActive(false);
            // Re-enable the character controller when the UI is hidden
            if (characterController != null)
            {
                characterController.enabled = true;
            }
        }

        // Hide the cursor and lock it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isCameraSwitched = false;
    }

    private void ShowPuzzleHintMessage()
    {
        if (puzzleHintMessage != null)
        {
            puzzleHintMessage.text = "Solve the Bookcase puzzle first!";
            puzzleHintMessage.gameObject.SetActive(true);
            StartCoroutine(HidePuzzleHintMessage());
        }
    }

    private IEnumerator HidePuzzleHintMessage()
    {
        yield return new WaitForSeconds(3f);
        if (puzzleHintMessage != null)
        {
            puzzleHintMessage.gameObject.SetActive(false);
        }
    }

    public void SubmitCode()
    {
        // Check if codeInputField is assigned
        if (codeInputField != null)
        {
            string playerCode = codeInputField.text;

            // Validate the player's code
            if (IsCodeCorrect(playerCode))
            {
                feedbackText.text = "The code is correct! Head to the bathroom";
                Debug.Log("Code is correct!");
                OpenDoors(); // Open the doors when the correct code is entered

                // Display "Go to the bathroom" for 2 seconds
                StartCoroutine(ShowBathroomMessage());
            }
            else
            {
                feedbackText.text = "The code is incorrect. Please try again.";
                Debug.Log("Code is incorrect.");
            }
        }
    }

    private bool IsCodeCorrect(string playerCode)
    {
        // Check if the player's code matches the correct code
        return playerCode.Trim() == correctCode.Trim();
    }

    private IEnumerator ShowBathroomMessage()
    {
        if (bathroomMessage != null)
        {
            bathroomMessage.text = "Go to the bathroom";
            bathroomMessage.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            bathroomMessage.gameObject.SetActive(false);
        }
    }

    private void OpenDoors()
    {
        if (isDoorOpen)
            return;

        // Open the first door by setting isOpen to true and play the sound
        if (doorAnimator1 != null)
        {
            doorAnimator1.SetBool("isOpen", true);
            if (doorSound1 != null)
            {
                doorSound1.Play();
            }
        }

        // Open the second door by setting isOpen to true and play the sound
        if (doorAnimator2 != null)
        {
            doorAnimator2.SetBool("isOpen", true);
            if (doorSound2 != null)
            {
                doorSound2.Play();
            }
        }

        isDoorOpen = true; // Prevent doors from being opened again
    }

    private void ResetCodeInputField()
    {
        // Reset the text in the code input field to the wrong code
        if (codeInputField != null)
        {
            codeInputField.text = wrongCode;
            feedbackText.text = ""; // Clear the feedback text as well
            Debug.Log("Code input field reset to the wrong code.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearComputer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearComputer = false;
        }
    }
}
