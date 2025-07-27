using UnityEngine;
using System.Collections; 

public class HideRoof : MonoBehaviour
{
    [Header("Objetos a desvanecer")]
    public Renderer roofRenderer;
    public Renderer chimneyRenderer;

    public float fadeDuration = 0.5f;
    private Material roofMatInstance;
    private Material chimneyMatInstance;
    private Coroutine fadeRoutine;

    private void Start()
    {
        roofMatInstance = roofRenderer.material;
        chimneyMatInstance = chimneyRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            roofRenderer.gameObject.SetActive(true);
            chimneyRenderer.gameObject.SetActive(true);

            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeOut());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color roofStart = roofMatInstance.color;
        Color chimneyStart = chimneyMatInstance.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            roofMatInstance.color = new Color(roofStart.r, roofStart.g, roofStart.b, alpha);
            chimneyMatInstance.color = new Color(chimneyStart.r, chimneyStart.g, chimneyStart.b, alpha);
            yield return null;
        }

        // Desativar los objectos si se prefiere
        roofRenderer.gameObject.SetActive(false);
        chimneyRenderer.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        roofRenderer.gameObject.SetActive(true);
        chimneyRenderer.gameObject.SetActive(true);

        float elapsed = 0f;
        Color roofStart = roofMatInstance.color;
        Color chimneyStart = chimneyMatInstance.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            roofMatInstance.color = new Color(roofStart.r, roofStart.g, roofStart.b, alpha);
            chimneyMatInstance.color = new Color(chimneyStart.r, chimneyStart.g, chimneyStart.b, alpha);
            yield return null;
        }

        roofMatInstance.color = new Color(roofStart.r, roofStart.g, roofStart.b, 1f);
        chimneyMatInstance.color = new Color(chimneyStart.r, chimneyStart.g, chimneyStart.b, 1f);
    }
}
