using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovementManager : MonoBehaviour
{
    public static PlayerMovementManager instance;

    #region COMPONENTS
    private ControlsMap controlsMap;
    private CharacterController controller;
    [SerializeField] private Animator animator;
    #endregion

    #region VARIABLES
    private MovementState currentMovementState = MovementState.Idling;
    private Vector2 movementDirection;
    private Vector2 dashDirection;
    private bool isDashReady = true;

    public MovementState CurrentMovementState { get => currentMovementState; }
    public Vector2 DashDirection { get => dashDirection; }
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float dashSpeed = 50;
    [SerializeField] private float dashCD = 3;
    [SerializeField] private float dashDuration = .5f;
    [SerializeField] private float gravityForce = 9.81f;
    #endregion

    private void OnEnable() => controlsMap.Gameplay.Enable();
    private void OnDisable() => controlsMap.Gameplay.Disable();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        controlsMap = new ControlsMap();

        controlsMap.Gameplay.Movement.performed += ctx => ReadMovementDirection(ctx);
        controlsMap.Gameplay.Movement.canceled += ctx => StopReadMovementDirection();
        controlsMap.Gameplay.Dash.performed += ctx => StartCoroutine(Dashing());

        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        switch (currentMovementState)
        {
            case MovementState.Moving:
                if(controller.isGrounded)
                    controller.Move(new Vector3(movementDirection.x, 0, movementDirection.y) * movementSpeed * Time.deltaTime);
                else
                    controller.Move(new Vector3(movementDirection.x, -gravityForce, movementDirection.y) * movementSpeed * Time.deltaTime);
                animator.SetFloat("Angle", Mathf.Abs(Vector2.Angle(movementDirection, PlayerAimManager.instance.AimDirection)));
                break;
            case MovementState.Dashing:
                break;
            case MovementState.Idling:
                break;
            default:
                Debug.LogError("No reason to be there !");
                break;
        }
    }

    private IEnumerator Dashing()
    {
        if (isDashReady)
        {
            animator.SetBool("Dashing", true);
            currentMovementState = MovementState.Dashing;
            isDashReady = false;
            if(movementDirection != Vector2.zero)
                dashDirection = movementDirection;
            float startTime = Time.time;
            while( Time.time < startTime + dashDuration) 
            {
                controller.Move(new Vector3(dashDirection.x, 0, dashDirection.y) * dashSpeed * Time.deltaTime);
                yield return null;
            }
            animator.SetBool("Dashing", false);
            currentMovementState = MovementState.Moving;
            yield return new WaitForSeconds(dashCD - dashDuration);
            isDashReady = true;
        }
    }

    private void ReadMovementDirection(InputAction.CallbackContext ctx)
    {
        movementDirection = ctx.ReadValue<Vector2>().normalized;
        currentMovementState = MovementState.Moving;
        animator.SetBool("Running", true);
    }

    private void StopReadMovementDirection()
    {
        dashDirection = movementDirection;
        movementDirection = Vector2.zero;
        currentMovementState = MovementState.Idling;
        animator.SetBool("Running", false);
    }

    public enum MovementState
    {
        Idling, Moving, Dashing
    }
}
