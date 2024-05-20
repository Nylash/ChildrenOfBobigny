using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private Image _HPBarFill;
    [SerializeField] private GameObject _HPBar;
    [SerializeField] private WeaponEvents _weaponEventsScript;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    #endregion

    #region VARIABLES
    private float _HP;
    private List<GameObject> _hitUnits = new List<GameObject>();
    private RoomBehavior _associatedRoom;
    private Transform _target;
    #region ACCESSORS
    public RoomBehavior AssociatedRoom { get => _associatedRoom; set => _associatedRoom = value; }
    public Transform Target { get => _target; set => _target = value; }
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

    private void Update()
    {
        if (_target)
        {
            _navMeshAgent.SetDestination(_target.position);
        }
    }

    private void LateUpdate()
    {
        _HPBar.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
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
        _associatedRoom.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
