using System;
using UnityEngine;

public class PlayerHealthManager : Singleton<PlayerHealthManager>
{
    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private Data_Player _playerData;
    #endregion

    public void TakeDamage(float damage)
    {
        throw new NotImplementedException();
    }
}
