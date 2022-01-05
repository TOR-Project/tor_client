using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CountryManager : MonoBehaviour
{
    public const int COUNTRY_EVEGENIS = 0;
    public const int COUNTRY_ENFILIIS = 1;
    public const int COUNTRY_HELLVESTA = 2;
    public const int COUNTRY_TRIPOLI = 3;
    public const int COUNTRY_BARBAROS = 4;
    public const int COUNTRY_MAX = 5;
    public const int COUNTRY_ALL = 99;

    private string[] FLAG_IMAGE_URL_LIST = new string[]
    {
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/flag/evegenis.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/flag/enfiliis.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/flag/hellvesta.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/flag/tripoli.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/flag/barbaros.png",
    };

    Dictionary<int, Sprite> flagSpriteMap = new Dictionary<int, Sprite>();

    static CountryManager mInstance;
    public static CountryManager instance {
        get {
            return mInstance;
        }
    }

    private CountryManager()
    {
        mInstance = this;
    }
    
    public string getCountryText(int _cid)
    {
        string key = "";
        switch(_cid)
        {
            case COUNTRY_EVEGENIS:
                key = "ID_EVEGENIS";
                break;
            case COUNTRY_ENFILIIS:
                key = "ID_ENFILIIS";
                break;
            case COUNTRY_HELLVESTA:
                key = "ID_HELLVESTA";
                break;
            case COUNTRY_TRIPOLI:
                key = "ID_TRIPOLI";
                break;
            case COUNTRY_BARBAROS:
                key = "ID_BARBAROS";
                break;
            default:
                break;
        }

        return LanguageManager.instance.getText(key);
    }

    public void startFlagImageLoading()
    {
        for (int i = 0; i < FLAG_IMAGE_URL_LIST.Length; i++)
        {
            if (FLAG_IMAGE_URL_LIST[i] == "")
            {
                continue;
            }

            int key = i;
            AssetsLoadManager.instance.requestSprite(FLAG_IMAGE_URL_LIST[key], (_sprite) =>
            {
                if (!flagSpriteMap.ContainsKey(key))
                {
                    flagSpriteMap.Add(key, _sprite);
                }
                return true;
            }, null);
        }
    }

    public bool isFlagImageLoaded()
    {
        return flagSpriteMap.Count >= COUNTRY_MAX;
    }

    public Sprite getFlagImage(int _cid)
    {
        return flagSpriteMap[_cid];
    }
}
