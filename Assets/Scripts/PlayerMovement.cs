using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private PlayerControllers controls;
    private CharacterController characterController;
    private Animator anim;
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float speed;
    private Vector3 movementDirection;
    private float verticalVelocity;
    private bool isRunning;

    [Header("Aiming")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookingDirection;


    private Vector2 moveInput;
    private Vector2 aimInput;

    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        speed = walkSpeed;
        AssignInputEvents();
    }
    private void Update()
    {
        ApplyMovement();
        AimTowardsMouse();
        AnimatorController();
    }

    private void AnimatorController()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        anim.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        anim.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

        bool playRunAnim = isRunning && movementDirection.magnitude > 0;
        anim.SetBool("isRunning", playRunAnim);
    }

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();
        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * speed * Time.deltaTime);
        }
    }
    private void AimTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lookingDirection = hitInfo.point;
            lookingDirection.y = 0;
            lookingDirection.Normalize();

            transform.forward = lookingDirection;

            aim.position = new Vector3(hitInfo.point.x, transform.position.y + 1, hitInfo.point.z);
        }
    }
    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity -= 9.81f * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
            verticalVelocity = -.5f;
    }
    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Player.Movement.canceled += context => moveInput = Vector2.zero;

        controls.Player.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Player.Aim.canceled += context => aimInput = Vector2.zero;

        controls.Player.Run.performed += context =>
        {
            isRunning = true;
            speed = runSpeed;
        };
        controls.Player.Run.canceled += context =>
        {
            isRunning = false;
            speed = walkSpeed;
        };
    }

}
