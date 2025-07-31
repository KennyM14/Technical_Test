using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject deathVFX;

    //Play when it´s destroyed
    [SerializeField] private AudioClip destroyClip;

    //When he received damage
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private Transform hitPoint;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (hitVFX != null)
        {
            Instantiate(hitVFX, hitPoint.position, Quaternion.identity); 
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        GetComponent<EnemyAI>()?.DisableEnemy();

        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        }

        if (destroyClip != null)
        {
            AudioSource.PlayClipAtPoint(destroyClip, transform.position);
        }

        EnemyManager.Instance?.EnemyDestroyed(); 
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
}
