using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private AudioClip healSound;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        if (healSound != null)
        {
            AudioSource.PlayClipAtPoint(healSound, transform.position);
        }
        
        Debug.Log("Current HP: " + currentHealth);
    }

    public bool CanHeal()
    {
        return currentHealth < maxHealth;
    }

    // Para mostrar en UI
    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
    
}
