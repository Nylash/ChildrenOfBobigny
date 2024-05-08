using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_GameMain : Singleton<UI_GameMain>
{
    #region COMPONENTS
    private UIDocument _gameMainUI;
    #endregion

    #region VARIABLES
    private ProgressBar _healthBar;
    private ProgressBar _manaBar;
    private ProgressBar _dashBar;
    #endregion

    #region CONFIGURATION
    [Header("CONFIGURATION")]
    [SerializeField] private Data_Player _playerData;
    #endregion

    protected override void OnAwake()
    {
        _gameMainUI = GetComponent<UIDocument>();
        _healthBar = _gameMainUI.rootVisualElement.Q("HealthBar") as ProgressBar;
        _manaBar = _gameMainUI.rootVisualElement.Q("ManaBar") as ProgressBar;
        _dashBar = _gameMainUI.rootVisualElement.Q("DashBar") as ProgressBar;

        _dashBar.Q(className: "unity-progress-bar__progress").style.backgroundColor = _playerData.ColorDashReady;
    }

    private void OnEnable()
    {
        _playerData.event_dashAvailabilityUpdated.AddListener(SwitchDashStatus);
    }

    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;
        _playerData.event_dashAvailabilityUpdated.RemoveListener(SwitchDashStatus);
    }

    private void Start()
    {
        _dashBar.highValue = _playerData.DashCD;
        _dashBar.value = _playerData.DashCD;
    }

    private void SwitchDashStatus(bool dashIsReady)
    {
        if(dashIsReady) 
        {
            _dashBar.Q(className: "unity-progress-bar__progress").style.backgroundColor = _playerData.ColorDashReady;
            StopCoroutine(FillingDashBar());
        }
        else
        {
            _dashBar.Q(className: "unity-progress-bar__progress").style.backgroundColor = _playerData.ColorDashInCD;
            _dashBar.value = 0;
            StartCoroutine(FillingDashBar());
        }
    }

    private IEnumerator FillingDashBar()
    {
        while (!_playerData.DashIsReady)
        {
            _dashBar.value += Time.deltaTime;
            yield return null;
        }
    }
}
