using UnityEngine;

public class BookInteraction : MonoBehaviour
{
    public int bookCode; // Unique code for each book
    public CodeManager gameManager; // Reference to the GameManager
    public float moveDistance = 0.03f; // Distance to move the book along the x-axis when selected
    private Color selectedColor = Color.black; // Color to indicate selection
    private Color originalColor; // Store the original color of the second material

    private Renderer bookRenderer; // Renderer component of the book
    private Vector3 originalPosition; // Store the original position of the book

    void Start()
    {
        // Store the original position of the book
        originalPosition = transform.position;

        // Get the Renderer component
        bookRenderer = GetComponent<Renderer>();
        if (bookRenderer != null)
        {
            // Check if there are at least two materials
            if (bookRenderer.materials.Length > 1)
            {
                // Store the original color of the second material
                originalColor = bookRenderer.materials[1].color;
            }
            else
            {
                Debug.LogError("The book does not have a second material.");
            }
        }
        else
        {
            Debug.LogError("Renderer component is missing from the book.");
        }
    }

    void OnMouseDown()
    {
        if (gameManager != null)
        {
            // Check if the book is at its original position and move it if so
            if (IsAtOriginalPosition())
            {
                MoveBook();
            }

            // Update the bool vector in GameManager based on the bookCode
            gameManager.UpdateBoolVector(bookCode);

            // Change the book's second material color to indicate it has been pressed
            if (bookRenderer != null && bookRenderer.materials.Length > 1)
            {
                var materials = bookRenderer.materials;
                materials[1].color = gameManager.IsBookTrue(bookCode) ? selectedColor : originalColor;
                bookRenderer.materials = materials; // Apply changes
            }
        }
    }

    public void MoveBook()
    {
        // Move the book by the specified distance along the x-axis
        transform.position += new Vector3(moveDistance, 0, 0);

        // Log the new position to verify movement
        Debug.Log("Book moved to: " + transform.position);
    }

    public void ResetBookPosition()
    {
        // Reset the book to its original position
        transform.position = originalPosition;

        // Log reset to verify
        Debug.Log("Book position reset to: " + transform.position);
    }

    private bool IsAtOriginalPosition()
    {
        return transform.position == originalPosition;
    }
}
