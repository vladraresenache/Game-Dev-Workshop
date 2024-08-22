using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public TextMeshProUGUI notificationText; // Reference to the TextMeshProUGUI component
    public float fadeDuration = 1f; // Duration for the fade effect

    void Start()
    {
        if (notificationText != null)
        {
            notificationText.gameObject.SetActive(false); // Ensure the notification is initially hidden
        }
    }

    public void ShowNotification(string message)
    {
        if (notificationText != null)
        {
            StartCoroutine(DisplayNotification(message));
        }
    }

    private IEnumerator DisplayNotification(string message)
    {
        // Set the message and show the text
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, 1f); // Ensure text is fully opaque

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Fade out the text over the fadeDuration period
        float elapsedTime = 0f;
        Color startColor = notificationText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            notificationText.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure the text is fully transparent and hide it
        notificationText.color = endColor;
        notificationText.gameObject.SetActive(false);
    }
}
