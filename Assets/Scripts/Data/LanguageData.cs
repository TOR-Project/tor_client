using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LanguageData
{
    public LanguageItem[] items;
}

[System.Serializable]
public class LanguageItem
{
    public string key;
    public string value;
}