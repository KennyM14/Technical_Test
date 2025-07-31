using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private PlayerHealth playerHealth; 
    
    void Update()
    {
        if (playerHealth != null)
        {
            float fillAmount = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;
            fillImage.fillAmount = fillAmount; 

            healthText.text = playerHealth.CurrentHealth.ToString();
        }
    }
}
