using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private int currentHealth;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject healVFX;
    [SerializeField] private Transform hitPoint;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining HP: {currentHealth}");

        if (hitVFX != null)
        {
            Instantiate(hitVFX, hitPoint.position, Quaternion.identity); 
        }

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

        if (healVFX != null)
        {
            Instantiate(healVFX, hitPoint.position, Quaternion.identity); 
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
        GetComponent<PlayerController>().enabled = false;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        GameManager.Instance?.PlayerLoses();

        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        Debug.Log("PLAYER DEAD!");
        Destroy(gameObject, 2f);
        
    }
    
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
}
