using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SafeController : MonoBehaviour
{
    public Animator doorAnimator;                 // Animator controlling the door animation
    public Animator otherObjectAnimator;          // Animator for the first additional object
    public Animator secondObjectAnimator;         // Animator for the second additional object
    public Transform playerTransform;             // Player's transform
    public TextMeshProUGUI promptText;            // TextMeshProUGUI for the prompt to press "E"
    public TextMeshProUGUI messageText;           // TextMeshProUGUI for displaying the "Go to the living room" message
    public GameObject codeInputPanel;             // UI panel containing the code input field and submit button
    public InputField codeInputField;             // Standard Unity InputField for code entry
    public Button submitButton;                   // Submit button to check the code
    public string correctCode = "415643";         // The correct code to open the safe
    public float activationDistance = 4f;         // Distance within which the player can interact with the safe

    public AudioSource safeDoorSound;             // AudioSource for the safe door sound
    public AudioSource otherDoorSound;            // AudioSource for the first additional object's sound
    public AudioSource secondDoorSound;           // AudioSource for the second additional object's sound

    private bool isPanelActive = false;           // To check if the input panel is active
    private bool isDoorOpen = false;              // To prevent reopening the door once it's open

    void Start()
    {
        // Ensure the input panel and prompt text are deactivated at the start
        codeInputPanel.SetActive(false);
        promptText.gameObject.SetActive(false);
        messageText.gameObject.SetActive(false);  // Hide the message text initially

        // Set the listener for the submit button
        submitButton.onClick.AddListener(CheckCode);
    }

    void Update()
    {
        if (isDoorOpen)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer <= activationDistance && !isPanelActive)
        {
            promptText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleInputPanel();
            }
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }

        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            CheckCode();
        }
    }

    void ToggleInputPanel()
    {
        isPanelActive = !isPanelActive;  // Toggle the panel's state
        codeInputPanel.SetActive(isPanelActive);
        promptText.gameObject.SetActive(!isPanelActive);  // Hide prompt text when the panel is active

        if (isPanelActive)
        {
            codeInputField.Select();
            codeInputField.ActivateInputField();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void CheckCode()
    {
        if (codeInputField.text == correctCode)
        {
            Debug.Log("Correct code! Opening the safe.");
            OpenSafe();
        }
        else
        {
            Debug.Log("Incorrect code.");
            // Optional: Display a message to the player about the incorrect code
        }

        codeInputField.text = "";
        codeInputPanel.SetActive(false);
        isPanelActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OpenSafe()
    {
        // Trigger the door opening animation
        doorAnimator.SetTrigger("OpenDoor");
        isDoorOpen = true;  // Set the flag to prevent reopening

        // Play the sound effect for the safe door
        if (safeDoorSound != null)
        {
            safeDoorSound.Play();
        }

        // Trigger the animation on the first additional object
        if (otherObjectAnimator != null)
        {
            Debug.Log("Opening the first additional object's animation.");
            otherObjectAnimator.SetBool("isOpen", true);

            // Play the sound effect for the first additional object
            if (otherDoorSound != null)
            {
                otherDoorSound.Play();
            }

            // Log the current state of the animation
            AnimatorStateInfo stateInfo = otherObjectAnimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"First Object - Current State: {stateInfo.shortNameHash}, IsPlaying: {stateInfo.IsName("YourAnimationStateName")}");
        }

        // Trigger the animation on the second additional object
        if (secondObjectAnimator != null)
        {
            Debug.Log("Opening the second additional object's animation.");
            secondObjectAnimator.SetBool("isOpen", true);

            // Play the sound effect for the second additional object
            if (secondDoorSound != null)
            {
                secondDoorSound.Play();
            }

            // Log the current state of the animation
            AnimatorStateInfo stateInfo = secondObjectAnimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Second Object - Current State: {stateInfo.shortNameHash}, IsPlaying: {stateInfo.IsName("YourAnimationStateName")}");
        }

        // Display the "Go to the living room" message
        messageText.text = "Go to the living room";
        messageText.gameObject.SetActive(true);

        // Start coroutine to hide the message after 2 seconds
        StartCoroutine(HideMessageAfterDelay(2f));

        promptText.gameObject.SetActive(false);
    }

    IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.gameObject.SetActive(false);
    }
}
