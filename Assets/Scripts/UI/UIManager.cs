using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] private Transform _player;

    [SerializeField] private GameObject _gameoverScreen;

    [SerializeField] private RectTransform _playerTargetUI;

    [SerializeField] private Image _dashGuage;

    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;

    [SerializeField] private GameObject _upgradeEffect;

    public static UIManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _playerTargetUI.position = Camera.main.WorldToScreenPoint(_player.position);
    }

    public void SetDashGuage(float value)
    {
        _dashGuage.fillAmount = value;
    }

    public void SetHpGuage(int current, int max)
    {
        _hpText.text = $"체력 {current}/{max}";
    }


    public void SetLevelGuage(int level)
    {
        _levelText.text = $"검의 레벨 : {level}";
    }


    public void SetCostGuage(int cost)
    {
        if (cost == -1)
        {
            _costText.text = $"검이 만족하였다.";
        }
        else
        {
            _costText.text = $"검이 원하는 피의 양 : {cost} (Q를 눌러 흡혈)";
        }
    }

    public void SetCostGuage(float time)
    {
        _costText.text = $"다음 흡혈 까지 {(int) time}초 남음.";
    }

    public void ShowSwordUpgradeEffect()
    {
        CameraMovement.StartShake(0.5f, 2);
        
        _upgradeEffect.SetActive(false);
        _upgradeEffect.SetActive(true);
    }

    public void ShowGameOverScreen()
    {
        _gameoverScreen.SetActive(true);
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(0);
    }
}