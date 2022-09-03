using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword
{
    private int _level =0;
    public int Level => _level;

    public int NextCost { get { return DataManager.Instance.SwordGrowthGameData.GetData(Level + 1)?.CostHp ?? -1; } }

    public int AtkPower { get { return DataManager.Instance.SwordGrowthGameData.GetData(Level)?.AtkPower ?? 0; } }
    public float AtkCoolTime { get { return DataManager.Instance.SwordGrowthGameData.GetData(Level)?.AtkCoolTime ?? 0; } }
    public float AtkRange { get { return DataManager.Instance.SwordGrowthGameData.GetData(Level)?.AtkRange ?? 0; } }
    public float Shake { get { return DataManager.Instance.SwordGrowthGameData.GetData(Level)?.Shake ?? 0; } }
    public float UpgradeCooltime { get { return DataManager.Instance.SwordGrowthGameData.GetData(Level)?.UpgradeCooltime ?? 0; } }

    public bool LevelUp(ref int hp)
    {
        if(DataManager.Instance.SwordGrowthGameData.GetData(Level + 1) == null)
        {
            return false;
        }

        hp -= NextCost;
        _level++;
        return true;
    }
}
