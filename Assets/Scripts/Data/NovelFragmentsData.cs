using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum NovelFragmentsType
{
    TITLE,
    CONTENTS
}

[System.Serializable]
public class NovelFragmentsData
{
    public int page = 0;
    public string contents = "";
    public NovelFragmentsType type;

    public NovelFragmentsData(int _page, string _contetns, NovelFragmentsType _type)
    {
        page = _page;
        contents = _contetns;
        type = _type;
    }

}