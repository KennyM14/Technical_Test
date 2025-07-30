using UnityEngine;
using DG.Tweening;

public class UIWarning : MonoBehaviour
{
    [SerializeField] private CanvasGroup healWarningCanvasGroup;
    [SerializeField] private CanvasGroup ammoWarningCanvasGroup;

    public void ShowHealWarning()
    {
        healWarningCanvasGroup.alpha = 0f;
        healWarningCanvasGroup.gameObject.SetActive(true);

        // Fade in
        healWarningCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
        {
            // Esperar 2 segundos y hase desaparece con un fade out
            DOVirtual.DelayedCall(2f, () =>
            {
                healWarningCanvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    healWarningCanvasGroup.gameObject.SetActive(false);
                });
            });
        });
    }
    
    public void ShowAmmoWarning()
    {
        ammoWarningCanvasGroup.alpha = 0f;
        ammoWarningCanvasGroup.gameObject.SetActive(true);

        // Fade in
        ammoWarningCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
        {
            // Esperar 2 segundos y hase desaparece con un fade out
            DOVirtual.DelayedCall(2f, () =>
            {
                ammoWarningCanvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    ammoWarningCanvasGroup.gameObject.SetActive(false);
                });
            });
        });
    }
}
