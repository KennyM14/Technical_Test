using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    public GameObject[] instructionPanels;
    private int currentIndex = 0;
    private const string FirstTimeKey = "HasSeenInstructions";

    void Start()
    {
        bool shouldShow = PlayerPrefs.GetInt("ShowInstructionsOnLoad", 0) == 1;
        bool hasSeenInstructions = PlayerPrefs.HasKey(FirstTimeKey);

        if (shouldShow && !hasSeenInstructions)
        {
            Time.timeScale = 0f;
            ShowCurrentPanel();
        }
        else
        {
            gameObject.SetActive(false);
        }   
        
        PlayerPrefs.DeleteKey("ShowInstructionsOnLoad");
    }
    
    public void NextPanel()
    {
        instructionPanels[currentIndex].SetActive(false);
        currentIndex++;

        if (currentIndex < instructionPanels.Length)
        {
            ShowCurrentPanel();
        }
        else
        {
            Time.timeScale = 1f;
            PlayerPrefs.SetInt(FirstTimeKey, 1);
            gameObject.SetActive(false); 
        }
    }

    private void ShowCurrentPanel()
    {
        for (int i = 0; i < instructionPanels.Length; i++)
        {
            instructionPanels[i].SetActive(false);
        }

        instructionPanels[currentIndex].SetActive(true);
    }

}
