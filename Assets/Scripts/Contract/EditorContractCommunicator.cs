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

    public void reqConnectWallet()
    {
        mContractManager.StartCoroutine(progConnectWallet());
    }

    private IEnumerator progConnectWallet()
    {
        yield return new WaitForSeconds(1);

        bool serverBlocked = false;
        bool versionMismatched = false;
        bool networkMismatched = false;
        bool walletConnectFailed = false;

        int errCode = 0;
        if (serverBlocked)
        {
            errCode = 1;
        } else if (versionMismatched)
        {
            errCode = 2;
        } else if (networkMismatched)
        {
            errCode = 3;
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
        yield return new WaitForSeconds(3);

        bool userBanned = false;
        bool noCharacter = false;

        int errCode = 0;
        if (userBanned)
        {
            errCode = 5;
        }
        else if (noCharacter)
        {
            errCode = 6;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["userInfo"] = "null";
        data["characterIdList"] = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        data["err"] = errCode;
        var values = JsonConvert.SerializeObject(data);
        Debug.Log("resLoginInfomation() " + values);
        mContractManager.resLoginInfomation(values);
    }

}