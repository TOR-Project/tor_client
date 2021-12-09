using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchDogManager : MonoBehaviour
{
    static WatchDogManager mInstance;
    public static WatchDogManager instance {
        get {
            return mInstance;
        }
    }

    private WatchDogManager()
    {
        mInstance = this;
    }


}