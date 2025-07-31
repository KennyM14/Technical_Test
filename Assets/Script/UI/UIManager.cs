using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Key Icons")]
    public GameObject yellowKeyIcon;
    public GameObject redKeyIcon;
    public GameObject blueKeyIcon;
    public GameObject WeaponIcon;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // evitar duplicados
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        WeaponIcon.SetActive(true); 
    }

    public void ShowKey(KeyColor keyColor, bool show)
    {
        switch (keyColor)
        {
            case KeyColor.Yellow:
                yellowKeyIcon.SetActive(show);
                break;
            case KeyColor.Red:
                redKeyIcon.SetActive(show);
                break;
            case KeyColor.Blue:
                blueKeyIcon.SetActive(show);
                break;
        }
    }
}
