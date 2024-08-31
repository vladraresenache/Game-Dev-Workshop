using UnityEngine;

public class CodeInputObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AppendCodeToFifthObject();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null. Make sure GameManager is properly initialized.");
        }
    }
}
