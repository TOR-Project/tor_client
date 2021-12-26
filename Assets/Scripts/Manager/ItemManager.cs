using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    static ItemManager mInstance;
    public static ItemManager instance {
        get {
            return mInstance;
        }
    }

    private ItemManager()
    {
        mInstance = this;
    }

}