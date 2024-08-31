using UnityEngine;

public class CodeManager : MonoBehaviour
{
    public bool[] boolVector = new bool[35]; // Vector to store the sequence of book codes
    private bool[] fibonacciVector; // Vector representing the Fibonacci sequence in bool format

    public AudioSource successSound; // AudioSource to play when the Fibonacci sequence matches
    public AudioClip successClip; // AudioClip for the success sound

    void Start()
    {
        // Initialize the bool vector to false
        for (int i = 0; i < boolVector.Length; i++)
        {
            boolVector[i] = false;
        }

        // Generate the Fibonacci sequence as a bool vector
        fibonacciVector = GenerateFibonacciVector(boolVector.Length);

        // Log the initial Fibonacci vector
        LogFibonacciVector(fibonacciVector, "Initial Fibonacci Vector");
    }

    public void UpdateBoolVector(int bookCode)
    {
        // Ensure the bookCode is within the valid range
        if (bookCode >= 0 && bookCode < boolVector.Length)
        {
            // Update the bool vector at the index corresponding to the bookCode
            boolVector[bookCode] = !boolVector[bookCode];

            // Log the current user vector and the correct Fibonacci vector
            LogFibonacciVector(fibonacciVector, "Fibonacci Vector");
            LogUserVector(boolVector, "Current User Vector");

            // Check if the boolVector matches the Fibonacci sequence
            if (IsFibonacciSequence(boolVector))
            {
                Debug.Log("The boolVector matches the Fibonacci sequence!");
                PlaySuccessSound();
            }
            else
            {
                Debug.Log("The boolVector does not match the Fibonacci sequence.");
            }
        }
        else
        {
            Debug.LogError("Invalid bookCode: " + bookCode);
        }
    }

    public bool IsBookTrue(int bookCode)
    {
        if (bookCode >= 0 && bookCode < boolVector.Length)
        {
            return boolVector[bookCode];
        }
        return false;
    }

    private bool[] GenerateFibonacciVector(int length)
    {
        bool[] fibVector = new bool[length];

        // Start with initial Fibonacci numbers
        int a = 0, b = 1;
        fibVector[0] = false; // Index 0 is not used
        if (length > 1) fibVector[1] = true;  // Index 1 is the first Fibonacci number

        // Generate Fibonacci sequence and mark the bool vector
        for (int i = 2; i < length; i++)
        {
            int next = a + b;
            a = b;
            b = next;

            // Mark index as true if it is a Fibonacci number
            if (next < length)
            {
                fibVector[next] = true;
            }
        }

        return fibVector;
    }

    public bool IsFibonacciSequence(bool[] sequence)
    {
        if (sequence.Length != fibonacciVector.Length)
        {
            return false;
        }

        // Check if the sequence matches the predefined Fibonacci vector
        for (int i = 0; i < sequence.Length; i++)
        {
            if (sequence[i] != fibonacciVector[i])
            {
                return false;
            }
        }

        return true;
    }

    private void LogFibonacciVector(bool[] vector, string label)
    {
        // Log the Fibonacci vector to the Unity Console with a label
        string vectorString = label + ": ";
        for (int i = 0; i < vector.Length; i++)
        {
            vectorString += vector[i] ? "1 " : "0 ";
        }
        Debug.Log(vectorString.Trim());
    }

    private void LogUserVector(bool[] vector, string label)
    {
        // Log the user vector to the Unity Console with a label
        string vectorString = label + ": ";
        for (int i = 0; i < vector.Length; i++)
        {
            vectorString += vector[i] ? "1 " : "0 ";
        }
        Debug.Log(vectorString.Trim());
    }

    private void PlaySuccessSound()
    {
        if (successSound != null && successClip != null)
        {
            successSound.clip = successClip;
            successSound.Play();
        }
        else
        {
            Debug.LogWarning("SuccessSound or SuccessClip is not set.");
        }
    }
}
