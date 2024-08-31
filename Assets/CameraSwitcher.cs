using UnityEngine;
using TMPro; // For TextMeshPro components

public class CameraSwitcher : MonoBehaviour
{
    public Camera targetCamera; // The camera to switch to
    public Camera mainCamera; // The original main camera (First-Person Camera)
    public float switchDistance = 3f; // The distance within which the player can switch the camera
    public KeyCode switchKey = KeyCode.E; // The key to press to switch the camera
    public GameObject player; // The player object with the character controller
    public Light spotlight; // The spotlight to be controlled
    public TextMeshProUGUI interactionText; // The TMP Text element to display interaction prompt
    public GameObject crosshair; // The crosshair GameObject

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
        }

        // Ensure the target camera is not the main camera at the start
        if (targetCamera != null)
        {
            targetCamera.tag = "Untagged";
            targetCamera.enabled = false;
        }

        // Ensure the spotlight is inactive at the start
        if (spotlight != null)
        {
            spotlight.enabled = false;
        }

        // Ensure the interaction text is initially inactive
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        // Ensure the crosshair is visible initially (when main camera is active)
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }
    }

    void Update()
    {
        // Calculate the distance between the player and the switch point
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Check if the player is within the switch distance
        if (distance <= switchDistance)
        {
            // Show the interaction text only if the target camera is not active
            if (interactionText != null)
            {
                if (!isCameraSwitched) // Check if the target camera is not active
                {
                    interactionText.text = "Press E to inspect";
                    interactionText.gameObject.SetActive(true);
                }
                else
                {
                    interactionText.gameObject.SetActive(false);
                }
            }

            // Check if the player presses the switch key
            if (Input.GetKeyDown(switchKey))
            {
                if (isCameraSwitched)
                {
                    // If the camera is switched, return to the original main camera
                    SwitchToMainCamera();
                }
                else
                {
                    // If the camera is not switched, switch to the target camera
                    SwitchToTargetCamera();
                }
            }
        }
        else
        {
            // Hide the interaction text if the player is too far away
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
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

        // Hide the cursor and lock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Hide the crosshair when the target camera is active
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }

        isCameraSwitched = true;
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

        // Show the crosshair when the main camera is active
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }

        isCameraSwitched = false;
    }
}
