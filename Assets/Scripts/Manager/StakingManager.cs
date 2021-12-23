using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StakingManager : MonoBehaviour
{
    public const int PURPOSE_MONARCH = 1;
    public const int PURPOSE_GOVERNANCE = 2;
    public const int PURPOSE_REBELLION = 3;
    public const int PURPOSE_MINING = 4;


    static StakingManager mInstance;
    public static StakingManager instance {
        get {
            return mInstance;
        }
    }

    private StakingManager()
    {
        mInstance = this;
    }

    internal string getStakingText(int _purpose)
    {
        switch (_purpose)
        {
            case PURPOSE_MONARCH:
                return "ID_MONARCH_STATE";
            case PURPOSE_GOVERNANCE:
                return "ID_GOVERNANCE_STATE";
            case PURPOSE_REBELLION:
                return "ID_REBELLION_STATE";
            case PURPOSE_MINING:
                return "ID_MINING_STATE";
            default:
                return "";
        }
    }
}
