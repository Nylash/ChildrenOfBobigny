using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private bool _isAttacking;

    public bool IsAttacking { get => _isAttacking; set => _isAttacking = value; }

    public void AttackEnding()
    {
        _isAttacking = false;
    }
}
