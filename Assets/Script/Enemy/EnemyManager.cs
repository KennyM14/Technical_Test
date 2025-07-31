using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    [SerializeField] private int totalEnemies = 3; // Total de enemigos en escena
    private int enemiesDefeated = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Llamar cada vez que un enemigo sea destruido
    public void EnemyDestroyed()
    {
        enemiesDefeated++;
        
        if (enemiesDefeated >= totalEnemies)
        {
            GameManager.Instance.PlayerWins();
        }
    }
}
