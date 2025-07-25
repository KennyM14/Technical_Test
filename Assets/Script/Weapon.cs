using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
 /*   [SerializeField] private Animator anim;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;

    private float fireCooldown;
    private Camera mainCamera;
    private bool isShooting;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnShootInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isShooting = true;
        }
        else if (ctx.canceled)
        {
            isShooting = false;
        }
    }

    void Update()
    {
        RotateTowardsMouse();

        if (isShooting && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }
        fireCooldown -= Time.deltaTime;
    }

    void RotateTowardsMouse()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition(mainCamera);
        Vector3 direction = mouseWorldPos - transform.position;
        direction.z = 0;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = rotation;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = firePoint.up * 10f; // Ajusta la velocidad
        anim.SetTrigger("Shoot"); 
    }

    Vector3 GetMouseWorldPosition(Camera cam)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        worldPos.z = 0;
        return worldPos;
    } */
}
