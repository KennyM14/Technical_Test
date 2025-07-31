using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Victory panel
    [SerializeField] private CanvasGroup victoryPanel;
    [SerializeField] private float victoryFadeDuration = 1f;

    //Gama over panel
    [SerializeField] private CanvasGroup youDiedText;
    [SerializeField] private CanvasGroup gameOverPanel;
    [SerializeField] private float gameOverFadeDuration = 0.8f;

    //Pause Menu
    [SerializeField] private GameObject pausePanel;
    private bool isPaused = false;
    private bool isGameEnded = false;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DOTween.Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeUI();
        pausePanel.SetActive(false); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(); 
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            PauseGameAudio();
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            ResumeGameAudio(); 
        }
    }

    public void ResumenGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        ResumeGameAudio(); 
        pausePanel.SetActive(false);
    }

    private void InitializeUI()
    {

        SetCanvasGroup(victoryPanel, 0, false, false);
        SetCanvasGroup(youDiedText, 0, false, false);
        SetCanvasGroup(gameOverPanel, 0, false, false);
    }

    // Llamar cuando el jugador gana
    public void PlayerWins()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        ShowVictoryPanel();
        PauseGameAudio();
        Time.timeScale = 0f;
    }

    //Llamar cuando el jugador pierde
    public void PlayerLoses()
    {
        Debug.Log("Player is already dead");
        if (isGameEnded) return;
        isGameEnded = true;

        ShowGameOverSequence();
        PauseGameAudio();
        Time.timeScale = 0f;
    }

    //Detener sonidos
    private void PauseGameAudio()
    {
        AudioListener.pause = true;

        var playerAudio = FindObjectOfType<PlayerController>()?.GetComponent<AudioSource>();
        if (playerAudio != null) playerAudio.Stop();
    }

    private void ShowVictoryPanel()
    {
        victoryPanel.gameObject.SetActive(true);
        victoryPanel.DOFade(1f, victoryFadeDuration)
            .SetUpdate(true)
            .OnComplete(() => SetCanvasGroup(victoryPanel, 1, true, true));
    }

    private void ShowGameOverSequence()
    {
        youDiedText.gameObject.SetActive(true);
        youDiedText.alpha = 0;

        Sequence seq = DOTween.Sequence();
        seq.Append(youDiedText.DOFade(1f, gameOverFadeDuration).SetUpdate(true));
        seq.AppendCallback(() =>
        {
            gameOverPanel.gameObject.SetActive(true);
            gameOverPanel.alpha = 0;
            gameOverPanel.DOFade(1f, gameOverFadeDuration).SetUpdate(true).OnComplete(() =>
                {
                    gameOverPanel.interactable = true;
                    gameOverPanel.blocksRaycasts = true;
                });
        });
        seq.SetUpdate(true);
    }

    //Volver a intentar 
    public void RetryGame()
    {
        Time.timeScale = 1f;
        ResumeGameAudio();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    //Activar sonido
    private void ResumeGameAudio()
    {
        AudioListener.pause = false;
    }

    //Salir del juego
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetCanvasGroup(CanvasGroup group, float alpha, bool interactable, bool blocksRaycasts)
    {
        if (group == null) return;

        group.alpha = alpha;
        group.interactable = interactable;
        group.blocksRaycasts = blocksRaycasts;
    }
    
    //Volver al menú principal
    public void BackHome()
    {
        PlayerPrefs.DeleteKey("HasSeenInstructions");
        SceneManager.LoadScene("Menu");
    }
}