using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private HashSet<KeyColor> keys = new HashSet<KeyColor>();

    void Start()
    {
        // ocultar todo al inicio
        foreach (KeyColor color in System.Enum.GetValues(typeof(KeyColor)))
        {
            UIManager.Instance.ShowKey(color, false);
        }
    }

    public void PickUpKey(KeyColor color)
    {
        keys.Add(color);
        UIManager.Instance.ShowKey(color, true);
    }

    public bool HasKey(KeyColor color)
    {
        return keys.Contains(color);
    }

    public void UseKey(KeyColor color)
    {
        if (keys.Contains(color))
        {
            keys.Remove(color);
            UIManager.Instance.ShowKey(color, false);
        }
    }
}
