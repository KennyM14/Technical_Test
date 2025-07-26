using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask pickableLayer;
    private IPickable currentPickable;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Interact.performed += ctx => TryInteract();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        CheckForPickables();
    }

        private void TryInteract()
    {
        if (currentPickable != null)
        {
            currentPickable.OnPickUp(gameObject);
            currentPickable = null;
        }
    }
    
    private void CheckForPickables()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, interactRange, pickableLayer))
        {
            currentPickable = hit.collider.GetComponent<IPickable>();
        }
        else
        {
            currentPickable = null;
        }
    }
}
