using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInfoManager : MonoBehaviour
{

    public long blockNumber = 0;

    static SystemInfoManager mInstance;
    public static SystemInfoManager instance {
        get {
            return mInstance;
        }
    }

    private SystemInfoManager()
    {
        mInstance = this;
    }

}