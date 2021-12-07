using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LanguageManager : MonoBehaviour
{
    public const int LANG_ENG = 0;
    public const int LANG_KOR = 1;
    public const int LANG_FRN = 2;
    public const int LANG_DEU = 3;
    public const int LANG_ITA = 4;
    public const int LANG_ESP = 5;
    public const int LANG_PQR = 6;
    public const int LANG_PYC = 7;
    public const int LANG_CHN_G = 8;
    public const int LANG_CHN_B = 9;
    public const int LANG_JAP = 10;
    public const int LANG_COUNT = 10;

    private ArrayList observerList = new ArrayList();
    private Dictionary<string, string> currentLanguageMap = new Dictionary<string, string>();
    private string missingTextString = "";
    public int currentLanguage = 0;
    private bool isLoadingCompleted = false;

    private static LanguageManager mInstance;
    public static LanguageManager instance {
        get {
            return mInstance;
        }
    }

    private LanguageManager()
    {
        mInstance = this;
    }

    public IEnumerator loadLanguagePack()
    {
        currentLanguageMap.Clear();
        string filePath;
        string dataAsJson = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = Path.Combine("jar:file://" + Application.dataPath + "!assets/", getFileName());
            WWW reader = new WWW(filePath);
            while (!reader.isDone)
            {
            }
            dataAsJson = reader.text;
        } else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            filePath = Path.Combine(Application.streamingAssetsPath, getFileName());
            WWW www = new WWW(filePath);
            yield return www;
            if (www.error == null)
            {
                dataAsJson = www.text;
            }
            else
            {
                Debug.Log("ERROR: " + www.error);
            }
        } else
        {
            filePath = Path.Combine(Application.streamingAssetsPath, getFileName());
            if (File.Exists(filePath))
            {
                dataAsJson = File.ReadAllText(filePath);
            }
        }

        Debug.Log("Loading langunage file = " + filePath + ", platform = " + Application.platform);
        LanguageData loadedData = JsonUtility.FromJson<LanguageData>(dataAsJson);

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            currentLanguageMap.Add(loadedData.items[i].key, loadedData.items[i].value);
        }

        Debug.Log("Data loaded, dictionary contains: " + currentLanguageMap.Count + " entries");
    }

    public int getDefaultLanguage()
    {
        SystemLanguage locale = Application.systemLanguage;
        switch (locale)
        {
            default:
            case SystemLanguage.English:
                return LANG_ENG;
            case SystemLanguage.Korean:
                return LANG_KOR;
            case SystemLanguage.French:
                return LANG_FRN;
            case SystemLanguage.Dutch:
                return LANG_DEU;
            case SystemLanguage.Italian:
                return LANG_ITA;
            case SystemLanguage.Spanish:
                return LANG_ESP;
            case SystemLanguage.Portuguese:
                return LANG_PQR;
            case SystemLanguage.Russian:
                return LANG_PYC;
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseSimplified:
                return LANG_CHN_B;
            case SystemLanguage.ChineseTraditional:
                return LANG_CHN_G;
            case SystemLanguage.Japanese:
                return LANG_JAP;
        }
    }

    private string getFileName()
    {
        switch(currentLanguage)
        {
            case LANG_ENG:
                return "eng.json";
            case LANG_KOR:
                return "kor.json";
            case LANG_FRN:
                return "frn.json";
            case LANG_DEU:
                return "deu.json";
            case LANG_ITA:
                return "ita.json";
            case LANG_ESP:
                return "esp.json";
            case LANG_PQR:
                return "pqr.json";
            case LANG_PYC:
                return "pyc.json";
            case LANG_CHN_G:
                return "chn_g.json";
            case LANG_CHN_B:
                return "chn_b.json";
            case LANG_JAP:
                return "jpn.json";
        }

        return "eng.json";
    }

    public string getText(string key)
    {
        string result = missingTextString;
        if (currentLanguageMap.ContainsKey(key))
        {
            result = currentLanguageMap[key];
        }

        return result;

    }

    private void Awake()
    {
        StartCoroutine(loadLanguage());
    }

    public void setLanguage(int lang)
    {
        currentLanguage = lang;
        StartCoroutine(loadLanguagePack());
    }

    private IEnumerator setLanguageCoroutine()
    {
        yield return loadLanguagePack();
        notifyLanguageChanged();
    }

    private IEnumerator loadLanguage()
    {
        currentLanguage = getDefaultLanguage();
        yield return loadLanguagePack();
        notifyLanguageChanged();

        isLoadingCompleted = true;
    }

    public bool isReady()
    {
        return isLoadingCompleted;
    }

    public void notifyLanguageChanged()
    {
        foreach (LanguageObserever ob in observerList)
        {
            ob.onLanguageChanged();
        }
    }

    public void addObserver(LanguageObserever ob)
    {
        observerList.Add(ob);
        ob.onLanguageChanged();
    }

    public void removeObserver(LanguageObserever ob)
    {
        observerList.Remove(ob);
    }
}
