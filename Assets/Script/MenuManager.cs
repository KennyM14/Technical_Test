using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void Start()
    {
        creditsPanel.SetActive(false);
    }

    public void OnCreditsButton()
    {
        creditsPanel.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OnBackToMenu()
    {
        creditsPanel.SetActive(false);
    }

    public void StartGameScene()
    {
        audioSource.Stop();
        PlayerPrefs.SetInt("ShowInstructionsOnLoad", 1);
        SceneManager.LoadScene("MainScene");
    }
}
