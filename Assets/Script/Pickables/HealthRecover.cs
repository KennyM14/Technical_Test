using UnityEngine;
using UnityEngine.UI; 

public class HealthRecover : MonoBehaviour, IPickable
{
    [SerializeField] private int healAmount = 20;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private GameObject healVFX;

    [Header("UI")]
    [SerializeField] private UIWarning uIWarning; 

    public void Pick(PlayerController playerController, PlayerHealth playerHealth, Weapon weapon)
    {
        if (playerHealth != null && playerHealth.CanHeal())
        {
            playerHealth.Heal(healAmount);

            if (healSound != null)
            {
                AudioSource.PlayClipAtPoint(healSound, transform.position);
            }

            if (healVFX != null)
            {
                GameObject vfx = Instantiate(healVFX, playerController.transform.position, Quaternion.identity);
                Destroy(vfx, 1.5f);
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Health is full or player health component missing");
            if (uIWarning != null)
            {
                uIWarning.ShowHealWarning(); 
            }
        }
    }

}
