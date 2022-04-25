using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonModule
{
    private int dragonDetectRate = 0; // max 10 -> low 0

    public DragonModule()
    {
    }

    public void setDragonDetectRate(int rate)
    {
        dragonDetectRate = rate;
    }

    public int getDragon(List<int> tokenIdList, int usingScrollCount)
    {
        if (tokenIdList.Count == 0)
        {
            return -1;
        }

        for (int i = 0; i < usingScrollCount; i++)
        {
            bool detectSuccess = UnityEngine.Random.Range(0, 10000) < dragonDetectRate;
            if (detectSuccess)
            {
                int idx = UnityEngine.Random.Range(0, tokenIdList.Count);
                return tokenIdList[idx];
            }
        }

        return -1;
    }
}
