using UnityEngine;
using TMPro; // Make sure to include the TMPro namespace

public class BookShelfCamera : MonoBehaviour
{
    public Camera targetCamera; // The camera to switch to
    public Camera mainCamera; // The original main camera (First-Person Camera)
    public float switchDistance = 3f; // The distance within which the player can switch the camera
    public KeyCode switchKey = KeyCode.E; // The key to press to switch the camera
    public GameObject player; // The player object with the character controller
    public TextMeshProUGUI inspectMessage; // The TextMeshProUGUI element to display the message
    public Light spotlight; // The spotlight pointing to the bookshelf

    private bool isCameraSwitched = false; // Whether the camera is currently switched
    private CharacterController characterController; // The character controller component

    void Start()
    {
        // Find the character controller on the player object
        characterController = player.GetComponent<CharacterController>();

        // Ensure that the main camera has the "MainCamera" tag initially
        if (mainCamera != null)
        {
            mainCamera.tag = "MainCamera";
            mainCamera.enabled = true;
        }

        // Ensure the target camera is not the main camera at the start
        if (targetCamera != null)
        {
            targetCamera.tag = "Untagged";
            targetCamera.enabled = false;
        }

        // Ensure the spotlight is disabled at the start
        if (spotlight != null)
        {
            spotlight.enabled = false;
        }

        // Hide the inspect message initially
        if (inspectMessage != null)
        {
            inspectMessage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Calculate the distance between the player and the switch point
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Check if the player is within the switch distance
        if (distance <= switchDistance)
        {
            // Show the inspect message
            if (inspectMessage != null)
            {
                inspectMessage.text = isCameraSwitched ? "Press E to exit" : "Press E to inspect";
                inspectMessage.gameObject.SetActive(true);
            }

            // Check if the player presses the switch key
            if (Input.GetKeyDown(switchKey))
            {
                if (isCameraSwitched)
                {
                    SwitchToMainCamera(); // Switch back to the main camera
                }
                else
                {
                    SwitchToTargetCamera(); // Switch to the target camera
                }
            }
        }
        else
        {
            // Hide the inspect message if the player is too far away
            if (inspectMessage != null)
            {
                inspectMessage.gameObject.SetActive(false);
            }
        }
    }

    void SwitchToTargetCamera()
    {
        // Disable the main camera
        if (mainCamera != null)
        {
            mainCamera.enabled = false;
            mainCamera.tag = "Untagged";
        }

        // Enable the target camera
        if (targetCamera != null)
        {
            targetCamera.enabled = true;
            targetCamera.tag = "MainCamera";
        }

        // Enable the spotlight
        if (spotlight != null)
        {
            spotlight.enabled = true;
        }

        // Disable the character controller
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Make the cursor visible and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isCameraSwitched = true;

        // Hide the inspect message when switching the camera
        if (inspectMessage != null)
        {
            inspectMessage.gameObject.SetActive(false);
        }
    }

    void SwitchToMainCamera()
    {
        // Disable the target camera
        if (targetCamera != null)
        {
            targetCamera.enabled = false;
            targetCamera.tag = "Untagged";
        }

        // Enable the main camera
        if (mainCamera != null)
        {
            mainCamera.enabled = true;
            mainCamera.tag = "MainCamera";
        }

        // Disable the spotlight
        if (spotlight != null)
        {
            spotlight.enabled = false;
        }

        // Enable the character controller
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        // Hide the cursor and lock it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isCameraSwitched = false;

        // Hide the inspect message when switching back to the main camera
        if (inspectMessage != null)
        {
            inspectMessage.gameObject.SetActive(false);
        }
    }
}
