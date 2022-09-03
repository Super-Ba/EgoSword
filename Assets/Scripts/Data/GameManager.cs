using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;


    [SerializeField] private SwordGrowthGameData _swordGrowthGameData;
    public SwordGrowthGameData SwordGrowthGameData => _swordGrowthGameData;

    [SerializeField] private GameObject _hpItem;
    public GameObject HpItem => _hpItem;
    

    private void Awake()
    {
        Instance = this;
    }
}
