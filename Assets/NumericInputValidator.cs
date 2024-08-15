using UnityEngine;
using UnityEngine.UI;

public class NumericInputValidator : MonoBehaviour
{
    private InputField inputField;

    void Start()
    {
        inputField = GetComponent<InputField>();
        inputField.contentType = InputField.ContentType.Custom; // Set content type to Custom to apply custom validation
        inputField.onValueChanged.AddListener(ValidateInput); // Add listener for value change
    }

    // Function to validate input and restrict it to numbers only
    void ValidateInput(string input)
    {
        string numericInput = string.Empty;

        foreach (char c in input)
        {
            if (char.IsDigit(c)) // Check if the character is a digit
            {
                numericInput += c;
            }
        }

        // Update the InputField text with valid numeric input
        inputField.text = numericInput;
    }
}
