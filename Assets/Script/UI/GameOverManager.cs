using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup youDiedCanvasGroup;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject quitButton;

    [Header("Fade Settings")]
    [SerializeField] private float youDiedFadeDuration = 1.5f;
    [SerializeField] private float panelFadeDelay = 1f;
    [SerializeField] private float panelFadeDuration = 1.5f;

    private void Start()
    {
        // Ocultar al inicio
        youDiedCanvasGroup.alpha = 0;
        gameOverCanvasGroup.alpha = 0;
        gameOverCanvasGroup.interactable = false;
        gameOverCanvasGroup.blocksRaycasts = false;

        retryButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void TriggerGameOver()
    {
        // Secuencia de efectos
        Sequence gameOverSequence = DOTween.Sequence();

        gameOverSequence.Append(youDiedCanvasGroup.DOFade(1f, youDiedFadeDuration).SetEase(Ease.OutQuad))
            .AppendInterval(panelFadeDelay)
            .Append(gameOverCanvasGroup.DOFade(1f, panelFadeDuration).SetEase(Ease.OutSine))
            .AppendCallback(() =>
            {
                gameOverCanvasGroup.interactable = true;
                gameOverCanvasGroup.blocksRaycasts = true;
                retryButton.SetActive(true);
                quitButton.SetActive(true);
            });
    }
}
