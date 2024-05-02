using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Data")]
public class PlayerData : ScriptableObject
{
    #region MOVEMENT VARIABLES
    [Header("MOVEMENT")]
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField][Range(0, 1)] private float _rotationSpeed = .075f;
    [SerializeField] private float _gravityForce = 9.81f;
    #endregion

    #region DASH VARIABLES
    [Header("DASH")]
    [SerializeField] private float _dashSpeed = 20;
    [SerializeField] private float _dashCD = 1;
    [SerializeField] private float _dashDuration = .3f;
    #endregion

    #region ATTACK VARIABLES
    [Tooltip("At 1 attack speed is attack animation speed.")]
    [SerializeField][Range(0.5f, 3f)] private float _attackSpeed = 1f;
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

    public float AttackSpeed { get => _attackSpeed; 
        set 
        {
            _attackSpeed = value;
            attackSpeedEvent.Invoke(_attackSpeed);
        }  
    }
    #endregion

    #region EVENTS
    public UnityEvent<bool> dashIsReadyEvent;
    public UnityEvent<float> attackSpeedEvent;
    #endregion

    private void OnEnable()
    {
        if (dashIsReadyEvent == null)
            dashIsReadyEvent = new UnityEvent<bool>();
        if (attackSpeedEvent == null)
            attackSpeedEvent = new UnityEvent<float>();
    }
}
