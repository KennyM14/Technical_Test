using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey = false;


    void Start()
    {
        UIManager.Instance.ShowKeySlot(hasKey);
    }
    public void PickUpKey()
    {
        hasKey = true;
        UIManager.Instance.ShowKeySlot(true);
    }

    public void UseKey()
    {
        hasKey = false;
        UIManager.Instance.ShowKeySlot(false);
    }
}
