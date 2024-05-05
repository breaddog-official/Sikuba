using System.Collections;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using Scripts.InputManagement;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float wallrunSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float vaultSpeed;
    [SerializeField] private float airMinSpeed;

    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    [Space(5.0f)]
    [SerializeField] private float speedIncreaseMultiplier;
    [SerializeField] private float slopeIncreaseMultiplier;
    [Space(5.0f)]
    [SerializeField] private float groundDrag;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;

    bool readyToJump;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchYScale;

    private float startYScale;

    [Header("Keybinds (depricated)")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;

    public bool Grounded { get; private set; }

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;

    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("References")]
    [SerializeField] private Transform orientation;

    Vector2 movementVector;

    Vector3 moveDirection;

    Climbing climbingScript;
    Rigidbody rb;

    [ReadOnly] public MovementState state;
    public enum MovementState
    {
        freeze,
        unlimited,
        walking,
        sprinting,
        wallrunning,
        climbing,
        vaulting,
        crouching,
        sliding,
        air
    }

    public bool Sliding { get; set; }
    public bool Crouching { get; set; }
    public bool Wallrunning { get; set; }
    public bool Climbing { get; set; }
    public bool Vaulting { get; set; }

    public bool Freeze { get; set; }
    public bool Unlimited { get; set; }

    public bool Restricted { get; set; }

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI text_speed;
    [SerializeField] private TextMeshProUGUI text_mode;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        climbingScript = GetComponent<Climbing>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        // Grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        UpdateInput();
        SpeedControl();
        StateHandler();
        TextStuff();

        // handle drag
        if (Grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate() => MovePlayer();
    private void OnCollisionEnter(Collision collision) => Grounded = collision.GetContact(0).normal == -transform.up;

    #region Input
    private void OnEnable()
    {
        InputManager.Controls.Game.Jump.started += PerformJump;

        InputManager.Controls.Game.Crouch.started += StartCrouch;
        InputManager.Controls.Game.Crouch.canceled += StopCrouch;
    }
    private void OnDisable()
    {
        InputManager.Controls.Game.Jump.started -= PerformJump;

        InputManager.Controls.Game.Crouch.started -= StartCrouch;
        InputManager.Controls.Game.Crouch.canceled -= StopCrouch;
    }
    private void PerformJump(InputAction.CallbackContext ctx = default)
    {
        if (readyToJump && Grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void StartCrouch(InputAction.CallbackContext ctx = default)
    {
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        Crouching = true;
    }
    private void StopCrouch(InputAction.CallbackContext ctx = default)
    {
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        Crouching = false;
    }
    private void UpdateInput()
    {
        movementVector = InputManager.Controls.Game.Move.ReadValue<Vector2>();
    }
    #endregion

    bool keepMomentum;
    private void StateHandler()
    {
        // Mode - Freeze
        if (Freeze)
        {
            state = MovementState.freeze;
            rb.velocity = Vector3.zero;
            desiredMoveSpeed = 0f;
        }

        // Mode - Unlimited
        else if (Unlimited)
        {
            state = MovementState.unlimited;
            desiredMoveSpeed = 1024f;
        }

        // Mode - Vaulting
        else if (Vaulting)
        {
            state = MovementState.vaulting;
            desiredMoveSpeed = vaultSpeed;
        }

        // Mode - Climbing
        else if (Climbing)
        {
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }

        // Mode - Wallrunning
        else if (Wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }

        // Mode - Sliding
        else if (Sliding)
        {
            state = MovementState.sliding;

            // increase speed by one every second
            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
                keepMomentum = true;
            }

            else
                desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Crouching
        else if (Crouching)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (Grounded && InputManager.Controls.Game.Sprint.IsPressed())
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (Grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;

            if (moveSpeed < airMinSpeed)
                desiredMoveSpeed = airMinSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopCoroutine(SmoothlyLerpMoveSpeed());
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

        // deactivate keepMomentum
        if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.1f) keepMomentum = false;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        if (climbingScript.exitingWall) return;
        if (Restricted) return;

        // calculate movement direction
        moveDirection = orientation.forward * movementVector.y + orientation.right * movementVector.x;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(20f * moveSpeed * GetSlopeMoveDirection(moveDirection), ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (Grounded)
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);

        // in air
        else if (!Grounded)
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);

        // turn gravity off while on slope
        if (!Wallrunning) rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void TextStuff()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (text_speed != null)
        {
            if (OnSlope())
                text_speed.SetText("Speed: " + Round(rb.velocity.magnitude, 1) + " / " + Round(moveSpeed, 1));

            else
                text_speed.SetText("Speed: " + Round(flatVel.magnitude, 1) + " / " + Round(moveSpeed, 1));
        }
        else if (text_mode != null)
        {
            text_mode.SetText(state.ToString());
        }
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
