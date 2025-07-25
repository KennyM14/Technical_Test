using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator anim;
    private Vector2 inputVector;
    private CharacterController controller;
    private PlayerInput playerInput;
    private bool isShooting;

    [Header("Weapon")]
    [SerializeField] private GameObject Gun;
    private Quaternion initialGunRotation;
    [SerializeField] private Vector3 aimingRotation = new Vector3(0, 0, 0);
    private Weapon weapon;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        weapon = Gun.GetComponentInChildren<Weapon>();
    }

    void Start()
    {
        initialGunRotation = Gun.transform.localRotation;
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += Move;
        playerInput.actions["Move"].canceled += Move;
        playerInput.actions["Shoot"].performed += Shoot;
        playerInput.actions["Shoot"].canceled += StopShoot;
    }

    /*private void OnDisable()
    {
        playerInput.actions["Move"].performed -= Move;
        playerInput.actions["Move"].canceled -= Move;
        playerInput.actions["Shoot"].performed -= Shoot;
        playerInput.actions["Shoot"].canceled -= StopShoot;
    } */

    public void Move(InputAction.CallbackContext ctx)
    {
        inputVector = ctx.ReadValue<Vector2>();
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        isShooting = true;
        anim.SetBool("Shooting", true);
        Gun.transform.localRotation = Quaternion.Euler(aimingRotation);
        weapon?.SetShooting(true); 
    }

    public void StopShoot(InputAction.CallbackContext ctx)
    {
        isShooting = false;
        anim.SetBool("Shooting", false);
        Gun.transform.localRotation = initialGunRotation;
        weapon?.SetShooting(false); 
    }

    void Update()
    {
        Vector3 move = new Vector3(inputVector.x, 0, inputVector.y);
        bool isMoving = move.magnitude > 0.1f;

        if (isMoving)
        {
            Quaternion rotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 15f);
        }
        controller.Move(move * moveSpeed * Time.deltaTime);
        anim.SetBool("Running", isMoving);

    }
}
