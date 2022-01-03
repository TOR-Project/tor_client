using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private string META_URL = Const.PRODUCTION ? "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/2_cypress/2_metadata/" : "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/1_baobab/2_metadata/";
    private string IMAGE_URL = Const.PRODUCTION ? "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/2_cypress/1_image/" : "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/1_baobab/1_image/";

    public const int COUNTRY_EVEGENIS = 0;
    public const int COUNTRY_ENFILIIS = 1;
    public const int COUNTRY_HELLVESTA = 2;
    public const int COUNTRY_TRIPOLI = 3;
    public const int COUNTRY_BARBAROS = 4;
    public const int COUNTRY_MAX = 5;
    public const int COUNTRY_ALL = 99;

    public const int RACE_HUMAN = 0;
    public const int RACE_ELF = 1;
    public const int RACE_ORC = 2;
    public const int RACE_DARKELF = 3;
    public const int RACE_DRAGON = 4;
    public const int RACE_MAX = 5;
    public const int RACE_ALL = 99;

    public const int JOB_NOVICE = 0;
    public const int JOB_WARRIOR = 1;
    public const int JOB_RANGER = 2;
    public const int JOB_BISHOP = 3;
    public const int JOB_WIZARD = 4;
    public const int JOB_INFANTRY = 5;
    public const int JOB_WITCH_DOCTOR = 6;
    public const int JOB_ASSASSIN = 7;
    public const int JOB_SORCERER = 8;
    public const int JOB_MAX = 9;
    public const int JOB_ALL = 99;

    public const int TX_SPLIT_AMOUNT = 200;

    [SerializeField]
    private Dictionary<int, CharacterData> characterDataMap = new Dictionary<int, CharacterData>();
    private int characterCount = 0;
    private int stakingCharacterCount = 0;
    private int receivedStakingDataCount = 0;
    private int[] characterIdList = new int[] { };
    private int[] stakingCharacterIdList = new int[] { };
    private int[] notInitCharacterIdList = new int[] { };

    private List<CharacterData> notInitedCharacterDataList = new List<CharacterData>();
    private int[] pendingIdList;
    private int[] pendingCharacterDataList;
    private int[] pendingStatusDataList;
    private int[] pendingEquipDataList;

    public int loadingStep = 0;
    public int splitMax = 0;
    public int splitStep = 0;
    private Func<int, bool> foundCallback;
    private Func<int, bool> txCallback;
    public bool thereIsNotInitCharacter = false;

    static CharacterManager mInstance;
    public static CharacterManager instance {
        get {
            return mInstance;
        }
    }

    private CharacterManager()
    {
        mInstance = this;
    }

    public void resetAllData()
    {
        characterDataMap.Clear();
        characterCount  = 0;
        characterIdList = new int[] { };
        notInitCharacterIdList = new int[] { };
        notInitedCharacterDataList.Clear();
    }

    public List<CharacterData> getCharacterList()
    {
        return characterDataMap.Values.ToList();
    }

    public CharacterData getCharacterData(int _id)
    {
        return characterDataMap[_id];
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

    public string getRaceText(int _rid)
    {
        string key = "";
        switch (_rid)
        {
            case RACE_HUMAN:
                key = "ID_RACE_HUMAN";
                break;
            case RACE_ELF:
                key = "ID_RACE_ELF";
                break;
            case RACE_ORC:
                key = "ID_RACE_ORC";
                break;
            case RACE_DARKELF:
                key = "ID_RACE_DARKELF";
                break;
            case RACE_DRAGON:
                key = "ID_RACE_DRAGON";
                break;
            default:
                break;
        }

        return LanguageManager.instance.getText(key);
    }

    public string getJobText(int _jid)
    {
        string key = "";
        switch (_jid)
        {
            case JOB_NOVICE:
                key = "ID_JOB_NOVICE";
                break;
            case JOB_WARRIOR:
                key = "ID_JOB_WARRIOR";
                break;
            case JOB_RANGER:
                key = "ID_JOB_RANGER";
                break;
            case JOB_BISHOP:
                key = "ID_JOB_BISHOP";
                break;
            case JOB_WIZARD:
                key = "ID_JOB_WIZARD";
                break;
            case JOB_INFANTRY:
                key = "ID_JOB_INFANTRY";
                break;
            case JOB_WITCH_DOCTOR:
                key = "ID_JOB_WITCH_DOCTOR";
                break;
            case JOB_ASSASSIN:
                key = "ID_JOB_ASSASSIN";
                break;
            case JOB_SORCERER:
                key = "ID_JOB_SORCERER";
                break;
            default:
                break;
        }

        return LanguageManager.instance.getText(key);
    }



    /**
     *  1. start is there not inited character
     * */
    public void reqNotInitCharacterList(Func<int, bool> _foundCallback)
    {
        thereIsNotInitCharacter = true;
        foundCallback = _foundCallback;
        ContractManager.instance.reqNotInitCharacterList();
    }

    /**
     *  2. check there is not inited character
     *  3. load not inited character metadata from url
     * */
    public void setNotInitCharacterIdList(int[] _idList)
    {
        notInitCharacterIdList = _idList;

        if (notInitCharacterIdList.Length > 0)
        {
            loadCharacterFromMetadata();
        } else
        {
            thereIsNotInitCharacter = false;
        }
    }

    /**
      *  4. load not inited character list from url
      * */
    public void loadCharacterFromMetadata()
    {
        StartCoroutine(loadCharacterAsync(notInitCharacterIdList));
    }

    /**
      *  5. load async not inited character list from url
      *  6. generate character data for init character
      * */
    public IEnumerator loadCharacterAsync(int[] _idList)
    {
        notInitedCharacterDataList.Clear();

        for (int i = 0; i < _idList.Length; i++)
        {
            StartCoroutine(loadCharacterSingle(_idList[i]));
        }

        yield return new WaitUntil(() => notInitedCharacterDataList.Count == _idList.Length);

        generateCharacterDataForInit();
    }

    /**
      *  7. load not inited character single from url
      * */
    public IEnumerator loadCharacterSingle(int _id)
    {
        string url = META_URL + _id + ".json";
        // ContractManager.instance.printLog("Character loading start : " + url);
        for (int i = 0; i < 10; i++)
        {

            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                CharacterData data = getCharacterData(www.text);
                data.url = url;
                notInitedCharacterDataList.Add(data);
                // ContractManager.instance.printLog("Chracter loaded : " + data.tokenId + " (" + (loadedCharacterCount + 1) + "/" + maxCharacterCount + ")");
                break;
            }
            else
            {
                ContractManager.instance.printLog("ERROR: " + www.error);
            }
        }
    }

    /**
      *  8. parsing character data from url
      * */
    private CharacterData getCharacterData(string _jsonString)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_jsonString);

        CharacterData data = new CharacterData();
        data.tokenId = int.Parse(values["tokenId"].ToString());
        data.image = values["image"].ToString();
        data.description = values["description"].ToString();
        data.name = values["name"].ToString();

        object[] attrList = JsonConvert.DeserializeObject<object[]>(values["attributes"].ToString());
        for (int i = 0; i < attrList.Length; i++)
        {
            var attrValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(attrList[i].ToString());
            AttributeData aData = new AttributeData();
            if (attrValue.ContainsKey("trait_type"))
                aData.traitType = attrValue["trait_type"].ToString();
            if (attrValue.ContainsKey("boost_percentage"))
                aData.displayType = attrValue["boost_percentage"].ToString();
            if (attrValue.ContainsKey("value"))
                aData.valueStr = attrValue["value"].ToString();
            int.TryParse(aData.valueStr, out aData.valueInt);
            data.attributes.Add(aData);
            aData.applyToCharacterData(data);
        }

        return data;
    }

    /**
      *  9. generate character data for tx
      * */
    private void generateCharacterDataForInit()
    {
        pendingIdList = new int[notInitedCharacterDataList.Count];
        pendingCharacterDataList = new int[notInitedCharacterDataList.Count];
        pendingStatusDataList = new int[notInitedCharacterDataList.Count];
        pendingEquipDataList = new int[notInitedCharacterDataList.Count];

        for (int i = 0; i < notInitedCharacterDataList.Count; i++)
        {
            CharacterData cd = notInitedCharacterDataList[i];
            pendingIdList[i] = cd.tokenId;
            pendingCharacterDataList[i] = cd.level + cd.country * 10 + cd.race * 100 + cd.job * 1000;
            pendingStatusDataList[i] = cd.statusData.att + cd.statusData.def * 1000;
            pendingEquipDataList[i] = cd.equipData.weapon + cd.equipData.armor * 10 + cd.equipData.pants * 100 + cd.equipData.head * 1000 + cd.equipData.shoes * 10000 + cd.equipData.accessory * 100000;
        }

        splitMax = notInitedCharacterDataList.Count / TX_SPLIT_AMOUNT + (notInitedCharacterDataList.Count % TX_SPLIT_AMOUNT > 0 ? 1 : 0);
        splitStep = 0;
        foundCallback?.Invoke(splitMax);
        foundCallback = null;
    }

    /**
      *  10. request init character (called by character window controller)
      * */
    public void reqInitCharacter(Func<int, bool> _txCallback)
    {
        txCallback = _txCallback;

        int offset = TX_SPLIT_AMOUNT * splitStep;
        int length = (notInitedCharacterDataList.Count >= TX_SPLIT_AMOUNT * (splitStep + 1)) ? TX_SPLIT_AMOUNT : notInitedCharacterDataList.Count - TX_SPLIT_AMOUNT * splitStep;

        int[] _pendingIdList = new int[length];
        Array.Copy(pendingIdList, offset, _pendingIdList, 0, length);

        int[] _pendingCharacterDataList = new int[length];
        Array.Copy(pendingCharacterDataList, offset, _pendingCharacterDataList, 0, length);

        int[] _pendingStatusDataList = new int[length];
        Array.Copy(pendingStatusDataList, offset, _pendingStatusDataList, 0, length);

        int[] _pendingEquipDataList = new int[length];
        Array.Copy(pendingEquipDataList, offset, _pendingEquipDataList, 0, length);

        ContractManager.instance.reqInitCharacter(_pendingIdList, _pendingCharacterDataList, _pendingStatusDataList, _pendingEquipDataList);
    }

    /**
      *  11. completed init character
      * */
    public void initCharacterCompleted()
    {
        txCallback?.Invoke(++splitStep);

        if (splitMax == splitStep)
        {
            notInitCharacterIdList = new int[] { };
            thereIsNotInitCharacter = false;
        }
    }



    /**
     *  1. request character count
     * */
    public void loadCharacter()
    {
        ContractManager.instance.reqCharacterCount();
        loadingStep = 1;
    }

    /**
     *  2. receive character count
     *  3. request character list
     * */
    public void setCharacterCount(int _count, int _stakingCount)
    {
        characterCount = _count;
        stakingCharacterCount = _stakingCount;

        ContractManager.instance.reqCharacterList(characterCount);
        loadingStep = 2;
    }

    /**
     *  4. receive character list
     *  5. request character data from blockchain
     * */
    public void setCharacterIdList(int[] _idList, int[] _stakingIdList)
    {
        characterIdList = _idList;
        stakingCharacterIdList = _stakingIdList;
        characterDataMap.Clear();

        int[] allCharacterIdList = characterIdList.Concat(stakingCharacterIdList).ToArray();
        if (allCharacterIdList.Length == 0)
        {
            reqStakingData();
        }
        else
        {
            ContractManager.instance.reqCharacterData(allCharacterIdList);
            loadingStep = 3;
        }
    }

    /**
      *  6. receive character data from blockchain
      * */
    public void parsingCharacterData(Dictionary<string, object> _characterData, Dictionary<string, object> _statusData, Dictionary<string, object> _equipData)
    {
        CharacterData data = new CharacterData();

        data.tokenId = int.Parse(_characterData["tokenId"].ToString());
        data.image = IMAGE_URL + data.tokenId + ".jpg";
        data.name = _characterData["name"].ToString();
        if (data.name.Equals(""))
        {
            data.name = "#" + data.tokenId.ToString("0000");
        }

        data.level = int.Parse(_characterData["level"].ToString());
        data.exp = int.Parse(_characterData["exp"].ToString());
        data.country = int.Parse(_characterData["country"].ToString());
        data.race = int.Parse(_characterData["race"].ToString());
        data.job = int.Parse(_characterData["job"].ToString());
        data.statusBonus = int.Parse(_characterData["statusBonus"].ToString());
        data.version = int.Parse(_characterData["version"].ToString());

        data.statusData.att = int.Parse(_statusData["att"].ToString());
        data.statusData.def = int.Parse(_statusData["def"].ToString());

        data.equipData.weapon = int.Parse(_equipData["weapon"].ToString());
        data.equipData.head = int.Parse(_equipData["head"].ToString());
        data.equipData.accessory = int.Parse(_equipData["accessory"].ToString());
        data.equipData.armor = int.Parse(_equipData["armor"].ToString());
        data.equipData.pants = int.Parse(_equipData["pants"].ToString());
        data.equipData.shoes = int.Parse(_equipData["shoes"].ToString());

        if (!characterDataMap.ContainsKey(data.tokenId))
        {
            characterDataMap.Add(data.tokenId, data);
        } else
        {
            characterCount--;
        }

        if (characterDataMap.Count >= (characterCount + stakingCharacterCount))
        {
            reqStakingData();
        }
    }

    /**
      *  7. request staking character data from blockchain
      * */
    public void reqStakingData()
    {
        receivedStakingDataCount = 0;
        if (stakingCharacterCount == 0)
        {
            finishCharacterLoading();
        }
        else
        {
            ContractManager.instance.reqStakingData(stakingCharacterIdList);
            loadingStep = 4;
        }
    }

    /**
      *  6. receive character data from blockchain
      * */
    public void parsingStakingData(Dictionary<string, object> _stakingData)
    {
        int tokenId = int.Parse(_stakingData["id"].ToString());

        foreach (CharacterData cd in characterDataMap.Values)
        {
            if (cd.tokenId == tokenId)
            {
                cd.stakingData.parse(_stakingData);
                break;
            }
        }

        receivedStakingDataCount++;
        Debug.Log("parsingStakingData : " + receivedStakingDataCount + " / " + stakingCharacterCount);

        if (receivedStakingDataCount >= stakingCharacterCount)
        {
            finishCharacterLoading();
        }
    }

    /**
      *  9. finish character loading
      * */
    public void finishCharacterLoading()
    {
        loadingStep = 5;
    }

    public void resetLoadingStep()
    {
        loadingStep = 0;
    }
}
