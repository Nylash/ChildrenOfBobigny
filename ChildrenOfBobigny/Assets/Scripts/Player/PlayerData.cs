using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Data")]
public class PlayerData : ScriptableObject
{
    #region MOVEMENT VARIABLES
    [Header("MOVEMENT")]
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _rotationSpeed = .075f;
    [SerializeField] private float _gravityForce = 9.81f;
    #endregion

    #region DASH VARIABLES
    [Header("DASH")]
    [SerializeField] private float _dashSpeed = 20;
    [SerializeField] private float _dashCD = 1;
    [SerializeField] private float _dashDuration = .3f;
    #endregion

    #region UI VARIABLES
    [Header("UI")]
    [SerializeField] private Color _colorDashInCD;
    [SerializeField] private Color _colorDashReady;
    #endregion

    #region RUNTIME VARIABLES
    [Header("RUNTIME")]
    private bool _dashIsReady;
    #endregion

    #region ACCESSEURS
    public float DashSpeed { get => _dashSpeed; set => _dashSpeed = value; }
    public float DashCD { get => _dashCD; set => _dashCD = value; }
    public float DashDuration { get => _dashDuration; set => _dashDuration = value; }
    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
    public float GravityForce { get => _gravityForce; set => _gravityForce = value; }
    public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
    public Color ColorDashInCD { get => _colorDashInCD; set => _colorDashInCD = value; }
    public Color ColorDashReady { get => _colorDashReady; set => _colorDashReady = value; }
    public bool DashIsReady { get => _dashIsReady; 
        set 
        {
            _dashIsReady = value;
            dashIsReadyEvent.Invoke(_dashIsReady);
        }  
    }
    #endregion

    #region EVENTS
    public UnityEvent<bool> dashIsReadyEvent;
    #endregion

    private void OnEnable()
    {
        if (dashIsReadyEvent == null)
            dashIsReadyEvent = new UnityEvent<bool>();
    }
}
