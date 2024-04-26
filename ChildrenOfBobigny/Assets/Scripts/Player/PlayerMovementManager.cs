using System.Collections;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    public static PlayerMovementManager instance;

    #region COMPONENTS
    private ControlsMap controlsMap;
    private CharacterController controller;
    #endregion

    #region VARIABLES
    private MovementState currentMovementState = MovementState.Moving;
    private Vector2 movementDirection;
    private Vector2 dashDirection;
    private bool isDashReady = true;
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

        controlsMap.Gameplay.Movement.performed += ctx => movementDirection = ctx.ReadValue<Vector2>();
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
                break;
            case MovementState.Dashing:
                controller.Move(new Vector3(dashDirection.x, 0, dashDirection.y) * dashSpeed * Time.deltaTime);
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
            currentMovementState = MovementState.Dashing;
            isDashReady = false;
            if(movementDirection != Vector2.zero)
                dashDirection = movementDirection;
            yield return new WaitForSeconds(dashDuration);
            currentMovementState = MovementState.Moving;
            yield return new WaitForSeconds(dashCD - dashDuration);
            isDashReady = true;
        }
    }

    private void StopReadMovementDirection()
    {
        dashDirection = movementDirection;
        movementDirection = Vector2.zero;
    }

    private enum MovementState
    {
        Moving, Dashing
    }
}
