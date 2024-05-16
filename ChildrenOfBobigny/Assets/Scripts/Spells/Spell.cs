using System;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    private Vector3 _direction;

    public Vector3 Direction { get => _direction; set => _direction = value; }

    public virtual void Init(Data_Spell data)
    {
        throw new NotImplementedException();
    }
    public virtual void Init(Data_Spell_Projectile data)
    {
        throw new NotImplementedException();
    }
}
