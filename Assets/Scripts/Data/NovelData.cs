using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NovelData
{
    public int id = 0;
    public string mainTitle = "";
    public string subTitle = "";
    public int mainCategory = 0;
    public int subCategory = 0;
    public string contents = "";
    public string thumbnailUrl = "";
    public string illustrationUrl = "";
    public long freeBlock = 0;
    public bool isFullDataLoaded = false;
    public bool isSubscribed = false;

    public bool isFreeOrSubscribe()
    {
        return freeBlock == 0 || isSubscribed;
    }
}