using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;

    [Header("Bullet Settings")]
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float bulletLifeTime = 4f;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private int maxMagazineSize = 30;
    [SerializeField] private int currentBullets = 30;
    [SerializeField] private int totalBullets = 120;
    [SerializeField] private float reloadTime = 2f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Audio")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioSource audioSource;
    
    private bool isShooting = false;
    private bool isReloading = false;
    private float nextFireTime;

    public void OnReload(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isReloading && currentBullets < maxMagazineSize && totalBullets > 0)
        {
            StartCoroutine(Reload());
        }
    }

    void Update()
    {
        if (isShooting && !isReloading && currentBullets > 0 && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }

        if (currentBullets <= 0 && !isReloading && totalBullets > 0)
        {
            StartCoroutine(Reload());
        }
    }

    public void SetShooting(bool state) => isShooting = state;

    private void Fire()
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(firePoint.forward);

        //Evitar collision con las balas disparadas del enemigo
        bullet.layer = LayerMask.NameToLayer("PlayerBullet");

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Initialize(transform.root.gameObject); // El arma es hija del Player

        StartCoroutine(ReleaseAfterTime(bullet, bulletLifeTime));

        currentBullets--;
        UpdateAmmoUI();

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private IEnumerator ReleaseAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        BulletPool.Instance.ReturnBullet(bullet);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
        yield return new WaitForSeconds(reloadTime);

        int bulletsNeeded = maxMagazineSize - currentBullets;
        int bulletsToReload = Mathf.Min(bulletsNeeded, totalBullets);
        currentBullets += bulletsToReload;
        totalBullets -= bulletsToReload;

        UpdateAmmoUI();
        isReloading = false;
    }

    public void ManualReload()
    {
        if (!isReloading && currentBullets < maxMagazineSize && totalBullets > 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentBullets} / {totalBullets}";
        }
    }

    public void AddAmmo(int amount)
    {
        totalBullets += amount;
        UpdateAmmoUI();
    }

    public bool NeedsAmmo() => totalBullets < maxMagazineSize * 4;
}
