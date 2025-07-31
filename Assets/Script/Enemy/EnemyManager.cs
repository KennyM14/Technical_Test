using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    [SerializeField] private int totalEnemies = 3; // Total de enemigos en escena para destruir 
    private int enemiesDefeated = 0; //Enemigos destuidos

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI enemyCountText;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EnemyDestroyed()  // Llamar cada vez que un enemigo sea destruido
    {
        enemiesDefeated++;
        UpdateCount(); 

        if (enemiesDefeated >= totalEnemies)
        {
            GameManager.Instance.PlayerWins();
        }
    }

    private void UpdateCount()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"{enemiesDefeated} / {totalEnemies}"; 
        }
    }
}
