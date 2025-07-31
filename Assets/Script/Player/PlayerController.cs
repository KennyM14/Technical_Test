using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private float gravity = -9.81f;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform graphicsTransform;
    [SerializeField] private LayerMask aimLayerMask;
    [SerializeField] private Vector3 aimingRotation = new Vector3(0, 0, 0);

    [Header("Weapon")]
    [SerializeField] private GameObject Gun;
    private Vector2 inputVector;
    private Vector2 mouseScreenPos;
    private Quaternion initialGunRotation;

    [Header("Sound")]
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip footstepClip;

    [Header("Line Renderer")]
    [SerializeField] private LineRenderer aimLineRenderer;
    [SerializeField] private Transform firePoint; // Donde empieza el disparo
    [SerializeField] private float aimDistance = 50f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private PlayerInput playerInput;
    private Camera mainCamera;
    private Weapon weapon;
    private bool isShooting;
    private bool rotationAligned = false;
    private Vector3 movement;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();
        weapon = Gun.GetComponentInChildren<Weapon>();
        initialGunRotation = Gun.transform.localRotation;
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;

        playerInput.actions["Look"].performed += ctx => mouseScreenPos = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => mouseScreenPos = ctx.ReadValue<Vector2>();

        playerInput.actions["Shoot"].performed += OnShoot;
        playerInput.actions["Shoot"].canceled += OnStopShoot;
    }

    void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;

        playerInput.actions["Look"].performed -= ctx => mouseScreenPos = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled -= ctx => mouseScreenPos = ctx.ReadValue<Vector2>();

        playerInput.actions["Shoot"].performed -= OnShoot;
        playerInput.actions["Shoot"].canceled -= OnStopShoot;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        inputVector = ctx.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        isShooting = true;
        anim.SetBool("Shoot", true);
        rotationAligned = false; 
    }

    void OnStopShoot(InputAction.CallbackContext ctx)
    {
        isShooting = false;
        anim.SetBool("Shoot", false);
        Gun.transform.localRotation = initialGunRotation;
        weapon?.SetShooting(false);
    }

    void Update()
    {
        movement = new Vector3(inputVector.x, 0, inputVector.y);
        bool isMoving = movement.magnitude > 0.1f;
        anim.SetBool("Running", isMoving);

        if (isMoving && isGrounded)
        {
            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Play();
            }
        }
        else
        {
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Pause();
            }
        }

        Aim(); 
    }
    

    void FixedUpdate()
    {
        Vector3 targetVelocity = movement * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y; 
        
        Vector3 velocityChange = (targetVelocity - rb.linearVelocity);
        velocityChange.y = 0; // La velocidad vertical no cambia
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        
        // Aplicar gravedad personalizada
        if (!isGrounded)
        {
            rb.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
        }
    }

    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (!success)
        {
            aimLineRenderer.enabled = false;
            return;
        }

        Vector3 direction = position - firePoint.position;
        if (direction.sqrMagnitude < 0.01f) return;

        if (isShooting)
        {
            // Rotar al jugador hacia el mouse
            graphicsTransform.rotation = Quaternion.Slerp(
                graphicsTransform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * 15f
            );

            // Rotar el arma hacia el frente (apuntado)
            Gun.transform.localRotation = Quaternion.RotateTowards(
                Gun.transform.localRotation,
                Quaternion.Euler(aimingRotation),
                Time.deltaTime * 300f
            );

            // Comprobar si está alineada para disparar
            if (!rotationAligned && IsGunAligned())
            {
                rotationAligned = true;
                weapon?.SetShooting(true);
            }

            // Línea de apuntado
            aimLineRenderer.enabled = true;
            aimLineRenderer.SetPosition(0, firePoint.position);
            aimLineRenderer.SetPosition(1, firePoint.position + firePoint.forward * aimDistance);
        }
        else
        {
            aimLineRenderer.enabled = false;

            // Solo rotar al moverse si no se está disparando
            if (movement.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                graphicsTransform.rotation = Quaternion.Slerp(
                    graphicsTransform.rotation,
                    targetRotation,
                    Time.deltaTime * 10f
                );
            }
        }

    }
    

    private (bool success, Vector3 position) GetMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimLayerMask))
        {
            return (true, hit.point);
        }
        return (false, Vector3.zero);
    }

    void OnCollisionStay(Collision collision)
    {
        // Verificar si está en el suelo
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.7f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private bool IsGunAligned()
    {
        return Quaternion.Angle(Gun.transform.localRotation, Quaternion.Euler(aimingRotation)) < 1f;
    }
}