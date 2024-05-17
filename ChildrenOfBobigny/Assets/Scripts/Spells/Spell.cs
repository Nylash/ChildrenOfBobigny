using System;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    #region COMMON_VARIABLES
    protected bool _initDone;
    private Vector3 _direction;
    #endregion

    public Vector3 Direction { get => _direction; set => _direction = value; }

    //One Init method per Spell behavior, called with keyword "dynamic"

    public virtual void Init(Data_Spell data)
    {
        throw new NotImplementedException();
    }
    public virtual void Init(Data_Spell_Projectile data)
    {
        throw new NotImplementedException();
    }
    public virtual void Init(Data_Spell_Shield data)
    {
        throw new NotImplementedException();
    }
}
