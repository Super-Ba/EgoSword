using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordGrowthGameData", menuName = "Proto/SwordGrowthGameData")]
public class SwordGrowthGameData : ScriptableObject
{
    [System.Serializable]
    public class SwordGameData
    {
        public int CostHp;

        public int AtkPower;
        public float AtkCoolTime;
        public float AtkRange;

        public float Shake;
    }


    public List<SwordGameData> SwordGameDatas;

    public SwordGameData GetData(int level)
    {
        if(SwordGameDatas == null || SwordGameDatas.Count == 0 || level < 0 || level >= SwordGameDatas.Count)
        {
            return null;
        }

        return SwordGameDatas[level];
    }

}
