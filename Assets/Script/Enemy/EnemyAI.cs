using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] private Animator anim;

    //Firepoint 
    [SerializeField] private Transform firePoint; 
    [SerializeField] private AudioClip shootClip;
    private AudioSource audioSource;

    //Patroling
    [SerializeField] private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    //Attacking
    [SerializeField] private float timeBetweenAttack;
    private bool alreadyAttacked;
    [SerializeField] private GameObject enemyGun;

    //States
    [SerializeField] private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInSightRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            anim.SetBool("Running", true);
            agent.SetDestination(walkPoint);
        }

        float distanceToWalkPoint = Vector3.Distance(transform.position, walkPoint);

        //if reached
        if (distanceToWalkPoint < 1f)
        {
            walkPointSet = false;
            anim.SetBool("Running", false); 
        }
    }

    private void SearchWalkPoint()
    {

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 potentialPoint = transform.position + new Vector3(randomX, 0, randomZ);

        if (Physics.Raycast(potentialPoint, Vector3.down, 2f, whatIsGround))
        {
            walkPoint = potentialPoint;
            walkPointSet = true; 
        }
    }

    private void ChasePlayer()
    {
        anim.SetBool("Running", true);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        anim.SetBool("Running", false);
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (enemyGun != null)
        {
            enemyGun.transform.LookAt(player); 
        }

        if (!alreadyAttacked)
        {
            //Attack method Here
            anim.SetBool("Shoot", true);
            FireBullet(firePoint, gameObject);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void FireBullet(Transform firePoint, GameObject enemyObject)
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        //Evitar collisionar con las balas del player
        bullet.layer = LayerMask.NameToLayer("EnemyBullet");

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * 15f; // Velocidad deseada

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Initialize(enemyObject);

        if (shootClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootClip);
        }

        StartCoroutine(ReturnAfterTime(bullet, 4f)); // O la duración que quieras
    }

    private IEnumerator ReturnAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        BulletPool.Instance.ReturnBullet(bullet);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
