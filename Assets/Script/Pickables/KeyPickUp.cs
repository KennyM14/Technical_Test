using UnityEngine;

public class KeyPickUp : MonoBehaviour, IPickable
{
    [SerializeField] private KeyColor keyColor;
    [SerializeField] private AudioClip pickupSound;

    public void Pick(PlayerController playerController, PlayerHealth playerHealth, Weapon weapon)
    {
        PlayerInventory inventory = playerController.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.PickUpKey(keyColor);

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            gameObject.SetActive(false);
        }
    }
}
