using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    public Image image;
    public float fadeDuration = 1f;  // Duration of each fade (in seconds)
    public float delayBetweenFades = 1f;  // Delay between each fade (in seconds)

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = image.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = image.gameObject.AddComponent<CanvasGroup>();
        }
        StartFade();
    }

    private void StartFade()
    {
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        // Fade In
        yield return Fade(0f, 1f);

        // Delay between fades
        yield return new WaitForSeconds(delayBetweenFades);

        // Fade Out
        yield return Fade(1f, 0f);

        // Restart the fade loop
        StartFade();
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        float currentAlpha = startAlpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            canvasGroup.alpha = currentAlpha;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
