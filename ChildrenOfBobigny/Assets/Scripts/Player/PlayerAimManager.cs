using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimManager : MonoBehaviour
{
    public static PlayerAimManager instance;

    #region COMPONENTS
    [Header("COMPONENTS")]
    private ControlsMap controlsMap;
    [SerializeField] private Animator animatorOrientation;
    private PlayerInput playerInput;
    #endregion

    #region VARIABLES
    private Vector2 stickDirection;
    private Vector2 mouseDirection;
    private Vector2 aimDirection;

    public Vector2 AimDirection { get => aimDirection; }
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField][Range(0, 1)] private float rotationSpeed = .075f;
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

        controlsMap.Gameplay.AimStick.performed += ctx => stickDirection = ctx.ReadValue<Vector2>();
        controlsMap.Gameplay.AimMouse.performed += ctx => mouseDirection = ctx.ReadValue<Vector2>();

        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if(PlayerMovementManager.instance.CurrentMovementState != PlayerMovementManager.MovementState.Dashing)
        {
            if (playerInput.currentControlScheme == "Keyboard")
            {
                Ray ray = Camera.main.ScreenPointToRay(mouseDirection);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    Vector3 target = ray.GetPoint(distance) - transform.position;
                    aimDirection = new Vector2(target.x, target.z).normalized;
                }
            }
            else
            {
                aimDirection = stickDirection;
            }

            animatorOrientation.SetFloat("InputX", Mathf.MoveTowards(animatorOrientation.GetFloat("InputX"), aimDirection.x, rotationSpeed));
            animatorOrientation.SetFloat("InputY", Mathf.MoveTowards(animatorOrientation.GetFloat("InputY"), aimDirection.y, rotationSpeed));
        }
        else
        {
            animatorOrientation.SetFloat("InputX", PlayerMovementManager.instance.DashDirection.x);
            animatorOrientation.SetFloat("InputY", PlayerMovementManager.instance.DashDirection.y);
        }
    }
}
