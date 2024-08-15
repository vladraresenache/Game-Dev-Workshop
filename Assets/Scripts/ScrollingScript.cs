using UnityEngine;
using TMPro;
using System.Collections;

public class GradualTextReveal : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float wordRevealDelay = 0.5f; // Delay between revealing each word
    public float startDelay = 2f; // Initial delay before revealing starts
    public float paragraphDelay = 2f; // Delay between paragraphs

    private string fullText;

    void Start()
    {
        // Store the full text and make the text initially invisible
        fullText = textMeshPro.text;
        textMeshPro.text = "";
        // Start the coroutine to reveal the text
        StartCoroutine(RevealText());
    }

    IEnumerator RevealText()
    {
        // Initial start delay
        yield return new WaitForSeconds(startDelay);

        // Split the full text into paragraphs
        string[] paragraphs = fullText.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);

        foreach (string paragraph in paragraphs)
        {
            // Split the paragraph into words
            string[] words = paragraph.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                // Append the next word to the text
                textMeshPro.text += words[i] + " ";
                yield return new WaitForSeconds(wordRevealDelay);
            }

            // Add a new line for the next paragraph if it's not the last paragraph
            if (paragraph != paragraphs[paragraphs.Length - 1])
            {
                textMeshPro.text += "\n\n";
                // Delay before revealing the next paragraph
                yield return new WaitForSeconds(paragraphDelay);
            }
        }
    }
}
