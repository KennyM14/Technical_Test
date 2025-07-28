using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private KeyColor requiredKey;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioClip openSound;
    private bool isPlayerNearby = false;
    private PlayerInventory playerInventory;


    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null && playerInventory.HasKey(requiredKey))
            {
                doorAnimator.SetTrigger("Open");

                if (openSound != null)
                {
                    AudioSource.PlayClipAtPoint(openSound, transform.position);
                }

                playerInventory.UseKey(requiredKey);
                // Puedes desactivar el collider si quieres que no se cierre nunca
                GetComponent<Collider>().enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerInventory = null;
        }
    }
}
