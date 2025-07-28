using UnityEngine;

public class ShowUI : MonoBehaviour
{
    public GameObject interactionUI;
    private bool isPlayerNear = false;

    void Start()
    {
        interactionUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayerNear)
        {
            isPlayerNear = true; 
            interactionUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayerNear)
        {
            isPlayerNear = false; 
            interactionUI.SetActive(false);
        }
    }

    void OnDestroy()
    {
        
    }
}
