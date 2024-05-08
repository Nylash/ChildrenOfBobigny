using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Projectile Spell Data")]
public class Data_Spell_Projectile : Data_Spell
{
    private void OnEnable()
    {
        _spellBehavior = SpellBehavior.PROJECTILE;
    }
}
