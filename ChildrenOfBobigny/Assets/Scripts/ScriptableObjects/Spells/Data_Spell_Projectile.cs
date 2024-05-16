using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Projectile Spell Data")]
public class Data_Spell_Projectile : Data_Spell
{
    #region CONFIG
    [Header("PROJECTILE VARIABLES")]
    [SerializeField] private GameObject _projectileObject;
    [SerializeField] private float _speed;
    [SerializeField] private float _range;

    [Header("DAMAGE")]
    [SerializeField] private float _damage;
    #endregion

    #region ACCESSORS
    public GameObject ProjectileObject { get => _projectileObject; }
    public float Speed { get => _speed; }
    public float Range { get => _range; }
    public float Damage { get => _damage; }
    #endregion

    private void OnEnable()
    {
        _spellBehavior = SpellBehavior.PROJECTILE;
    }
}
