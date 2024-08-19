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
    private string secretCode = "0610";
    private bool isPanelActive = false;
    private float activationDistance = 5f;

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
        // Set the `isOpen` parameter to true to trigger the door opening animation
        animator.SetBool("isOpen", true);
    }
}
