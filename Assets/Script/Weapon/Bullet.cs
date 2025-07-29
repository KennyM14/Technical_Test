using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private GameObject owner;

    public void Initialize(GameObject owner)
    {
        this.owner = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner) return; // No dañar al que disparó

        // Si el dueño es el jugador y golpea a un enemigo
        if (owner.CompareTag("Player") && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                BulletPool.Instance.ReturnBullet(gameObject);
            }
        }
        // Si el dueño es un enemigo y golpea al jugador
        else if (owner.CompareTag("Enemy") && other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                BulletPool.Instance.ReturnBullet(gameObject);
            }
        }
    }
}
