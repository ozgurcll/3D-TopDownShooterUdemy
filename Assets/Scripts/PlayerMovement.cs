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
    [SerializeField] private float turnSpeed;

    private float speed;
    private float verticalVelocity;

    public Vector2 moveInput { get; private set; }
    private Vector3 movementDirection;

    private bool isRunning;



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
        ApplyRotation();
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
    private void ApplyRotation()
    {
        Vector3 lookingDirection = player.aim.GetMouseHitInfo().point - transform.position;
        lookingDirection.y = 0;
        lookingDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
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
