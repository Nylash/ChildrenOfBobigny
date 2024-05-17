using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Utilities;

public class UI_GameMain : Singleton<UI_GameMain>
{
    #region COMPONENTS
    [Header("COMPONENTS")]
    [SerializeField] private Image _healthBarProgress;
    [SerializeField] private Image _manaBarProgress;
    [SerializeField] private Image _dashBarProgress;
    [SerializeField] private Image _offensiveSpellIcon;
    [SerializeField] private Image _defensiveSpellIcon;
    [SerializeField] private Image _controlSpellIcon;
    [SerializeField] private Image _offensiveSpellCDImg;
    [SerializeField] private Image _defensiveSpellCDImg;
    [SerializeField] private Image _controlSpellCDImg;
    #endregion

    #region VARIABLES

    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private Data_Player _playerData;
    #endregion


    private void OnEnable()
    {
        _playerData.event_dashAvailabilityUpdated.AddListener(SwitchDashStatus);
        _playerData.event_currentHPUpdated.AddListener(UpdateHealthBar);
        _playerData.event_currentMPUpdated.AddListener(UpdateManaBar);
        _playerData.event_offensiveSpellCDUpdated.AddListener(UpdateOffensiveIcon);
        _playerData.event_defensiveSpellCDUpdated.AddListener(UpdateDefensiveIcon);
        _playerData.event_controlSpellCDUpdated.AddListener(UpdateControlIcon);
    }

    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;
        _playerData.event_dashAvailabilityUpdated.RemoveListener(SwitchDashStatus);
        _playerData.event_currentHPUpdated.RemoveListener(UpdateHealthBar);
        _playerData.event_currentMPUpdated.RemoveListener(UpdateManaBar);
        _playerData.event_offensiveSpellCDUpdated.RemoveListener(UpdateOffensiveIcon);
        _playerData.event_defensiveSpellCDUpdated.RemoveListener(UpdateDefensiveIcon);
        _playerData.event_controlSpellCDUpdated.RemoveListener(UpdateControlIcon);
    }

    private void UpdateHealthBar(float newHPValue)
    {
        _healthBarProgress.fillAmount = newHPValue / _playerData.MaxHP;
    }

    private void UpdateManaBar(float newMPValue)
    {
        _manaBarProgress.fillAmount = newMPValue / _playerData.MaxMP;
    }

    private void SwitchDashStatus(bool dashIsReady)
    {
        if(dashIsReady) 
        {
            _dashBarProgress.color = _playerData.ColorDashReady;
            StopCoroutine(FillingDashBar());
        }
        else
        {
            _dashBarProgress.color = _playerData.ColorDashInCD;
            StartCoroutine(FillingDashBar());
        }
    }

    private IEnumerator FillingDashBar()
    {
        float timer = 0;
        while (!_playerData.DashIsReady)
        {
            timer += Time.deltaTime;
            _dashBarProgress.fillAmount = timer / _playerData.DashCD;
            yield return null;
        }
    }

    private void UpdateOffensiveIcon(bool inCD)
    {
        if (inCD)
        {
            _offensiveSpellIcon.color = _playerData.TintSpellInCD;
            StartCoroutine(FillImageCD(SpellType.OFFENSIVE));
        }
        else
        {
            _offensiveSpellIcon.color = Color.white;
            _offensiveSpellCDImg.fillAmount = 0;
        }
    }

    private void UpdateDefensiveIcon(bool inCD)
    {
        if (inCD)
        {
            _defensiveSpellIcon.color = _playerData.TintSpellInCD;
            StartCoroutine(FillImageCD(SpellType.DEFENSIVE));
        }
        else
        {
            _defensiveSpellIcon.color = Color.white;
            _defensiveSpellCDImg.fillAmount = 0;
        }
    }

    private void UpdateControlIcon(bool inCD)
    {
        if (inCD)
        {
            _controlSpellIcon.color = _playerData.TintSpellInCD;
            StartCoroutine(FillImageCD(SpellType.CONTROL));
        }
        else
        {
            _controlSpellIcon.color = Color.white;
            _controlSpellCDImg.fillAmount = 0;
        }
    }

    private IEnumerator FillImageCD(SpellType type)
    {
        float timer = 0;
        switch (type)
        {
            case SpellType.OFFENSIVE:
                while (_playerData.OffensiveSpellInCD)
                {
                    timer += Time.deltaTime;
                    _offensiveSpellCDImg.fillAmount = timer / _playerData.OffensiveSpell.CD;
                    yield return null;
                }
                break;
            case SpellType.DEFENSIVE:
                
                while (_playerData.DefensiveSpellInCD)
                {
                    timer += Time.deltaTime;
                    _defensiveSpellCDImg.fillAmount = timer / _playerData.DefensiveSpell.CD;
                    yield return null;
                }
                break;
            case SpellType.CONTROL:
                while (_playerData.ControlSpellInCD)
                {
                    timer += Time.deltaTime;
                    _controlSpellCDImg.fillAmount = timer / _playerData.ControlSpell.CD;
                    yield return null;
                }
                break;
        }
    }
}
