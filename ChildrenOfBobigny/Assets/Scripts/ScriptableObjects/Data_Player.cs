using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This SO is use for getting data but also store some, to avoid references between objects
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Player Data")]
public class Data_Player : ScriptableObject
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
    [Tooltip("At 1 attack speed is 1 attack per second.")]
    [SerializeField][Range(0.5f, 3f)] private float _attackSpeed = 1f;
    [SerializeField] private int _attackDamage = 10;
    #endregion

    #region SPELLS VARIABLES
    [SerializeField] private Data_Spell _offensiveSpell;
    [SerializeField] private Data_Spell _defensiveSpell;
    [SerializeField] private Data_Spell _controlSpell;
    #endregion

    #region UI VARIABLES
    [Header("UI")]
    [SerializeField] private Color _colorDashInCD;
    [SerializeField] private Color _colorDashReady;
    #endregion

    #region RUNTIME VARIABLES
    [Header("RUNTIME")]
    private bool _dashIsReady = true;
    #endregion

    #region ACCESSORS
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
            event_dashAvailabilityUpdated.Invoke(_dashIsReady);
        }  
    }

    public float AttackSpeed { get => _attackSpeed; 
        set 
        {
            _attackSpeed = value;
            event_attackSpeedUpdated.Invoke(_attackSpeed);
        }  
    }

    public int AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public Data_Spell OffensiveSpell { get => _offensiveSpell; set => _offensiveSpell = value; }
    public Data_Spell DefensiveSpell { get => _defensiveSpell; set => _defensiveSpell = value; }
    public Data_Spell ControlSpell { get => _controlSpell; set => _controlSpell = value; }
    #endregion

    #region EVENTS
    public UnityEvent<bool> event_dashAvailabilityUpdated;
    public UnityEvent<float> event_attackSpeedUpdated;
    #endregion

    private void OnEnable()
    {
        if (event_dashAvailabilityUpdated == null)
            event_dashAvailabilityUpdated = new UnityEvent<bool>();
        if (event_attackSpeedUpdated == null)
            event_attackSpeedUpdated = new UnityEvent<float>();
    }
}
