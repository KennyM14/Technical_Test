using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject keySlot;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowKeySlot(bool show)
    {
        keySlot.SetActive(show); 
    }
}
