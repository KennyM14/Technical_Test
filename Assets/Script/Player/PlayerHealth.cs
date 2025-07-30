using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private AudioClip healSound;
    private int currentHealth;
    [SerializeField] private GameOverManager gameOverManager; 

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
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

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        // Mostrar Game Over, reiniciar escena, etc.
        if (gameOverManager != null)
        {
            gameOverManager.TriggerGameOver(); 
        }
        Debug.Log("PLAYER DEAD!");
        Destroy(gameObject); // o aplicar lógica adicional
    }
}
