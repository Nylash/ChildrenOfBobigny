using UnityEngine;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private Image _HPBarFill;
    [SerializeField] private GameObject _HPBar;
    #endregion

    #region VARIABLES
    private float _HP;
    #region ACCESSORS

    #endregion
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private Data_BasicEnemy _enemyData;
    #endregion

    private void Start()
    {
        _HP = _enemyData.MaxHP;
        UpdateHPBar();
    }

    public void TakeDamage(float damage)
    {
        _HP -= damage;
        UpdateHPBar();
        if(_HP <= 0)
        {
            Die();
        }
    }

    private void UpdateHPBar()
    {
        _HPBarFill.fillAmount = _HP / _enemyData.MaxHP;
        if (_HPBarFill.fillAmount < 1)
            _HPBar.SetActive(true);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
