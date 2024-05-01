using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UI_GameMainManager : MonoBehaviour
{
    public static UI_GameMainManager instance;

    #region COMPONENTS
    private UIDocument _gameMainUI;
    #endregion

    #region VARIABLES
    private ProgressBar _healthBar;
    private ProgressBar _manaBar;
    private ProgressBar _dashBar;

    public ProgressBar HealthBar { get => _healthBar; }
    public ProgressBar ManaBar { get => _manaBar; }
    public ProgressBar DashBar { get => _dashBar; }
    #endregion

    #region CONFIGURATION
    [SerializeField] private Color _dashInCD;
    [SerializeField] private Color _dashReady;
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        _gameMainUI = GetComponent<UIDocument>();
        _healthBar = _gameMainUI.rootVisualElement.Q("HealthBar") as ProgressBar;
        _manaBar = _gameMainUI.rootVisualElement.Q("ManaBar") as ProgressBar;
        _dashBar = _gameMainUI.rootVisualElement.Q("DashBar") as ProgressBar;

        _dashBar.Q(className: "unity-progress-bar__progress").style.backgroundColor = _dashReady;
    }

    public void SwitchDashBarColor()
    {
        if (_dashBar.Q(className: "unity-progress-bar__progress").style.backgroundColor != _dashInCD)
            _dashBar.Q(className: "unity-progress-bar__progress").style.backgroundColor = _dashInCD;
        else
            _dashBar.Q(className: "unity-progress-bar__progress").style.backgroundColor = _dashReady;
    }
}
