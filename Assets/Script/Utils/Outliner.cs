using UnityEngine;

public class Outliner : MonoBehaviour
{
    public GameObject interactionUI;
    public Material outlineMaterial;
    private MeshRenderer objectRenderer;
    private Material[] originalMaterials;
    private bool isPlayerNear = false;

    void Start()
    {
        objectRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        originalMaterials = objectRenderer.materials;
        interactionUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayerNear)
        {
            isPlayerNear = true; 

            Material[] newMats = new Material[originalMaterials.Length + 1];
            originalMaterials.CopyTo(newMats, 0);
            newMats[newMats.Length - 1] = outlineMaterial;

            objectRenderer.materials = newMats;
            interactionUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayerNear)
        {
            isPlayerNear = false; 
            objectRenderer.materials = originalMaterials;
            interactionUI.SetActive(false);
        }
    }
}
