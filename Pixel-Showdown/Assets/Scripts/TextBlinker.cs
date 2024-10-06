using System.Collections;
using UnityEngine;
using TMPro;

public class TextBlinker : MonoBehaviour
{
    public TextMeshProUGUI textToBlink; // Référence au composant TextMeshProUGUI
    public float blinkDuration = 0.5f; // Durée d'un cycle de clignotement

    private void Start()
    {
        if (textToBlink == null)
        {
            textToBlink = GetComponent<TextMeshProUGUI>();
        }

        if (textToBlink != null)
        {
            StartCoroutine(BlinkText());
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found! Please assign the textToBlink field in the inspector.");
        }
    }

    private IEnumerator BlinkText()
    {
        Color originalColor = textToBlink.color;
        float halfBlinkDuration = blinkDuration / 2f;

        while (true)
        {
            // Fade out
            for (float t = 0; t < halfBlinkDuration; t += Time.deltaTime)
            {
                float normalizedTime = t / halfBlinkDuration;
                textToBlink.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, normalizedTime));
                yield return null;
            }

            // Make sure the alpha is set to 0
            textToBlink.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

            // Fade in
            for (float t = 0; t < halfBlinkDuration; t += Time.deltaTime)
            {
                float normalizedTime = t / halfBlinkDuration;
                textToBlink.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, 1, normalizedTime));
                yield return null;
            }

            // Make sure the alpha is set to 1
            textToBlink.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        }
    }
}
