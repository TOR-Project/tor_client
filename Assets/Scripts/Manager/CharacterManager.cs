using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private string URL = Const.PRODUCTION ? "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/2_cypress/2_metadata/" : "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/1_baobab/2_metadata/";

    [SerializeField]
    private List<CharacterData> characterDataList = new List<CharacterData>();
    private int maxCharacterCount = 0;
    private int foundCharacterCount = 0;
    private int loadedCharacterCount = 0;

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
        characterDataList.Clear();
        maxCharacterCount = foundCharacterCount = loadedCharacterCount = 0;
    }

    public void setCharacterCount(int _count)
    {
        maxCharacterCount = _count;
    }

    public void setFoundCharacterCount(int _count)
    {
        foundCharacterCount = _count;
        // ContractManager.instance.printLog("Chracter found : (" + _count + "/" + maxCharacterCount + ")");
    }

    public bool isCharacterFindingCompleted()
    {
        return maxCharacterCount == foundCharacterCount;
    }

    public bool isCharacterLoadingCompleted()
    {
        return maxCharacterCount == loadedCharacterCount;
    }

    public void loadCharacter(int[] _idList)
    {
        loadedCharacterCount = 0;

        for (int i = 0; i < _idList.Length; i++)
        {
            StartCoroutine(loadCharacterSingle(_idList[i]));
        }
    }

    public IEnumerator loadCharacterSingle(int _id)
    {
        string url = URL + _id + ".json";
        // ContractManager.instance.printLog("Character loading start : " + url);
        for (int i = 0; i < 10; i++)
        {

            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                CharacterData data = getCharacterData(www.text);
                data.url = url;
                characterDataList.Add(data);
                // ContractManager.instance.printLog("Chracter loaded : " + data.tokenId + " (" + (loadedCharacterCount + 1) + "/" + maxCharacterCount + ")");
                break;
            }
            else
            {
                ContractManager.instance.printLog("ERROR: " + www.error);
            }
        }

        loadedCharacterCount++;
    }

    private CharacterData getCharacterData(string _jsonString)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_jsonString);

        CharacterData data = new CharacterData();
        data.tokenId = int.Parse(values["tokenId"].ToString());
        data.image = values["image"].ToString();
        data.description = values["description"].ToString();
        data.name= values["name"].ToString();

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
        }

        return data;
    }
}
