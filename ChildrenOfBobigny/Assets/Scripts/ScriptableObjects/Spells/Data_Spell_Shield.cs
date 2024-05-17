using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Shield Spell Data")]
public class Data_Spell_Shield : Data_Spell
{
    #region CONFIG
    [Header("SHIELD VARIABLES")]
    [SerializeField] private GameObject _shieldObject;
    [SerializeField] private float _lifetime;
    [SerializeField] private Vector3 _positionOffset;

    [Header("LAYER CONFIGURATION")]
    [SerializeField, Layer] private int _playerSpellLayer;
    [SerializeField, Layer] private int _playerShieldLayer;
    [SerializeField, Layer] private int _enemySpellLayer;
    [SerializeField, Layer] private int _enemyShieldLayer;
    #endregion

    #region ACCESSORS
    public GameObject ShieldObject { get => _shieldObject; }
    public float Lifetime { get => _lifetime; }
    public Vector3 PositionOffset { get => _positionOffset; }
    public int PlayerSpellLayer { get => _playerSpellLayer; }
    public int PlayerShieldLayer { get => _playerShieldLayer; }
    public int EnemySpellLayer { get => _enemySpellLayer; }
    public int EnemyShieldLayer { get => _enemyShieldLayer; }
    #endregion

    private void OnEnable()
    {
        _spellBehavior = SpellBehavior.SHIELD;
    }
}
