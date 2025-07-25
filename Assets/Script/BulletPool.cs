using UnityEngine;
using UnityEngine.Pool; 

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }

    [SerializeField] private GameObject bulletObj;
    [SerializeField] private int initialSize = 40;
    [SerializeField] private int maxSize = 100;
    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
            Destroy(gameObject);

        pool = new ObjectPool<GameObject>(
            CreateBullet,
            OnTakeBullet,
            OnReleaseBullet,
            OnDestroyBullet,
            true, initialSize, maxSize
        );
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(bulletObj);
        bullet.SetActive(false);
        return bullet;
    }

    private void OnTakeBullet(GameObject bullet)
    {
        bullet.SetActive(true);
    }

    private void OnReleaseBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    private void OnDestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }

    public GameObject GetBullet()
    {
        return pool.Get();
    }

    public void ReturnBullet(GameObject bullet)
    {
        if (!bullet.activeSelf) return;
        pool.Release(bullet);
    }
}
