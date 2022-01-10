using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NovelData
{
    public const string TITLE_TAG = "<title>";
    public const string TITLE_END_TAG = "</title>";

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

    public NovelData(Dictionary<string, object> _values)
    {
        id = int.Parse(_values["id"].ToString());

        mainTitle = _values["mainTitle"].ToString();
        subTitle = _values["subTitle"].ToString();

        mainCategory = int.Parse(_values["mainCategory"].ToString());
        subCategory = int.Parse(_values["subCategory"].ToString());

        thumbnailUrl = _values["thumbnailUrl"].ToString();

        freeBlock = long.Parse(_values["freeBlock"].ToString());

        isSubscribed = bool.Parse(_values["isSubscribed"].ToString());
    }

    public void setFullContents(string _contents, string _illustrationUrl)
    {
        contents = TITLE_TAG + mainTitle + TITLE_END_TAG + _contents;
        illustrationUrl = _illustrationUrl;
        isFullDataLoaded = true;
    }

    public bool isFreeOrSubscribe()
    {
        return freeBlock == 0 || isSubscribed;
    }
}