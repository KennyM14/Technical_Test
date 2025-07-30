using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private EnemyHealth enemyHealth;

    void Update()
    {
        if (enemyHealth != null)
        {
            float fillAmount = (float)enemyHealth.CurrentHealth / enemyHealth.MaxHealth;
            fillImage.fillAmount = fillAmount; 

            healthText.text = enemyHealth.CurrentHealth.ToString();
        }
    }
}
