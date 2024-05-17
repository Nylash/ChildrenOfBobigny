using UnityEngine;

public class PlayerHealthManager : Singleton<PlayerHealthManager>
{
    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private Data_Player _playerData;
    #endregion

    private void Start()
    {
        _playerData.CurrentHP = _playerData.MaxHP;
    }

    public void TakeDamage(float damage)
    {
        _playerData.CurrentHP -= damage;
        if( _playerData.CurrentHP < 0)
        {
            _playerData.CurrentHP = 0;
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        Destroy(gameObject);
    }
}
