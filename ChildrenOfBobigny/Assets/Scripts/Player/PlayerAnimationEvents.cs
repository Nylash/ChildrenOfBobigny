using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerAttackManager _attackManager;

    public void IsAttackCancelled()
    {
        _attackManager.IsAttackCancelled();
    }

    public void AttackFinished()
    {
        _attackManager.AttackFinishing();
    }
}