using UnityEngine;
using System.Collections; // Include this namespace to use IEnumerator

public class CodeInputObject : MonoBehaviour
{
    public Color selectedColor = Color.white; // The color to change to when selected
    private Color originalColor; // Store the original color
    private Material objectMaterial; // Reference to the object's material

    private void Start()
    {
        // Get the material of the object
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            objectMaterial = renderer.material;
            originalColor = objectMaterial.color; // Store the original color of the material
        }
        else
        {
            Debug.LogError("No Renderer found on the object. Make sure the object has a Renderer component.");
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AppendCodeToFifthObject();
            StartCoroutine(FlashColor());
        }
        else
        {
            Debug.LogError("GameManager.Instance is null. Make sure GameManager is properly initialized.");
        }
    }

    // Coroutine to briefly change color and revert after 0.5 seconds
    private IEnumerator FlashColor()
    {
        ChangeColor(selectedColor); // Change the color when the code is appended
        yield return new WaitForSeconds(0.5f); // Wait for half a second
        ChangeColor(originalColor); // Revert to the original color
    }

    private void ChangeColor(Color newColor)
    {
        if (objectMaterial != null)
        {
            objectMaterial.color = newColor; // Change the material color
        }
        else
        {
            Debug.LogError("objectMaterial is null. Ensure the object has a material.");
        }
    }
}
