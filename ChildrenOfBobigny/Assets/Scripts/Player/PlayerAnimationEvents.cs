using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    #region ATTACK EVENTS
    public void IsAttackCancelled()
    {
        PlayerAttackManager.Instance.IsAttackCancelled();
    }

    public void FinishAttack()
    {
        PlayerAttackManager.Instance.FinishAttack();
    }

    public void StartNewAttack()
    {
        PlayerAttackManager.Instance.StartNewAttack();
    }
    #endregion

    #region SPELL EVENTS
    public void FinishCastingSpell()
    {
        PlayerSpellsManager.Instance.FinishCastingSpell();
    }
    #endregion
}