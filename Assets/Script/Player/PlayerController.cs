using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
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
    [SerializeField] private Transform firePoint;
    [SerializeField] private float aimDistance = 50f;
    private bool ignoreHeight = false;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private PlayerInput playerInput;
    private Camera mainCamera;
    private Weapon weapon;
    private bool isShooting;
    private bool rotationAligned = false;
    private Vector3 movement;
    


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
        HandleMovement();
        HandleFootsteps();
        Aim(); 
    }
    

    void FixedUpdate()
    {
        Vector3 targetVelocity = movement * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y; 
        
        Vector3 velocityChange = (targetVelocity - rb.linearVelocity);
        velocityChange.y = 0; // La velocidad vertical no cambia
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        
    }

    private void HandleMovement()
    {
        movement = new Vector3(inputVector.x, 0, inputVector.y);
        bool isMoving = movement.magnitude > 0.1f;
        anim.SetBool("Running", isMoving);
    }

    private void HandleFootsteps()
    {
        bool isMoving = movement.magnitude > 0.1f;

        if (isMoving)
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

        if (ignoreHeight)
        {
            direction.y = 0;
        }

        if (direction.sqrMagnitude < 0.01f) return;

        if (isShooting)
        {
            // Rotar al jugador hacia el mouse
            graphicsTransform.rotation = Quaternion.Slerp(graphicsTransform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 15f);

            // Rotar el arma hacia el frente (apuntado)
            Gun.transform.localRotation = Quaternion.RotateTowards(Gun.transform.localRotation, Quaternion.Euler(aimingRotation), Time.deltaTime * 300f);

            // Comprobar si está alineada para disparar
            if (!rotationAligned && IsGunAligned())
            {
                rotationAligned = true;
                weapon?.SetShooting(true);
            }

            // Line Renderer
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
                graphicsTransform.rotation = Quaternion.Slerp(graphicsTransform.rotation, targetRotation, Time.deltaTime * 10f);
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

    // ignora la diferencia de altura (eje Y)
    public void OnChangeTargetMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ignoreHeight = !ignoreHeight;
            Debug.Log("Modo de apuntado cambiado. Ignorar altura: " + ignoreHeight);
        }
    }

    private bool IsGunAligned()
    {
        return Quaternion.Angle(Gun.transform.localRotation, Quaternion.Euler(aimingRotation)) < 1f;
    }
}