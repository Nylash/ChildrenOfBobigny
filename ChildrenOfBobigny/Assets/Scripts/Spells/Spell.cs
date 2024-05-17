using System;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    protected bool _initDone;
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
    public virtual void Init(Data_Spell_Shield data)
    {
        throw new NotImplementedException();
    }

    protected void SetLayers(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
