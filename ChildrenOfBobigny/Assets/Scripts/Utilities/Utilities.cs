using UnityEngine;

public static class Utilities 
{
    #region METHODS
    public static System.Type GetSpellBehaviorType(Data_Spell spell)
    {
        //Condensed switch
        return spell.CurrentSpellBehavior switch
        {
            SpellBehavior.PROJECTILE => typeof(ProjectileSpell),
            SpellBehavior.SHIELD => typeof(ShieldSpell),
            _ => null,
        };
    }

    public static void SetChildsLayers(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
    #endregion

    #region ENUMS
    public enum PlayerBehaviorState
    {
        IDLE, MOVE, DASH, ATTACK, CAST
    }

    public enum SpellType
    {
        OFFENSIVE, DEFENSIVE, CONTROL
    }

    public enum SpellBehavior
    {
        PROJECTILE, SHIELD
    }
    #endregion
}
