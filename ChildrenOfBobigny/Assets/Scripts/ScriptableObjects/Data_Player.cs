using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This SO is use for getting data but also store some, to avoid references between objects
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Player Data")]
public class Data_Player : ScriptableObject
{
    #region RESSOURCES
    [Header("Ressources")]
    [SerializeField] private float _maxHP = 100;
    [SerializeField] private float _maxMP = 100;
    #endregion

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
    [Header("ATTACK")]
    [Tooltip("At 1 attack speed is 1 attack per second.")]
    [SerializeField][Range(0.5f, 3f)] private float _attackSpeed = 1f;
    [SerializeField] private float _attackDamage = 10;
    #endregion

    #region SPELLS VARIABLES
    [Header("SPELLS")]
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
    //Be sure to reset those variables at the start of the game if needed
    [Header("RUNTIME")]
    private bool _dashIsReady;
    private float _currentHP;
    private float _currentMP;
    private bool _offensiveSpellInCD;
    private bool _defensiveSpellInCD;
    private bool _controlSpellInCD;
    #endregion

    #region ACCESSORS
    public float DashSpeed { get => _dashSpeed; }
    public float DashCD { get => _dashCD; }
    public float DashDuration { get => _dashDuration; }
    public float MovementSpeed { get => _movementSpeed; }
    public float GravityForce { get => _gravityForce; }
    public float RotationSpeed { get => _rotationSpeed; }
    public Color ColorDashInCD { get => _colorDashInCD; }
    public Color ColorDashReady { get => _colorDashReady; }
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
    public float AttackDamage { get => _attackDamage; }
    public Data_Spell OffensiveSpell { get => _offensiveSpell; set => _offensiveSpell = value; }
    public Data_Spell DefensiveSpell { get => _defensiveSpell; set => _defensiveSpell = value; }
    public Data_Spell ControlSpell { get => _controlSpell; set => _controlSpell = value; }
    public float CurrentHP { get => _currentHP; 
        set
        {
            _currentHP = value;
            event_currentHPUpdated.Invoke(_currentHP);
        }
    }
    public float CurrentMP
    {
        get => _currentMP;
        set
        {
            _currentMP = value;
            event_currentMPUpdated.Invoke(_currentMP);
        }
    }
    public float MaxHP { get => _maxHP; set => _maxHP = value; }
    public float MaxMP { get => _maxMP; set => _maxMP = value; }
    public bool OffensiveSpellInCD
    {
        get => _offensiveSpellInCD; 
        set
        {
            _offensiveSpellInCD = value;
            event_offensiveSpellCDUpdated.Invoke(_offensiveSpellInCD);
        }
    }
    public bool DefensiveSpellInCD
    {
        get => _defensiveSpellInCD;
        set
        {
            _defensiveSpellInCD = value;
            event_defensiveSpellCDUpdated.Invoke(_defensiveSpellInCD);
        }
    }
    public bool ControlSpellInCD
    {
        get => _controlSpellInCD;
        set
        {
            _controlSpellInCD = value;
            event_controlSpellCDUpdated.Invoke(_controlSpellInCD);
        }
    }
    #endregion

    #region EVENTS
    [HideInInspector] public UnityEvent<bool> event_dashAvailabilityUpdated;
    [HideInInspector] public UnityEvent<float> event_attackSpeedUpdated;
    [HideInInspector] public UnityEvent<float> event_currentHPUpdated;
    [HideInInspector] public UnityEvent<float> event_currentMPUpdated;
    [HideInInspector] public UnityEvent<bool> event_offensiveSpellCDUpdated;
    [HideInInspector] public UnityEvent<bool> event_defensiveSpellCDUpdated;
    [HideInInspector] public UnityEvent<bool> event_controlSpellCDUpdated;
    #endregion

    private void OnEnable()
    {
        if (event_dashAvailabilityUpdated == null)
            event_dashAvailabilityUpdated = new UnityEvent<bool>();
        if (event_attackSpeedUpdated == null)
            event_attackSpeedUpdated = new UnityEvent<float>();
        if (event_currentHPUpdated == null)
            event_currentHPUpdated = new UnityEvent<float>();
        if (event_currentMPUpdated == null)
            event_currentMPUpdated = new UnityEvent<float>();
        if (event_offensiveSpellCDUpdated == null)
            event_offensiveSpellCDUpdated = new UnityEvent<bool>();
        if (event_defensiveSpellCDUpdated == null)
            event_defensiveSpellCDUpdated = new UnityEvent<bool>();
        if (event_controlSpellCDUpdated == null)
            event_controlSpellCDUpdated = new UnityEvent<bool>();
    }
}
