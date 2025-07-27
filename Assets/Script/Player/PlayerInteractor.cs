using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask pickableLayer;
    private IPickable currentPickable;


    private void Update()
    {
        CheckForPickables();
    }

    public void Pick(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (currentPickable != null)
        {
            Debug.Log("Recogiendo objeto: " + currentPickable);
            currentPickable.OnPickUp(gameObject);
            currentPickable = null;
        }
        else
        {
            Debug.Log("No hay objeto recogible al frente");
        }
    }
    
    private void CheckForPickables()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, pickableLayer))
        {
            Debug.Log("Detectado objeto: " + hit.collider.name);
            currentPickable = hit.collider.GetComponent<IPickable>();
        }
        else
        {
            currentPickable = null;
        }
    }
}
