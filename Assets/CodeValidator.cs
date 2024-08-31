using UnityEngine;

public class CodeValidator : MonoBehaviour
{
    public string correctCode = @"
public class SumCalculator
{
    public int CalculateSum(int n)
    {
        if (n <= 0)
        {
            return 0; // Handle invalid input
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

    public string incorrectCode = @"
public class SumCalculator
{
    public int CalculateSum(int n)
    {
        if (n < 0)
        {
            return -1; // Incorrectly handles invalid input
        }
        int sum = 0;
        for (int i = 0; i <= n; i++) // Mistake: starts loop from 0 instead of 1
        {
            sum += i;
        }
        return sum;
    }
}
";

    public string playerCode; // The player's code input

    void Start()
    {
        // Initialization code
    }

    public void ValidatePlayerCode()
    {
        if (playerCode == correctCode)
        {
            Debug.Log("The player's code is correct!");
            // Trigger any event that should happen when the code is correct
        }
        else
        {
            Debug.Log("The player's code is incorrect. Please try again.");
            // Provide feedback or allow the player to try again
        }
    }

    // Optional: A method to simulate setting player code (for testing)
    public void SetPlayerCode(string code)
    {
        playerCode = code;
    }
}
