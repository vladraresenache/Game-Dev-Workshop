using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorController1 : MonoBehaviour
{
    public Animator animator;
    public InputField codeInputField;
    public Button submitButton;
    public GameObject inputPanel;  // An object that contains both InputField and Button
    public Transform playerTransform;
    public TextMeshProUGUI proximityText;  // Reference to your TextMeshPro component
    public string secretCode = "";  // Can be empty to bypass code input
    public bool isInteractable = true;  // New variable to control whether the door is interactable
    private bool isPanelActive = false;
    private bool isDoorOpen = false;  // Flag to check if the door has been opened
    private float activationDistance = 4f;

    // Add references to the audio sources for opening and closing sounds
    public AudioSource openSound;
    public AudioSource closeSound;

    void Start()
    {
        // Ensure the input panel and proximity text are deactivated at the start
        inputPanel.SetActive(false);
        proximityText.gameObject.SetActive(false);

        // Set the listener for the submit button
        submitButton.onClick.AddListener(CheckCode);
    }

    void Update()
    {
        // If the door is not interactable, hide the proximity text and disable interaction
        if (!isInteractable)
        {
            proximityText.gameObject.SetActive(false);
            if (isPanelActive)
            {
                inputPanel.SetActive(false);
                isPanelActive = false;
            }
            return;
        }

        if (isDoorOpen)
        {
            // If the door is already open, hide the proximity text and disable the input panel if active
            proximityText.gameObject.SetActive(false);
            if (isPanelActive)
            {
                inputPanel.SetActive(false);
                isPanelActive = false;
            }
            return;
        }

        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        // Show proximity text when the player is within range and the panel is not active
        if (distanceToPlayer <= activationDistance && !isPanelActive)
        {
            proximityText.gameObject.SetActive(true);
        }
        else
        {
            proximityText.gameObject.SetActive(false);
        }

        // Automatically open the door if no code is required
        if (string.IsNullOrEmpty(secretCode))
        {
            if (distanceToPlayer <= activationDistance && Input.GetKeyDown(KeyCode.E))
            {
                OpenDoor();
            }
            return;  // Exit Update early since no code interaction is required
        }

        // Toggle the input panel on/off when the "E" key is pressed
        if (distanceToPlayer <= activationDistance && Input.GetKeyDown(KeyCode.E))
        {
            isPanelActive = !isPanelActive;  // Toggle the panel's state
            inputPanel.SetActive(isPanelActive);
            proximityText.gameObject.SetActive(!isPanelActive); // Hide the proximity text when the panel is active

            if (isPanelActive)
            {
                // Set focus on the InputField when the panel is activated
                codeInputField.Select();
                codeInputField.ActivateInputField();
            }
        }

        // Check if the "Enter" key is pressed while the input panel is active
        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            CheckCode();
        }
    }

    void CheckCode()
    {
        // Check if the entered code is correct
        if (codeInputField.text == secretCode)
        {
            Debug.Log("Correct code! Opening the door.");
            OpenDoor();
        }
        else
        {
            Debug.Log("Incorrect code.");
            // You can add a message for the user here if you want
        }

        // Reset the code input field
        codeInputField.text = "";

        // Hide the input panel after the code has been checked
        inputPanel.SetActive(false);
        isPanelActive = false;
    }

    void OpenDoor()
    {
        // Set the isOpen parameter to true to trigger the door opening animation
        animator.SetBool("isOpen", true);
        isDoorOpen = true;  // Set the flag to true to prevent reopening

        // Play the opening sound
        if (openSound != null)
        {
            openSound.Play();
        }
    }

    // Optional: Method to close the door and play the closing sound
    public void CloseDoor()
    {
        // Set the isOpen parameter to false to trigger the door closing animation
        animator.SetBool("isOpen", false);
        isDoorOpen = false;  // Reset the flag to allow reopening

        // Play the closing sound
        if (closeSound != null)
        {
            closeSound.Play();
        }
    }
}
