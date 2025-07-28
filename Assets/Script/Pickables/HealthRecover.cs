using UnityEngine;

public class HealthRecover : MonoBehaviour, IPickable
{
    [SerializeField] private int healAmount = 20;
    [SerializeField] private AudioClip pickupSound;

    public void Pick(PlayerController playerController, PlayerHealth playerHealth, Weapon weapon)
    {
        if (playerHealth != null && playerHealth.CanHeal())
        {
            playerHealth.Heal(healAmount);
            
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }
            
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Health is full or player health component missing");
        }
    }

}
