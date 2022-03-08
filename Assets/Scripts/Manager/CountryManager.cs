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

    public const int PROPERTY_CATEGORY_EVEGENIS_RAYNOR_BLESS = 1;

    public const int PROPERTY_TYPE_MINING_INC = 1;
    public const int PROPERTY_TYPE_MINING_DEC = 2;

    public const int LOG_TYPE_RESIGNATION = 1;
    public const int LOG_TYPE_INAUGURATION = 2;
    public const int LOG_TYPE_ADJUST_MINING_TAX = 3;
    public const int LOG_TYPE_DONATION = 4;
    public const int LOG_TYPE_START_RESEARCH = 5;
    public const int LOG_TYPE_STOP_RESEARCH_STOP = 6;
    public const int LOG_TYPE_END_RESEARCH = 7;
    public const int LOG_TYPE_OCCUR_REBELLION = 8;
    public const int LOG_TYPE_FAIL_REBELLION = 9;
    public const int LOG_TYPE_SUCCESS_REBELLION = 10;
    public const int LOG_TYPE_RENAME_CASTLE = 11;
    public const int LOG_TYPE_ADD_PROPERTY = 12;
    public const int LOG_TYPE_REBELLION_START = 13;
    public const int LOG_TYPE_REBELLION_FAILED = 14;
    public const int LOG_TYPE_REBELLION_SUCCESSED = 15;

    public const int COUNTRY_DATA_REQUEST_INTERVAL = 180; // 3 min

    private string[] CHARACTER_FLAG_IMAGE_URL_LIST = new string[]
    {
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/flag/evegenis.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/flag/enfiliis.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/flag/hellvesta.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/flag/tripoli.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/flag/barbaros.png",
    };

    private string[] BIG_FLAG_IMAGE_URL_LIST = new string[]
    {
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/country_flag/flag_0002_A35-4.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/country_flag/flag_0004_A35-2.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/country_flag/flag_0000_A35.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/country_flag/flag_0003_A35-3.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/country_flag/flag_0001_A35-5.png",
    };

    private string[] SMALL_FLAG_IMAGE_URL_LIST = new string[]
    {
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/country_flag/small_bedge_0001_%EC%A0%9C%EB%AA%A9_%EC%97%86%EB%8A%94_%EC%95%84%ED%8A%B8%EC%9B%8C%ED%81%AC-21.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/country_flag/small_bedge_0004_%EC%A0%9C%EB%AA%A9_%EC%97%86%EB%8A%94_%EC%95%84%ED%8A%B8%EC%9B%8C%ED%81%AC-19.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/country_flag/small_bedge_0002_%EC%A0%9C%EB%AA%A9_%EC%97%86%EB%8A%94_%EC%95%84%ED%8A%B8%EC%9B%8C%ED%81%AC-20.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/country_flag/small_bedge_0000_%EC%A0%9C%EB%AA%A9_%EC%97%86%EB%8A%94_%EC%95%84%ED%8A%B8%EC%9B%8C%ED%81%AC-22.png",
        "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/country_flag/small_bedge_0003_%EC%A0%9C%EB%AA%A9_%EC%97%86%EB%8A%94_%EC%95%84%ED%8A%B8%EC%9B%8C%ED%81%AC-23.png",
    };

    Dictionary<int, CountryData> countryDataMap = new Dictionary<int, CountryData>();
    Dictionary<int, List<Func<CountryData, bool>>> countryDataCallbackPendingMap = new Dictionary<int, List<Func<CountryData, bool>>>();
    Dictionary<int, List<Func<CountryData, bool>>> logDataCallbackPendingMap = new Dictionary<int, List<Func<CountryData, bool>>>();

    Dictionary<int, Sprite> characterFlagSpriteMap = new Dictionary<int, Sprite>();
    Dictionary<int, Sprite> bigFlagSpriteMap = new Dictionary<int, Sprite>();
    Dictionary<int, Sprite> smallFlagSpriteMap = new Dictionary<int, Sprite>();

    static CountryManager mInstance;
    public static CountryManager instance
    {
        get
        {
            return mInstance;
        }
    }

    private CountryManager()
    {
        mInstance = this;
    }

    public string getCountryName(int _cid)
    {
        string key = "";
        switch (_cid)
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

    public string getCountryExp(int _cid)
    {
        string key = "";
        switch (_cid)
        {
            case COUNTRY_EVEGENIS:
                key = "ID_EVEGENIS_EXP";
                break;
            case COUNTRY_ENFILIIS:
                key = "ID_ENFILIIS_EXP";
                break;
            case COUNTRY_HELLVESTA:
                key = "ID_HELLVESTA_EXP";
                break;
            case COUNTRY_TRIPOLI:
                key = "ID_TRIPOLI_EXP";
                break;
            case COUNTRY_BARBAROS:
                key = "ID_BARBAROS_EXP";
                break;
            default:
                break;
        }

        return LanguageManager.instance.getText(key);
    }

    public string getPropertyTitle(int _id)
    {
        string key = "";
        switch (_id)
        {
            case PROPERTY_CATEGORY_EVEGENIS_RAYNOR_BLESS:
                key = "ID_PROPERTY_CATEGORY_BLESS_OF_RAYNOR";
                break;
            default:
                break;
        }

        return LanguageManager.instance.getText(key);
    }

    public void startFlagImageLoading()
    {
        for (int i = 0; i < COUNTRY_MAX; i++)
        {
            int key = i;
            AssetsLoadManager.instance.requestSprite(CHARACTER_FLAG_IMAGE_URL_LIST[key], (_sprite) =>
            {
                if (!characterFlagSpriteMap.ContainsKey(key))
                {
                    characterFlagSpriteMap.Add(key, _sprite);
                }
                return true;
            }, null);

            AssetsLoadManager.instance.requestSprite(BIG_FLAG_IMAGE_URL_LIST[key], (_sprite) =>
            {
                if (!bigFlagSpriteMap.ContainsKey(key))
                {
                    bigFlagSpriteMap.Add(key, _sprite);
                }
                return true;
            }, null);

            AssetsLoadManager.instance.requestSprite(SMALL_FLAG_IMAGE_URL_LIST[key], (_sprite) =>
            {
                if (!smallFlagSpriteMap.ContainsKey(key))
                {
                    smallFlagSpriteMap.Add(key, _sprite);
                }
                return true;
            }, null);
        }
    }

    public bool isFlagImageLoaded()
    {
        return characterFlagSpriteMap.Count >= COUNTRY_MAX
            && bigFlagSpriteMap.Count >= COUNTRY_MAX
            && smallFlagSpriteMap.Count >= COUNTRY_MAX;
    }

    public Sprite getCharacterFlagImage(int _cid)
    {
        return characterFlagSpriteMap[_cid];
    }

    public Sprite getBigFlagImage(int _cid)
    {
        return bigFlagSpriteMap[_cid];
    }

    public Sprite getSmallFlagImage(int _cid)
    {
        return smallFlagSpriteMap[_cid];
    }

    public bool isMonarcOfCountry(int _cid)
    {
        if (!countryDataMap.ContainsKey(_cid))
        {
            countryDataMap.Add(_cid, new CountryData());
        }

        if (!countryDataMap[_cid].castleData.hasMonarch)
        {
            return false;
        }

        return CharacterManager.instance.hasCharacter(countryDataMap[_cid].castleData.monarchId);

    }

    public void requestCountryData(int _cid, Func<CountryData, bool> _callback)
    {
        if (!countryDataMap.ContainsKey(_cid))
        {
            countryDataMap.Add(_cid, new CountryData());
        }

        if (countryDataMap[_cid].lastUpdatedBlock + COUNTRY_DATA_REQUEST_INTERVAL < SystemInfoManager.instance.blockNumber)
        {

            if (!countryDataCallbackPendingMap.ContainsKey(_cid))
            {
                countryDataCallbackPendingMap.Add(_cid, new List<Func<CountryData, bool>>());
            }

            countryDataCallbackPendingMap[_cid].Add(_callback);
            ContractManager.instance.reqCountryData(_cid);
        }
        else
        {
            _callback(countryDataMap[_cid]);
        }
    }

    public void responseCountyData(Dictionary<string, object> _data)
    {
        int cid = int.Parse(_data["id"].ToString());
        CountryData countryData = countryDataMap[cid];
        countryData.parseData(_data);
        Debug.Log("responseCountyData " + cid);
        foreach (Func<CountryData, bool> callback in countryDataCallbackPendingMap[cid])
        {
            callback(countryData);
        }

        countryDataCallbackPendingMap[cid].Clear();
    }

    public void resetLastUpdatedBlock()
    {
        for (int cid = 0; cid < countryDataMap.Count; cid++)
        {
            countryDataMap[cid].lastUpdatedBlock = 0;
        }
    }

    public CountryData getCountryData(int _cid)
    {
        if (countryDataMap.ContainsKey(_cid)) return countryDataMap[_cid];
        return new CountryData();
    }

    public void addLog(int _cid, LogData _data)
    {
        if (!countryDataMap.ContainsKey(_cid))
        {
            countryDataMap.Add(_cid, new CountryData());
        }

        countryDataMap[_cid].addLog(_data);
    }

    internal void requestMoreLogData(int _cid, int _fromId, int _count, Func<CountryData, bool> _callback)
    {
        if (!countryDataMap.ContainsKey(_cid))
        {
            countryDataMap.Add(_cid, new CountryData());
        }

        if (!logDataCallbackPendingMap.ContainsKey(_cid))
        {
            logDataCallbackPendingMap.Add(_cid, new List<Func<CountryData, bool>>());
        }

        logDataCallbackPendingMap[_cid].Add(_callback);
        ContractManager.instance.reqMoreLogData(_cid, _fromId, _count);
    }

    public void responseLogData(Dictionary<string, object> _data)
    {
        int cid = int.Parse(_data["id"].ToString());
        CountryData countryData = countryDataMap[cid];
        countryData.parseLogData(_data);

        Debug.Log("responseLogData " + cid);
        foreach (Func<CountryData, bool> callback in logDataCallbackPendingMap[cid])
        {
            callback(countryData);
        }

        logDataCallbackPendingMap[cid].Clear();
    }
}
