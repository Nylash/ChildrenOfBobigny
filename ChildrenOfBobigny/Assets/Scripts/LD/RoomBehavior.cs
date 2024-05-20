using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    #region VARIABLES
    [Header("VARIABLES")]
    [SerializeField, ReadOnly] private List<BasicEnemy> _associatedEnemies = new List<BasicEnemy>();
    [SerializeField, ReadOnly] private bool _roomCleared;
    #endregion

    #region ACCESSORS
    public bool RoomCleared { get => _roomCleared; set => _roomCleared = value; }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (BasicEnemy enemy in _associatedEnemies)
            {
                enemy.Target = other.transform;
            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(other.gameObject.GetComponent<BasicEnemy>().AssociatedRoom == null) 
            {
                other.gameObject.GetComponent<BasicEnemy>().AssociatedRoom = this;
                _associatedEnemies.Add(other.gameObject.GetComponent<BasicEnemy>());
            }
        }
    } 

    public void RemoveEnemy(BasicEnemy enemy)
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
