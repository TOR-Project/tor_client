using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

public class EditorContractCommunicator : IContractCommunicator
{
    private ContractManager mContractManager;

    public EditorContractCommunicator(ContractManager cm)
    {
        mContractManager = cm;
        mContractManager.readyToUnityInstance();
    }

    public void printLog(string log)
    {
        Debug.Log(log);
    }

    public void reqConnectWallet()
    {
        mContractManager.StartCoroutine(progConnectWallet());
    }

    private IEnumerator progConnectWallet()
    {
        yield return new WaitForSeconds(1);

        bool serverBlocked = false;
        bool versionMismatched = false;
        bool walletConnectFailed = false;

        int errCode = 0;
        if (serverBlocked)
        {
            errCode = 1;
        } else if (versionMismatched)
        {
            errCode = 2;
        } else if (walletConnectFailed)
        {
            errCode = 4;
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
        yield return new WaitForSeconds(1);

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
        yield return new WaitForSeconds(1);

        bool userBanned = false;
        bool noCharacter = false;
        bool hasUserData = false;
        bool tokenUsing = true;
        bool nftUsing = true;

        Dictionary<string, object> data = new Dictionary<string, object>();

        int errCode = 0;
        if (userBanned)
        {
            errCode = 5;
            data["startBlock"] = 123456789;
            data["endBlock"] = 987654321;
            data["reason"] = "도배";
        }
        else if (noCharacter)
        {
            errCode = 6;
        }

        if (hasUserData)
        {
            data["hasUserData"] = true;
            data["nickName"] = "Crow";
            data["termsVersion"] = 1;
            data["friends"] = new string[] {"0x123456789", "0x123456789", "0x123456789", "0x123456789", "0x123456789" };

            data["tokenUsing"] = tokenUsing;
            data["nftUsing"] = nftUsing;
        } else
        {
            data["hasUserData"] = false;
        }
        
        data["err"] = errCode;
        data["characterCount"] = 100;
        var values = JsonConvert.SerializeObject(data);
        Debug.Log("resLoginInfomation() " + values);
        mContractManager.resLoginInfomation(values);

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

    }
}