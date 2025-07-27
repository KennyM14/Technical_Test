using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask interactableLayers;
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private List<IPickable> nearbyPickables = new List<IPickable>();

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayers) != 0)
        {
            IPickable pickable = other.GetComponent<IPickable>();
            if (pickable != null && !nearbyPickables.Contains(pickable))
            {
                nearbyPickables.Add(pickable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayers) != 0)
        {
            IPickable pickable = other.GetComponent<IPickable>();
            if (pickable != null && nearbyPickables.Contains(pickable))
            {
                nearbyPickables.Remove(pickable);
            }
        }
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || nearbyPickables.Count == 0) return;
        IPickable closest = GetClosestPickable();
        if (closest != null)
        {
            closest.Pick(playerController, playerHealth);
            nearbyPickables.Remove(closest);
        }
    }

    private IPickable GetClosestPickable()
    {
        IPickable closest = null;
        float minDistance = Mathf.Infinity;

        foreach (IPickable pickable in nearbyPickables)
        {
            float distance = Vector3.Distance(transform.position, ((MonoBehaviour)pickable).transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = pickable;
            }
        }

        return closest;
    }
}
