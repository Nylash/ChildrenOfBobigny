using System;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public virtual void Init(Data_Spell data)
    {
        throw new NotImplementedException();
    }
    public virtual void Init(Data_Spell_Projectile data)
    {
        throw new NotImplementedException();
    }
}
