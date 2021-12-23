using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Numerics;
using System;

public class EditorContractCommunicator : IContractCommunicator
{
    private ContractManager mContractManager;

    public EditorContractCommunicator(ContractManager cm)
    {
        mContractManager = cm;
        mContractManager.readyToUnityInstance();
    }

    public void printLog(string _log)
    {
        Debug.Log(_log);
    }

    public void reqConnectWallet()
    {
        mContractManager.StartCoroutine(progConnectWallet());
    }

    private IEnumerator progConnectWallet()
    {
        yield return new WaitForSeconds(0.5f);

        bool serverBlocked = false;
        bool versionMismatched = false;
        bool walletConnectFailed = false;

        int errCode = Const.NO_ERROR;
        if (serverBlocked)
        {
            errCode = Const.ERR_SERVER_BLOCKED;
        }
        else if (versionMismatched)
        {
            errCode = Const.ERR_VERSION_MISMATCHED;
        }
        else if (walletConnectFailed)
        {
            errCode = Const.ERR_WALLET_CONNECTION_FAILED;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["addr"] = "0x0000000000000000";
        data["err"] = errCode;
        var values = JsonConvert.SerializeObject(data);
        Debug.Log("resConnectWallet() " + values);
        mContractManager.resConnectWallet(values);
    }

    public void reqLatestNotice()
    {
        mContractManager.StartCoroutine(progLatestNotice());
    }

    private IEnumerator progLatestNotice()
    {
        yield return new WaitForSeconds(0.5f);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["title"] = "Welcome";
        data["date"] = 0;
        data["contents"] = "강인한 오크도, 지혜로운 엘프도,\n강인한 인간도, 용감한 다크엘프도,\n우리의 고향 [루그란디스]에 오신 것을\n진심으로 환영합니다.\n\n이곳은 영광의 땅 [루그란디스]\n\n이 문은 채굴, 통치, 반란, 교류, 탐험을 할 수 있는\n영광의 땅 [루그란디스]로 이어집니다.\n\n다시 돌아온 강인하고, 지혜롭고,\n용감하고, 현명한 모험가여\n레이나의 축복이\n부디 그대들과 함께하기를 바랍니다.";
        var values = JsonConvert.SerializeObject(data);
        Debug.Log("resLatestNotice() " + values);
        mContractManager.resLatestNotice(values);
    }

    public void reqLoginInfomation(string addr)
    {
        mContractManager.StartCoroutine(progLoginInfomation(addr));
    }

    private IEnumerator progLoginInfomation(string addr)
    {
        yield return new WaitForSeconds(0.5f);

        bool userBanned = false;
        bool noCharacter = false;
        bool hasUserData = true;
        bool tokenUsing = true;
        bool nftUsing = true;

        Dictionary<string, object> data = new Dictionary<string, object>();

        int errCode = Const.NO_ERROR;
        if (userBanned)
        {
            errCode = Const.ERR_USER_BANNED;
            data["startBlock"] = 123456789;
            data["endBlock"] = 987654321;
            data["reason"] = "도배";
        }
        else if (noCharacter)
        {
            errCode = Const.ERR_NO_CHARACTER;
        }

        if (hasUserData)
        {
            data["hasUserData"] = true;
            data["nickName"] = "Crow";
            data["termsVersion"] = 1;
            data["friends"] = new string[] { "0x123456789", "0x123456789", "0x123456789", "0x123456789", "0x123456789" };

            data["tokenUsing"] = tokenUsing;
            data["nftUsing"] = nftUsing;
        }
        else
        {
            data["hasUserData"] = false;
        }

        data["err"] = errCode;
        var values = JsonConvert.SerializeObject(data);
        Debug.Log("resLoginInfomation() " + values);
        mContractManager.resLoginInfomation(values);

        /*
        data["characterCount"] = 100;

        int[] characterList = new int[100];
        for (int i = 0; i < 100; i++)
        {
            data.Clear();
            data["progress"] = i;
            values = JsonConvert.SerializeObject(data);
            mContractManager.resFindingCharacter(values);
            yield return new WaitForSeconds(0.02f);
            characterList[i] = i;
        }

        data.Clear();
        data["characterIdList"] = characterList;
        values = JsonConvert.SerializeObject(data);
        mContractManager.resFoundCharacter(values);
        */
    }

    public void reqAgreeTerms(int _ver)
    {
        mContractManager.StartCoroutine(progAgreeTerms(_ver));
    }

    private IEnumerator progAgreeTerms(int _ver)
    {
        yield return new WaitForSeconds(0.5f);

        mContractManager.resAgreeTerms(null);
    }

    public void reqUsingToken()
    {
        mContractManager.StartCoroutine(progUsingToken());
    }

    private IEnumerator progUsingToken()
    {
        yield return new WaitForSeconds(0.5f);

        mContractManager.resUsingToken(null);
    }

    public void reqUsingNFT()
    {
        mContractManager.StartCoroutine(progUsingNFT());
    }

    private IEnumerator progUsingNFT()
    {
        yield return new WaitForSeconds(0.5f);

        mContractManager.resUsingNFT(null);
    }

    public void reqCheckRedundancy(string _nickname)
    {
        mContractManager.StartCoroutine(progCheckRedundancy(_nickname));
    }

    private IEnumerator progCheckRedundancy(string _nickname)
    {
        yield return new WaitForSeconds(0.5f);

        bool available = true;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["available"] = available;
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resCheckRedundancy(values);
    }

    public void reqCreateUser(string _nickname, int _ver)
    {
        mContractManager.StartCoroutine(progCreateUser(_nickname));
    }

    private IEnumerator progCreateUser(string _nickname)
    {
        yield return new WaitForSeconds(0.5f);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["nickname"] = _nickname;
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resCreateUser(values);
    }

    public void reqCoinAmount()
    {
        mContractManager.StartCoroutine(progCoinAmount());
    }

    int coinAdditional = 0;
    private IEnumerator progCoinAmount()
    {
        yield return new WaitForSeconds(0.5f);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["amount"] = BigInteger.Add(BigInteger.Parse("1234567800000000000000"), BigInteger.Multiply(BigInteger.Parse((coinAdditional++).ToString()), BigInteger.Parse("1000000000000000000")));
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resCoinAmount(values);
    }

    List<int> characterIdList = new List<int>();
    List<int> newCharacterIdList = new List<int>();
    List<int> stakingIdList = new List<int>();
    public void reqCharacterCount()
    {
        mContractManager.StartCoroutine(progCharacterCount());
    }

    private IEnumerator progCharacterCount()
    {
        yield return new WaitForSeconds(0.5f);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["characterCount"] = characterIdList.Count;
        data["stakingCharacterCount"] = stakingIdList.Count;
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resCharacterCount(values);
    }

    public void reqCharacterList(int _characterCount)
    {
        mContractManager.StartCoroutine(progCharacterList(_characterCount));
    }

    private IEnumerator progCharacterList(int _characterCount)
    {
        yield return new WaitForSeconds(0.5f);

        int[] characterList = new int[characterIdList.Count];
        for (int i = 0; i < characterList.Length; i++)
        {
            yield return new WaitForSeconds(0.02f);
            characterList[i] = characterIdList[i];
        }

        int[] stakingCharacterList = new int[stakingIdList.Count];
        for (int i = 0; i < stakingCharacterList.Length; i++)
        {
            yield return new WaitForSeconds(0.02f);
            stakingCharacterList[i] = stakingIdList[i];
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["characterIdList"] = characterList;
        data["stakingCharacterIdList"] = stakingCharacterList;
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resCharacterList(values);
    }

    public void reqNotInitCharacterList()
    {
        mContractManager.StartCoroutine(progNotInitCharacterList());
    }

    private IEnumerator progNotInitCharacterList()
    {
        yield return new WaitForSeconds(0.5f);

        int randCount = UnityEngine.Random.Range(0, 100);
        for (int i = 0; i < randCount; i++)
        {
            int id = UnityEngine.Random.Range(0, 10000);
            if (!characterIdList.Contains(id) && !stakingIdList.Contains(id))
            {
                newCharacterIdList.Add(id);
            }
        }

        int[] characterList = new int[newCharacterIdList.Count];
        for (int i = 0; i < characterList.Length; i++)
        {
            yield return new WaitForSeconds(0.02f);
            characterList[i] = newCharacterIdList[i];
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["characterIdList"] = characterList;
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resNotInitCharacterList(values);
    }

    public void reqInitCharacter(int[] _idList, int[] _characterDataList, int[] _statusDataList, int[] _equipDataList)
    {
        mContractManager.StartCoroutine(progInitCharacter(_idList, _characterDataList, _statusDataList, _equipDataList));
    }

    private IEnumerator progInitCharacter(int[] _idList, int[] _characterDataList, int[] _statusDataList, int[] _equipDataList)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < newCharacterIdList.Count; i++)
        {
            yield return new WaitForSeconds(0.02f);

            int seed = UnityEngine.Random.Range(0, 3);

            if (seed == 0)
            {
                characterIdList.Add(newCharacterIdList[i]);
            } else if (seed == 1)
            {
                stakingIdList.Add(newCharacterIdList[i]);
            }
        }

        newCharacterIdList.Clear();

        mContractManager.resInitCharacter("");

    }
    public void reqCharacterData(int[] _characterIdList)
    {
        mContractManager.StartCoroutine(progCharacterData(_characterIdList));
    }

    private IEnumerator progCharacterData(int[] _characterIdList)
    {
        yield return new WaitForSeconds(0.3f);

        foreach (int id in _characterIdList)
        {
            yield return new WaitForSeconds(0.02f);

            Dictionary<string, object> data = new Dictionary<string, object>();

            Dictionary<string, object> characterData = new Dictionary<string, object>();
            characterData["name"] = "Tale of Raynor #" + id.ToString("0000");
            characterData["tokenId"] = id;
            characterData["level"] = UnityEngine.Random.Range(1, 10);
            characterData["exp"] = 0;
            characterData["country"] = UnityEngine.Random.Range(0, 5);
            characterData["race"] = UnityEngine.Random.Range(0, 5);
            characterData["job"] = UnityEngine.Random.Range(0, 9);
            characterData["statusBonus"] = 0;
            characterData["version"] = 1;

            data["characterData"] = JsonConvert.SerializeObject(characterData);

            Dictionary<string, object> statusData = new Dictionary<string, object>();
            statusData["att"] = UnityEngine.Random.Range(50, 500);
            statusData["def"] = UnityEngine.Random.Range(50, 500);

            data["statusData"] = JsonConvert.SerializeObject(statusData);

            Dictionary<string, object> equipData = new Dictionary<string, object>();
            equipData["weapon"] = UnityEngine.Random.Range(0, 4);
            equipData["armor"] = UnityEngine.Random.Range(0, 4);
            equipData["pants"] = UnityEngine.Random.Range(0, 4);
            equipData["head"] = UnityEngine.Random.Range(0, 4);
            equipData["shoes"] = UnityEngine.Random.Range(0, 4);
            equipData["accessory"] = UnityEngine.Random.Range(0, 6);

            data["equipData"] = JsonConvert.SerializeObject(equipData);

            var outValue = JsonConvert.SerializeObject(data);

            mContractManager.resCharacterData(outValue);
        }
    }

    public void reqStakingData(int _count)
    {
        mContractManager.StartCoroutine(progStakingData(_count));
    }

    private IEnumerator progStakingData(int _count)
    {
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < _count; i++)
        {
            yield return new WaitForSeconds(0.03f);

            Dictionary<string, object> data = new Dictionary<string, object>();
            data["id"] = stakingIdList[i];
            data["startBlock"] = 12345678;
            data["endBlock"] = 0;
            data["purpose"] = 4;
            var outValue = JsonConvert.SerializeObject(data);

            mContractManager.resStakingData(outValue);
        }
    }
}