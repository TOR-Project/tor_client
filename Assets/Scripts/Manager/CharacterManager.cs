﻿using Newtonsoft.Json;
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
    }

    public void loadCharacter(int[] _idList, Func<int, bool> progressCallback, Action finishCallback)
    {
        StartCoroutine(loadCharacterList(_idList, progressCallback, finishCallback));
    }

    public IEnumerator loadCharacterList(int[] _idList, Func<int, bool> progressCallback, Action finishCallback)
    {
        maxCharacterCount = _idList.Length;
        loadedCharacterCount = 0;

        for (int i = 0; i < _idList.Length; i++)
        {
            StartCoroutine(loadCharacterSingle(progressCallback, _idList[i]));
        }

        yield return new WaitUntil(() => maxCharacterCount <= loadedCharacterCount);

        finishCallback();
    }

    public IEnumerator loadCharacterSingle(Func<int, bool> progressCallback, int _id)
    {
        for(int i = 0; i < 10; i++)
        {
            string url = URL + _id + ".json";
            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                CharacterData data = getCharacterData(www.text);
                data.url = url;
                characterDataList.Add(data);
                Debug.Log("Chracter loaded : " + data.tokenId);
                break;
            }
            else
            {
                Debug.Log("ERROR: " + www.error);
            }
        }
        progressCallback(loadedCharacterCount++);
    }

    private CharacterData getCharacterData(string  jsonString)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

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
