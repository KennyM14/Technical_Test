using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement & Aiming")]
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

    private CharacterController controller;
    private PlayerInput playerInput;
    private Camera mainCamera;
    private Weapon weapon;
    private bool isShooting;
    private bool rotationAligned = false;
    

    void Awake()
    {
        controller = GetComponent<CharacterController>();
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

    void OnMove(InputAction.CallbackContext ctx)
    {
        inputVector = ctx.ReadValue<Vector2>();
    }

    void OnShoot(InputAction.CallbackContext ctx)
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
        Vector3 move = new Vector3(inputVector.x, 0, inputVector.y);
        bool isMoving = move.magnitude > 0.1f;

        controller.Move(move * moveSpeed * Time.deltaTime);
        anim.SetBool("Running", isMoving);

        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimLayerMask))
        {
            Vector3 direction = hit.point - graphicsTransform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                if (isShooting)
                {
                    graphicsTransform.rotation = Quaternion.Slerp(graphicsTransform.rotation, lookRotation, Time.deltaTime * 15f);
                    Gun.transform.localRotation = Quaternion.RotateTowards(Gun.transform.localRotation, Quaternion.Euler(aimingRotation), Time.deltaTime * 300f);

                    if (!rotationAligned && IsGunAligned())
                    {
                        rotationAligned = true;
                        weapon?.SetShooting(true);
                    }
                }
                else if (isMoving)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(move);
                    graphicsTransform.rotation = Quaternion.Slerp(graphicsTransform.rotation, targetRotation, Time.deltaTime * 10f);
                }
            }
        }
    }

    private bool IsGunAligned()
    {
        float angle = Quaternion.Angle(Gun.transform.localRotation, Quaternion.Euler(aimingRotation));
        return angle < 1f; 
    }
}
