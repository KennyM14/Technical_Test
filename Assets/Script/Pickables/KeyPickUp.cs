using UnityEngine;

public class KeyPickUp : MonoBehaviour, IPickable
{
    public void Pick(PlayerController playerController, PlayerHealth playerHealth)
    {
        PlayerInventory inventory = playerController.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.PickUpKey();
            Destroy(gameObject); // Eliminar la llave después de recogerla
        }
    }
}