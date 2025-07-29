using UnityEngine;
using System.Collections;

public class CutoutObject : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody rb;
    public float holeSize = 0.1f; 

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(rb.transform.position, 5f);
        foreach (var hitCollider in hitColliders)
        {
            float x = 0f;

            if (Vector3.Distance(hitCollider.transform.position, mainCamera.transform.position) < Vector3.Distance(rb.centerOfMass + rb.transform.position, mainCamera.transform.position))
            {
                x = holeSize;
            }

            try
            {
                Material[] materials = hitCollider.transform.GetComponent<Renderer>().materials;
                for (int m = 0; m < materials.Length; ++m)
                {
                    materials[m].SetFloat("_Step", x); 
                }
            }
            catch
            {

            }
        }
    }

}
