using BehaviorTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class BasicEnemy_BT : BehaviorTree.Tree
{
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
    [Header("CONFIGURATION")]
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

    protected override void Update()
    {
        base.Update();
    }

    protected virtual void LateUpdate()
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
