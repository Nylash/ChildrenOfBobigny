using BehaviorTree;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasicEnemy_BT : BehaviorTree.Tree
{
    [Header("BASE")]
    #region COMPONENTS
    [SerializeField] private Image _HPBarFill;
    [SerializeField] private GameObject _HPBar;
    #endregion

    #region VARIABLES
    private float _HP;
    private RoomBehavior _associatedRoom;
    #region ACCESSORS
    public RoomBehavior AssociatedRoom { get => _associatedRoom; set => _associatedRoom = value; }
    #endregion
    #endregion

    #region CONFIGURATION
    [SerializeField] protected Data_BasicEnemy _enemyData;
    #endregion

    protected override Node SetupTree()
    {
        throw new NotImplementedException();
    }

    protected override void Start()
    {
        base.Start();

        _HP = _enemyData.MaxHP;
        UpdateHPBar();
    }

    protected void LateUpdate()
    {
        //Make HP bar always face the camera
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


    private void Die()
    {
        _associatedRoom.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public void SetTarget(Transform target)
    {
        _root.SetData("Target", target);
    }
}
