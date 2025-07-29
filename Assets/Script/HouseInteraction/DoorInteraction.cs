using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private KeyColor requiredKey;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    private PlayerInventory playerInventory;
    private bool isUnlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInventory = other.GetComponent<PlayerInventory>();
        if (isUnlocked)
        {
            OpenDoor();
        }
        else if (playerInventory != null && playerInventory.HasKey(requiredKey))
        {
            playerInventory.UseKey(requiredKey);
            isUnlocked = true;
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isUnlocked)
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        doorAnimator.SetBool("Open", true);

        if (openSound != null)
        {
            AudioSource.PlayClipAtPoint(openSound, transform.position);
        }
    }

    private void CloseDoor()
    {
        doorAnimator.SetBool("Open", false); 

        if (closeSound != null)
        {
            AudioSource.PlayClipAtPoint(closeSound, transform.position);
        }
    }

}
