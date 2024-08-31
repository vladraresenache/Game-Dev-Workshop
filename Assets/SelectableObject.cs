using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public string code; // Code for this object

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
}
