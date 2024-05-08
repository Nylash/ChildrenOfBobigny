using System;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public Data_Spell _spellData;

    public virtual void Init(Data_Spell data)
    {
        throw new Exception("Not implemented yet.");
    }
    public virtual void Init(Data_Spell_Projectile data)
    {
        throw new Exception("Not implemented yet.");
    }
}
