using UnityEngine;

public class Ammo : MonoBehaviour, IPickable
{
    [SerializeField] private int ammoAmount = 30;
    [SerializeField] private AudioClip pickupSound;

    public void Pick(PlayerController playerController, PlayerHealth playerHealth, Weapon weapon)
    {
        if (weapon != null && weapon.NeedsAmmo())
        {
            weapon.addAmmo(ammoAmount);
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Weapon doesn't need ammo right now");
        }
    }
}
