using UnityEngine;

public class KeyPickUp : MonoBehaviour, IPickable
{
    public void OnPickUp(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.PickUpKey();
            Destroy(gameObject);
        }
    }
}
