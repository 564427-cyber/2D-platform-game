using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;

    private void Awake()
    {
        if (fadeImage == null)
            fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color c = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }
}
