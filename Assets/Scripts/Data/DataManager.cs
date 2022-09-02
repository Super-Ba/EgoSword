using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;


    [SerializeField] private SwordGrowthGameData _swordGrowthGameData;

    public SwordGrowthGameData SwordGrowthGameData => _swordGrowthGameData;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
