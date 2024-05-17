using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private Image _HPBarFill;
    [SerializeField] private GameObject _HPBar;
    [SerializeField] private WeaponEvents _weaponEventsScript;
    #endregion

    #region VARIABLES
    private float _HP;
    private List<GameObject> _hitUnits = new List<GameObject>();
    #region ACCESSORS

    #endregion
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private Data_BasicEnemy _enemyData;
    #endregion

    private void OnEnable()
    {
        _weaponEventsScript.event_weaponHitsSomething.AddListener(WeaponHitSomething);
    }
    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;//Avoid null ref
        _weaponEventsScript.event_weaponHitsSomething.RemoveListener(WeaponHitSomething);
    }

    private void Start()
    {
        _HP = _enemyData.MaxHP;
        UpdateHPBar();

        InvokeRepeating("ResetHitUnits", 0, 3);
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

    //TODO Remove this and clear the list at the end of the attack animation
    private void ResetHitUnits()
    {
        _hitUnits.Clear();
    }

    private void WeaponHitSomething(Collider other)
    {
        if (!_hitUnits.Contains(other.gameObject))
        {
            if (other.CompareTag("Player"))
            {
                //Once player is hit we add it to the list so we avoid hitting him twice with one attack
                _hitUnits.Add(other.gameObject);
                try
                {
                    PlayerHealthManager.Instance.TakeDamage(_enemyData.AttackDamage);
                }
                catch (System.Exception)
                {
                    Debug.LogError("Enemy weapon hit Player which doesn't have PlayerHealthManager script : " + other.gameObject.name);
                }
                return;
            }
            Debug.LogError("Enemy weapon hit something not handled : " + other.gameObject.name);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
