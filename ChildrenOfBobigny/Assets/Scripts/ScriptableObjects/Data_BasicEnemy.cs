using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This SO is only use for getting data, nothing is store here since multiple instance can access it.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Basic Enemy Data")]
public class Data_BasicEnemy : ScriptableObject
{
    [SerializeField] private int _maxHP;

    #region ACCESSORS
    public int MaxHP { get => _maxHP;}
    #endregion
}