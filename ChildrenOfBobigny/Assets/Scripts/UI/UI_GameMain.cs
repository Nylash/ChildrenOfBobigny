using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameMain : Singleton<UI_GameMain>
{
    #region COMPONENTS
    //private UIDocument _gameMainUI;
    [SerializeField] private Image _healthBarProgress;
    [SerializeField] private Image _manaBarProgress;
    [SerializeField] private Image _dashBarProgress;
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
        _playerData.event_currentMPUpdated.AddListener(UpdateManaBar);
    }

    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;
        _playerData.event_dashAvailabilityUpdated.RemoveListener(SwitchDashStatus);
        _playerData.event_currentMPUpdated.RemoveListener(UpdateManaBar);
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
}
