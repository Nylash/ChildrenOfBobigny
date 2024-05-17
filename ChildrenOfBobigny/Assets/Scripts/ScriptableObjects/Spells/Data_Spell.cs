using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This SO is used to balance spells, value can be modified by skills
/// </summary>
public abstract class Data_Spell : ScriptableObject
{
    #region CONFIGURATION
    [Header("GLOBAL INFO")]
    [SerializeField] private string _spellName;
    [SerializeField] private SpellType _spellType;
    [SerializeField, ReadOnly] protected SpellBehavior _spellBehavior;

    [Header("CONSTRAINTS")]
    [SerializeField] private float _CD;
    [SerializeField] private float _manaCost;
    #endregion

    #region ACCESSORS
    public string SpellName { get => _spellName; }
    public SpellType CurrentSpellType { get => _spellType; }
    public SpellBehavior CurrentSpellBehavior { get => _spellBehavior; }
    public float CD { get => _CD; }
    public float ManaCost { get => _manaCost; }
    #endregion

    public enum SpellType
    {
        OFFENSIVE, DEFENSIVE, CONTROL
    }

    public enum SpellBehavior
    {
        PROJECTILE, SHIELD
    }
}
