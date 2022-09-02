using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform _player;

    [SerializeField] private RectTransform _playerTargetUI;

    [SerializeField] private Image _dashGuage;

    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;


    public static UIManager Instance = null;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
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
        _levelText.text = $"검의 레벨 {level}";
    }


    public void SetCostGuage(int cost)
    {
        if (cost == -1)
        {
            _costText.text = $"검이 만족하였다.";
        }
        else
        {
            _costText.text = $"검이 원하는 피의 양 {cost}";
        }
    }
}
