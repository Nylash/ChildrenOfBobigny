using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerAttackManager _attackManager;

    public void IsAttackCancelled()
    {
        _attackManager.IsAttackCancelled();
    }

    public void AttackFinishing()
    {
        _attackManager.AttackFinishing();
    }

    public void StartNewAttack()
    {
        _attackManager.StartNewAttack();
    }
}