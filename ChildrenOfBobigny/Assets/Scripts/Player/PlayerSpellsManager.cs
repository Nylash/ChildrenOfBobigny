using UnityEngine;
using static Data_Spell;
using static PlayerMovementManager;

public class PlayerSpellsManager : Singleton<PlayerSpellsManager>
{
    #region COMPONENTS
    private ControlsMap _controlsMap;
    [SerializeField] private Animator _graphAnimator;
    #endregion

    #region VARIABLES

    #region ACCESSORS

    #endregion
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private Data_Player _playerData;
    [SerializeField] private GameObject _emptySpell;
    #endregion

    private void OnEnable()
    {
        _controlsMap.Gameplay.Enable();
    }
    private void OnDisable()
    {
        _controlsMap.Gameplay.Disable();
        if (!this.gameObject.scene.isLoaded) return;//Avoid null ref
        //Remove listener here
    }

    protected override void OnAwake()
    {
        _controlsMap = new ControlsMap();

        _controlsMap.Gameplay.OffensiveSpell.performed += ctx => OffensiveSpell();
        _controlsMap.Gameplay.DefensiveSpell.performed += ctx => DefensiveSpell();
        _controlsMap.Gameplay.ControlSpell.performed += ctx => ControlSpell();
    }

    private void OffensiveSpell()
    {
        if (InitCastingSpell())
        {
            GameObject _currentSpell = new GameObject();
            Spell _currentSpellScript = _currentSpell.AddComponent(GetSpellBehaviorType(_playerData.OffensiveSpell.CurrentSpellBehavior)) as Spell;
            _currentSpellScript.Init(_playerData.OffensiveSpell as dynamic);
        }
        return;
    }

    private void DefensiveSpell()
    {

    }

    private void ControlSpell()
    {

    }

    private bool InitCastingSpell()
    {
        //Prevent casting while dashing
        if (PlayerMovementManager.Instance.CurrentMovementState != BehaviorState.DASH)
        {
            _graphAnimator.SetTrigger("CastingSpell");
            //Prevent moving while casting
            PlayerMovementManager.Instance.StopReadMovementDirection();
            PlayerMovementManager.Instance.CurrentMovementState = BehaviorState.CAST;
            return true;
        }
        return false;
    }

    public void FinishCastingSpell()
    {

        PlayerMovementManager.Instance.CurrentMovementState = BehaviorState.IDLE;
        //If spell input still pressed we start attacking right away
        if (_controlsMap.Gameplay.OffensiveSpell.IsPressed())
        {
            OffensiveSpell();
        }
        //If movement input is pressed we directly start using it
        else if (_controlsMap.Gameplay.Movement.IsPressed())
        {
            PlayerMovementManager.Instance.ReadMovementDirection(PlayerMovementManager.Instance.MovementDirection);
        }
    }

    private System.Type GetSpellBehaviorType(Data_Spell.SpellBehavior behavior)
    {
        return behavior switch
        {
            SpellBehavior.PROJECTILE => typeof(ProjectileSpell),
            _ => null,
        };
    }
}
