using UnityEngine;
using UnityEngine.UI;

public class DoorController1 : MonoBehaviour
{
    public Animator animator;
    public InputField codeInputField;
    public Button submitButton;
    public GameObject inputPanel;  // An object that contains both InputField and Button
    public Transform playerTransform;
    private string secretCode = "0610";
    private bool isPanelActive = false;
    private float activationDistance = 5f;

    void Start()
    {
        // Ensure the input panel is deactivated at the start
        inputPanel.SetActive(false);

        // Set the listener for the submit button
        submitButton.onClick.AddListener(CheckCode);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        // Toggle the input panel on/off when the "E" key is pressed and the player is within range
        if (distanceToPlayer <= activationDistance && Input.GetKeyDown(KeyCode.E))
        {
            isPanelActive = !isPanelActive;  // Toggle the panel's state
            inputPanel.SetActive(isPanelActive);

            if (isPanelActive)
            {
                // Set focus on the InputField when the panel is activated
                codeInputField.Select();
                codeInputField.ActivateInputField();
            }
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
        // Set the `isOpen` parameter to true to trigger the door opening animation
        animator.SetBool("isOpen", true);
    }
}
