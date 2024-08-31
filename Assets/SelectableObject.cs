using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public string code; // Code for this object
    public Color selectedColor = Color.green; // The color to change to when selected
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
            GameManager.Instance.SelectObject(this);
        }
        else
        {
            Debug.LogError("GameManager.Instance is null. Make sure GameManager is properly initialized.");
        }
    }

    public void Select()
    {
        ChangeColor(selectedColor); // Change the color when the object is selected
    }

    public void Deselect()
    {
        ChangeColor(originalColor); // Revert to the original color when deselected
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
