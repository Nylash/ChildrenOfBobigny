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

    //Called right before init is finished to have the right layer on every children
    protected void SetLayers(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
