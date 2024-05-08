using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This SO is used to balance spells, value can be modified by skills
/// </summary>
public class Data_Spell : ScriptableObject
{
    [SerializeField] private string _spellName;

    [SerializeField] private SpellType _spellType;

    [SerializeField, ReadOnly] protected SpellBehavior _spellBehavior;

    /*[SerializeField] private int _damage;*/

    #region ACCESSORS
    public string SpellName { get => _spellName; set => _spellName = value; }
    public SpellType CurrentSpellType { get => _spellType; set => _spellType = value; }
    public SpellBehavior CurrentSpellBehavior { get => _spellBehavior; set => _spellBehavior = value; }
    #endregion

    public enum SpellType
    {
        OFFENSIVE, DEFENSIVE, CONTROL
    }

    public enum SpellBehavior
    {
        PROJECTILE
    }
}
