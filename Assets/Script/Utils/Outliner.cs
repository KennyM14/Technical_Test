using UnityEngine;

public class Outliner : MonoBehaviour
{
    public GameObject interactionUI;
    public Material outlineMaterial;
    private MeshRenderer objectRenderer;
    private Material[] originalMaterials;
    private Material[] outlinedMaterials;
    private bool isPlayerNear = false;

    void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>();
        originalMaterials = objectRenderer.materials;
        
        // material original + outline
        outlinedMaterials = new Material[originalMaterials.Length + 1];
        originalMaterials.CopyTo(outlinedMaterials, 0);
        outlinedMaterials[originalMaterials.Length] = outlineMaterial;
        
        interactionUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayerNear)
        {
            isPlayerNear = true; 
            
            // Aplicar materiales con outline
            objectRenderer.materials = outlinedMaterials;
            
            interactionUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayerNear)
        {
            isPlayerNear = false; 
            
            // Restaurar materiales originales
            objectRenderer.materials = originalMaterials;
            
            interactionUI.SetActive(false);
        }
    }

    void OnDestroy()
    {
        // Limpiar materiales
        if (outlinedMaterials != null)
        {
            for (int i = originalMaterials.Length; i < outlinedMaterials.Length; i++)
            {
                if (outlinedMaterials[i] != null)
                {
                    Destroy(outlinedMaterials[i]);
                }
            }
        }
    }
}