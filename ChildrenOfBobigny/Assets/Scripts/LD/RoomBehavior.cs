using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    #region VARIABLES
    [Header("VARIABLES")]
    [SerializeField, ReadOnly] private List<BasicEnemy_BT> _associatedEnemies = new List<BasicEnemy_BT>();
    [SerializeField, ReadOnly] private bool _roomCleared;
    #endregion

    #region ACCESSORS
    public bool RoomCleared { get => _roomCleared; set => _roomCleared = value; }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (BasicEnemy_BT enemy in _associatedEnemies)
            {
                enemy.SetTarget(other.transform);
            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(other.gameObject.GetComponent<BasicEnemy_BT>().AssociatedRoom == null) 
            {
                other.gameObject.GetComponent<BasicEnemy_BT>().AssociatedRoom = this;
                _associatedEnemies.Add(other.gameObject.GetComponent<BasicEnemy_BT>());
            }
        }
    } 

    public void RemoveEnemy(BasicEnemy_BT enemy)
    {
        if (_associatedEnemies.Contains(enemy))
        {
            _associatedEnemies.Remove(enemy);
            if(_associatedEnemies.Count == 0)
            {
                _roomCleared = true;
            }
        }
    }
}
