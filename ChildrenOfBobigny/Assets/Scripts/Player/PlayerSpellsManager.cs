using System.Collections;
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
    [SerializeField, Layer] private int _spellLayer;
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

    private void Start()
    {
        _playerData.CurrentMP = _playerData.MaxMP;
        _playerData.OffensiveSpellInCD = false;
        _playerData.DefensiveSpellInCD = false;
        _playerData.ControlSpellInCD = false;
    }

    private void OffensiveSpell()
    {
        if (InitCastingSpell(_playerData.OffensiveSpell))
        {
            CastSpell(_playerData.OffensiveSpell);
        }
        //else spell can't be launch
    }

    private void DefensiveSpell()
    {
        if (InitCastingSpell(_playerData.DefensiveSpell))
        {
            CastSpell(_playerData.DefensiveSpell);
        }
        //else spell can't be launch
    }

    private void ControlSpell()
    {

    }

    private bool InitCastingSpell(Data_Spell spell)
    {
        //Prevent casting while dashing
        if (PlayerMovementManager.Instance.CurrentMovementState == BehaviorState.DASH)
            return false;
        //Mana check
        if (_playerData.CurrentMP - spell.ManaCost < 0)
            return false;
        //CD check
        if (IsSpellInCD(spell))
            return false;

        _graphAnimator.SetTrigger("CastingSpell");
        //Prevent moving while casting
        PlayerMovementManager.Instance.StopReadMovementDirection();
        PlayerMovementManager.Instance.CurrentMovementState = BehaviorState.CAST;

        StartCoroutine(SpellCD(spell));
        _playerData.CurrentMP -= spell.ManaCost;

        return true;
    }

    //Actually cast the spell, by calling associated Init method
    private void CastSpell(Data_Spell spell)
    {
        GameObject _currentSpell = new GameObject("PlayerSpell");
        _currentSpell.layer = _spellLayer;
        _currentSpell.transform.position = transform.position;
        Spell _currentSpellScript = _currentSpell.AddComponent(GetSpellBehaviorType(spell)) as Spell;
        _currentSpellScript.Direction = new Vector3(PlayerAimManager.Instance.AimDirection.x, 0, PlayerAimManager.Instance.AimDirection.y);
        _currentSpellScript.Init(spell as dynamic);//Dynamic is used to call good Init method from abstract class Spell
    }

    public void FinishCastingSpell()
    {

        PlayerMovementManager.Instance.CurrentMovementState = BehaviorState.IDLE;
        /*
         * TODO If all spells have CD this is not needed. But if each category of spell has potentially no CD we need to check the 3 inputs
        */
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

    private IEnumerator SpellCD(Data_Spell spell)
    {
        switch (spell.CurrentSpellType)
        {
            case SpellType.OFFENSIVE:
                _playerData.OffensiveSpellInCD = true;
                yield return new WaitForSeconds(spell.CD);
                _playerData.OffensiveSpellInCD = false;
                break;
            case SpellType.DEFENSIVE:
                _playerData.DefensiveSpellInCD = true;
                yield return new WaitForSeconds(spell.CD);
                _playerData.DefensiveSpellInCD = false;
                break;
            case SpellType.CONTROL:
                _playerData.ControlSpellInCD = true;
                yield return new WaitForSeconds(spell.CD);
                _playerData.ControlSpellInCD = false;
                break;
        }
    }

    private bool IsSpellInCD(Data_Spell spell)
    {
        //Condensed switch
        return spell.CurrentSpellType switch
        {
            SpellType.OFFENSIVE => _playerData.OffensiveSpellInCD,
            SpellType.DEFENSIVE => _playerData.DefensiveSpellInCD,
            SpellType.CONTROL => _playerData.ControlSpellInCD,
            _ => true
        } ;
    }

    private System.Type GetSpellBehaviorType(Data_Spell spell)
    {
        //Condensed switch
        return spell.CurrentSpellBehavior switch
        {
            SpellBehavior.PROJECTILE => typeof(ProjectileSpell),
            SpellBehavior.SHIELD => typeof(ShieldSpell),
            _ => null,
        };
    }
}
