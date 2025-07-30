using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private GameObject owner;

    public void Initialize(GameObject owner)
    {
        this.owner = owner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        if (other == owner) return;

        if (collision.gameObject == owner) return; // No dañar al que disparó

        // Si el que disparó es el jugador entonces golpea a un enemigo
        if (owner.CompareTag("Player") && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("Impacto bala recibido..."); 
                enemyHealth.TakeDamage(damage);
            }
        }

        // Si el que disparó es un enemigo entonces golpea al jugador
        else if (owner.CompareTag("Enemy") && other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        BulletPool.Instance.ReturnBullet(gameObject);
    }
}
