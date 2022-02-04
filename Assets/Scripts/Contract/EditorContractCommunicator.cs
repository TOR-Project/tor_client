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
        mContractManager.readyToContract("notice");
    }

    public void printLog(string _log)
    {
        Debug.Log(_log);
    }

    long blockNumber = 79621981;
    public void reqBlockNumber()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["block"] = blockNumber;
        blockNumber += 60;
        var values = JsonConvert.SerializeObject(data);
        mContractManager.resBlockNumber(values);
    }

    public void reqConnectedWalletAddr()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = "0x0000000000000000";
        var values = JsonConvert.SerializeObject(data);
        mContractManager.resConnectedWalletAddr(values);
    }

    public void reqServerState()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["available"] = true;
        var values = JsonConvert.SerializeObject(data);
        mContractManager.resServerState(values);
    }

    public void reqConnectWallet()
    {
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
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["title"] = "Welcome";
        data["date"] = 0;
        data["contents"] = "강인한 오크도, 지혜로운 엘프도,\n현명한 인간도, 용감한 다크엘프도,\n우리의 고향 [루그란디스]에 오신 것을\n진심으로 환영합니다.\n\n이곳은 영광의 땅 [루그란디스]\n\n이 문은 채굴, 통치, 반란, 교류, 탐험을 할 수 있는\n영광의 땅 [루그란디스]로 이어집니다.\n\n다시 돌아온 강인하고, 지혜롭고,\n용감하고, 현명한 모험가여\n레이나의 축복이\n부디 그대들과 함께하기를 바랍니다.";
        var values = JsonConvert.SerializeObject(data);
        Debug.Log("resLatestNotice() " + values);
        mContractManager.resLatestNotice(values);
    }

    public void reqLoginInfomation(string addr)
    {
        bool userBanned = false;
        bool noCharacter = false;
        bool hasUserDataLegacy = true;
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

        if (hasUserDataLegacy)
        {
            data["nickNameLegacy"] = "Crow";
            data["termsVersionLegacy"] = 1;
        }

        if (hasUserData)
        {
            data["nickName"] = "Crow";
            data["termsVersion"] = 1;
            data["friends"] = new string[] { "0x123456789", "0x123456789", "0x123456789", "0x123456789", "0x123456789" };
        }

        data["hasUserDataLegacy"] = hasUserDataLegacy;
        data["hasUserData"] = hasUserData;
        data["tokenUsing"] = tokenUsing;
        data["nftUsing"] = nftUsing;

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
        mContractManager.resAgreeTerms(null);
    }

    public void reqUsingToken()
    {
        mContractManager.resUsingToken(null);
    }

    public void reqUsingNFT()
    {
        mContractManager.resUsingNFT(null);
    }

    public void reqCheckRedundancy(string _nickname)
    {
        bool available = true;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["available"] = available;
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resCheckRedundancy(values);
    }

    public void reqRegistTokenToWallet()
    {
        mContractManager.resRegistTokenToWallet("");
    }

    public void reqCreateUser(string _nickname, int _ver)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["nickname"] = _nickname;
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resCreateUser(values);
    }

    public void reqCoinAmount()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["amount"] = BigInteger.Parse("1234567800000000000000");
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resCoinAmount(values);
    }

    List<CharacterData> characterListArchive = new List<CharacterData>();
    List<CharacterData> newCharacterListArchive = new List<CharacterData>();
    List<CharacterData> stakingListArchive = new List<CharacterData>();
    Dictionary<int, StakingData> stakingDataArchive = new Dictionary<int, StakingData>();
    Dictionary<int, CharacterData> allCharacterMapArchive = new Dictionary<int, CharacterData>();

    private void initCharacterArchive()
    {
        if (characterListArchive.Count > 0 || newCharacterListArchive.Count > 0 || stakingListArchive.Count > 0)
        {
            return;
        }

        List<int> idCheckList = new List<int>();
        int randCount = UnityEngine.Random.Range(300, 1000);
        for (int i = 0; i < randCount; i++)
        {
            int id = UnityEngine.Random.Range(0, 10000);
            if (idCheckList.Contains(id))
            {
                continue;
            }
            idCheckList.Add(id);

            CharacterData data = getRandomCharacterData(id);

            int whichArr = UnityEngine.Random.Range(0, 3);
            if (whichArr == 0)
            {
                characterListArchive.Add(data);
            } else if (whichArr == 1)
            {
                newCharacterListArchive.Add(data);
            } else
            {
                stakingListArchive.Add(data);
                StakingData stakingData = new StakingData();
                stakingData.tokenId = data.tokenId;
                stakingData.startBlock = 123456789;
                stakingData.endBlock = 0;
                stakingData.purpose = StakingManager.PURPOSE_MINING;
                stakingDataArchive.Add(data.tokenId, stakingData);
            }

            allCharacterMapArchive.Add(id, data);
        }
    }

    private CharacterData getRandomCharacterData(int _id)
    {
        CharacterData data = new CharacterData();
        data.name = "#" + _id.ToString("0000");
        data.tokenId = _id;
        data.level = UnityEngine.Random.Range(1, 10);
        data.exp = 0;
        data.country = UnityEngine.Random.Range(0, 5);
        data.race = UnityEngine.Random.Range(0, 5);
        data.job = UnityEngine.Random.Range(1, 9);
        data.statusBonus = 0;
        data.version = 1;

        data.statusData.att = UnityEngine.Random.Range(50, 500);
        data.statusData.def = UnityEngine.Random.Range(50, 500);

        data.equipData.weapon = UnityEngine.Random.Range(0, 4);
        data.equipData.armor = UnityEngine.Random.Range(0, 4);
        data.equipData.accessory = UnityEngine.Random.Range(0, 4);
        data.equipData.pants = UnityEngine.Random.Range(0, 4);
        data.equipData.head = UnityEngine.Random.Range(0, 4);
        data.equipData.shoes = UnityEngine.Random.Range(0, 4);

        return data;
    }

    public void reqCharacterCount()
    {
        initCharacterArchive();

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["characterCount"] = characterListArchive.Count;
        data["stakingCharacterCount"] = stakingListArchive.Count;
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resCharacterCount(values);
    }

    public void reqCharacterList(int _characterCount)
    {
        int[] characterList = new int[characterListArchive.Count];
        for (int i = 0; i < characterList.Length; i++)
        {
            characterList[i] = characterListArchive[i].tokenId;
        }

        int[] stakingCharacterList = new int[stakingListArchive.Count];
        for (int i = 0; i < stakingCharacterList.Length; i++)
        {
            stakingCharacterList[i] = stakingListArchive[i].tokenId;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["characterIdList"] = characterList;
        data["stakingCharacterIdList"] = stakingCharacterList;
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resCharacterList(values);
    }

    public void reqNotInitCharacterList()
    {
        initCharacterArchive();

        int[] characterList = new int[newCharacterListArchive.Count];
        for (int i = 0; i < characterList.Length; i++)
        {
            characterList[i] = newCharacterListArchive[i].tokenId;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["characterIdList"] = characterList;
        var values = JsonConvert.SerializeObject(data);

        mContractManager.resNotInitCharacterList(values);
    }

    public void reqInitCharacter(int[] _idList, int[] _characterDataList, int[] _statusDataList, int[] _equipDataList)
    {
        for (int i = 0; i < newCharacterListArchive.Count; i++)
        {
            characterListArchive.Add(newCharacterListArchive[i]);
        }

        newCharacterListArchive.Clear();

        mContractManager.resInitCharacter("");
    }

    public void reqCharacterData(int[] _characterIdList)
    {
        foreach (int id in _characterIdList)
        {
            CharacterData cData;
            if (allCharacterMapArchive.ContainsKey(id))
            {
                cData = allCharacterMapArchive[id];
            } else
            {
                cData = getRandomCharacterData(id);
                allCharacterMapArchive.Add(id, cData);
            }

            Dictionary<string, object> data = new Dictionary<string, object>();

            Dictionary<string, object> characterData = new Dictionary<string, object>();
            characterData["name"] = cData.name;
            characterData["tokenId"] = cData.tokenId;
            characterData["level"] = cData.level;
            characterData["exp"] = cData.exp;
            characterData["country"] = cData.country;
            characterData["race"] = cData.race;
            characterData["job"] = cData.job;
            characterData["statusBonus"] = cData.statusBonus;
            characterData["version"] = cData.version;

            data["characterData"] = JsonConvert.SerializeObject(characterData);

            Dictionary<string, object> statusData = new Dictionary<string, object>();
            statusData["att"] = cData.statusData.att;
            statusData["def"] = cData.statusData.def;

            data["statusData"] = JsonConvert.SerializeObject(statusData);

            Dictionary<string, object> equipData = new Dictionary<string, object>();
            equipData["weapon"] = cData.equipData.weapon;
            equipData["armor"] = cData.equipData.armor;
            equipData["pants"] = cData.equipData.pants;
            equipData["head"] = cData.equipData.head;
            equipData["shoes"] = cData.equipData.shoes;
            equipData["accessory"] = cData.equipData.accessory;

            data["equipData"] = JsonConvert.SerializeObject(equipData);

            var outValue = JsonConvert.SerializeObject(data);

            mContractManager.resCharacterData(outValue);
        }
    }

    public void reqStakingData(int[] _idList)
    {
        foreach (int id in _idList)
        {
            StakingData stakingData = stakingDataArchive[id];

            Dictionary<string, object> data = new Dictionary<string, object>();
            data["id"] = stakingData.tokenId;
            data["startBlock"] = stakingData.startBlock;
            data["endBlock"] = stakingData.endBlock;
            data["purpose"] = stakingData.purpose;
            var outValue = JsonConvert.SerializeObject(data);

            mContractManager.resStakingData(outValue);
        }
    }

    public void reqAddMiningStaking(int[] _idList)
    {
        mContractManager.resAddMiningStaking("");
    }

    public void reqGetBackMiningStaking(int[] _idList)
    {
        mContractManager.resGetBackMiningStaking("");
    }

    public void reqReceiveMiningAmount(int[] _idList, string[] _countryTax, string _finalAmount, string _commissionAmount, int _password)
    {
        mContractManager.resReceiveMiningAmount("");
    }

    public void reqCalculateMiningAmount(int _id)
    {
        BigInteger basicAmount = BigInteger.Parse("1000000000000000000") * 10;
        BigInteger miningTaxAmount = -BigInteger.Parse("1000000000000000000") * 0;
        BigInteger countryAmount = BigInteger.Parse("1000000000000000000") * 0;
        BigInteger rebellionAmount = BigInteger.Parse("1000000000000000000") * 0;
        BigInteger earlybirdAmount = BigInteger.Parse("1000000000000000000") * 0;
        BigInteger accountReceivableAmount = BigInteger.Parse("1000000000000000000") * 0;
        BigInteger commissionAmount = -(basicAmount + miningTaxAmount + countryAmount + rebellionAmount + earlybirdAmount) / 10;
        BigInteger finalAmount = (basicAmount + miningTaxAmount + countryAmount + rebellionAmount + earlybirdAmount + commissionAmount);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["tokenId"] = _id;
        data["basicAmount"] = basicAmount;
        data["miningTaxAmount"] = miningTaxAmount;
        data["countryAmount"] = countryAmount;
        data["rebellionAmount"] = rebellionAmount;
        data["earlybirdAmount"] = earlybirdAmount;
        data["accountReceivableAmount"] = accountReceivableAmount;
        data["commissionAmount"] = commissionAmount;
        data["finalAmount"] = finalAmount;
        var value = JsonConvert.SerializeObject(data);

        mContractManager.resCalculateMiningAmount(value);
    }

    public void reqGetPassword()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["password"] = 123456;
        var value = JsonConvert.SerializeObject(data);

        mContractManager.resGetPassword(value);
    }

    public void reqGetStorySummery(int _id)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["id"] = _id;
        if (_id == 0)
        {
            data["mainTitle"] = "Prologue";
        } else if (_id == 1)
        {
            data["mainTitle"] = "지팡이를 든 왕 (1)";
        } else
        {
            data["mainTitle"] = "지팡이를 든 왕 (2)";
        }
        data["subTitle"] = "";
        data["mainCategory"] = 0;
        data["subCategory"] = _id;
        data["thumbnailUrl"] = "";
        data["freeBlock"] = _id >= 1 ? 1 : 0; // 0 free, 1 paid, 2 paid
        data["isSubscribed"] = _id == 1; // 0 false, 1 true, 2 false
        var value = JsonConvert.SerializeObject(data);

        mContractManager.resGetStorySummery(value);
    }

    public void reqGetStoryCount()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["count"] = 3;
        var value = JsonConvert.SerializeObject(data);

        mContractManager.resGetStoryCount(value);
    }

    public void reqSubscribeStory(int _id)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["id"] = _id;
        data["success"] = true;
        var value = JsonConvert.SerializeObject(data);

        mContractManager.resSubscribeStory(value);
    }

    public void reqGetStoryDataFull(int _id)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["id"] = _id;
        data["success"] = true;
        if (_id == 0)
        {
            data["contents"] = "이 땅의 이름은 ‘루그란디스’였다. 세상을 창조한 신의 직접 붙인 이름이라고 말하지만 이 땅에 사는 어느 누구도 그 이름으로 이 땅을 부르지 않았다.\n\n대륙의 패권을 나눠 가진 인간, 엘프, 오크 세 종족은 영토를 나눠 가진 후 그 땅에 서로 다른 이름을 붙였다.\n\n인간들의 개발로 숲과 들에 성벽과 도시가 들어선 트리폴리.\n\n엘프들이 자연과 조화를 이루며 살아가는 숲과 신비가 가득한 엔필리스.\n\n오크들이 생존을 위해 투쟁하는 척박한 헬베스타와 바르바로스.\n\n모든 종족들이 하나의 대륙에는 하나의 이름만 필요하다 생각했다. 그들은 이 땅의 다른 이름을 없애버리기 위해 서로를 죽였다.\n\n한 뼘 남짓한 땅조차 한 달에 수십번은 이름이 바뀌었다.\n\n오랜 시간 계속된 전쟁에 루그란디아라는 이름은 사람들의 머릿속에서 잊혀지고 대지는 시체 밑에서 썩어 들어가던 시대에, 단 한 사람만이 이 땅을 다시 ‘루그란디스’라고 불렀다.\n\n레이나라는 인간 소년였다. 그는 신에게 직접 이 땅의 이름을 들었다고 했다. 이 땅의 이름을 루그란디아로 되돌리라는 계시와 함께.\n\n신의 계시를 받았다는 그의 힘은 정말 기적이라고 밖에 말할 수 없었다. 그가 축복을 내리자 시체와 함께 썩어가던 땅은 생기를 되찾고 피로 물든 강은 다시 맑은 물이 흘렀다. 그는 손길만으로 병과 상처를 치료했다.\n\n그는 말 한마디로 전쟁을 멈추었다. 대륙 중앙을 중립지대로 두어 그 어느 종족도 서로 국경을 접하지 못하도록 만들었다. 그의 결정이 신의 의지였고 모든 종족들은 그의 결정에 따를 수 밖에 없었다.\n\n하지만 전쟁은 끝났어도 루그란디스라는 이름은 돌아오지 않았다. 그가 축복을 내린 후 중립 지대로 만든 땅은 사람들 사이에서 ‘에브제니스’라는 또 다른 이름으로 불렸다. 정작 축복을 내린 레이나는 그 땅을 여전히 루그란디스라라고 불렀다.\n\n그러나 레이나는 오늘 죽었으니 루그란디스라는 이름도 그와 함께 사라지겠지.\n\n\n***\n\n\n세 종족은 화합을 위해 에브제니스에서는 1년에 한 번 공의회가 열렸다. 에브제니스의 중심부에는 이름 모를 신을 모시던 오래된 신전이 있었고 그 안에는 원탁이 있었다.\n\n평등과 공정함을 의미하는 원탁에서 새 종족은 자유롭게 서로의 입장을 드러낼 수 있었다. 그러나 평등과 공정함이란 곧 누군가에게는 양보를 의미했다. 그리고 양보를 좋아하는 이는 없었다.\n\n\n***\n\n\n엘프들의 여왕인 이브노바는 의자에 앉은 채 죽어있는 레이나를 멍하니 바라 보았다.\n\n레이나는 이마가 관통된 채 고개를 숙이고 있었다. 그의 타는 듯 밝던 금발은 흐트러지고 생명력 넘치던 푸른 눈동자는 잿빛으로 변해 있었다.\n\n그를 죽인 오크의 손에는 손바닥만한 석궁이 들려 있었다. 사람의 머리를 관통할 정도로 크고 강한 것이었지만 거대한 오크의 손 안에는 숨길 수 있는 크기였다.\n\n이번 공의회는 처음부터 분위기가 심상치 않았다. 평화에 불만을 가진 오크가 일을 저지를 것이란 소문이 전 대륙에 퍼졌고 공의회가 취소될 것이란 말도 함께 돌았다.\n\n그러나 이브노바는 참가를 만류하는 신하들을 뿌리치고 공의회에 참가했다. 레이나가 참가했기 때문이었다.\n\n이브노바는 아무리 위험한 곳이라 해도 그가 있다면 괜찮을 것이라 생각했다. 왜냐하면 레이나는 신의 사자였으니까, 그런 자가 죽는 것은 상상할 수 없었다. 정확히는 인간의 육체를 가진 이상 언젠가는 죽겠지만, 평범하게 누워서 죽는 것이 아니라 하늘로 승천이라도 할 것이라 상상했다.\n\n그런데 너무나 쉽게 죽어버렸다.\n\n성자, 대현자, 구세주. 그를 칭하는 수 많은 칭호에 어울리지 않는 소박한 죽음이었다.\n\n그리 생각하는 것은 그를 죽인 오크도 마찮가지인 모양이었다. 그는 자리에서 일어선 채 앞으로 튀어나가려는 듯 어정쩡한 자세로 굳어 있었다. 신의 사자를 죽이기 위해 온갖 준비를 해 왔지만 그는 단 한 수만에 죽어버렸다.\n\n잠깐의 무거운 적막이 흐른 후, 이브노바가 오크를 향해 손바닥을 폈다. 그녀의 손에서 오크를 향해 거대한 불덩이가 쏘아졌다. 불은 그가 있던 자리를 집어 삼키고는 그가 있던 방향의 돌벽 마저 검게 그을려 버린 후 사라졌다.\n\n오크는 흔적도 남기지 않고 사라졌다. 정확히는 그의 육체만 사라졌을 뿐 다른 흔적은 남아 있었다. 찢어지고 불에 탄 스크롤 조각이 붙티와 함께 공중에 휘날렸다.\n\n이브노바는 팔을 축 늘어트린 채 가만히 발 끝을 내려다 봤다.\n\n모든 것이 혼란스러웠다. 오크가 마법을 사용하는 것도 그랬고 그자가 자신의 눈을 피해 무기를 숨겨 들어온 것도 그랬다. 무엇보다 레이나의 죽음이 그랬다.\n\n인간의 대표이자 평화의 상징인 그가 죽었으니 인간은 오크와 전쟁을 준비할 것이다.\n\n전쟁을 바라는 오크의 탐욕이 평화를 바라는 신의 의지보다 강했다는 말인가. 그게 아니면, 전쟁이 신의 뜻인가.\n\n짧은 평화는 이렇게 막을 내렸다.\n";
        } 
        else if (_id == 1)
        {
            data["contents"] = "여러 종족이 살아가는 루그란디아의 중심부, 에브게니스 지방은 대현자 레이나가 중립 지대로 선포한 이후 공식적으로는 아무도 살지 않는 땅이었다. 따라서 이곳에 사는 자들은 공식적으로 세상에 존재하지 않았다.\n\n누구도 살지 않는 땅이다 보니 이곳은 문명의 바깥에 자리하고 있었다. 사회에서 누릴 수 있는 혜택이라고는 아무것도 없는 이 땅에 숨어들어 사는 존재들은 세상에서 사라지고 싶은 이들이거나 사라져야만 하는 이들이었다.\n\n이곳까지 흘러 들어온 이유는 저마다 달랐지만 단 한가지 공통점이 있다면 다들 언젠가는 이 땅을 벗어나고 싶어 한다는 점이었다.\n\n다들 이곳을 빠져나가 사회에 두고 온 자신의 본래 삶을 되찾고 싶어했다.\n\n그런 점에서 키안은 별종이라 말 할 수 있는 소년이었다. 그는 이곳에서 태어났고 이곳에서 자랐다.\n\n부모에 관한 기억은 별로 없었다. 자아가 생기고 난 뒤로 기억나는 일은 그들과 함께 하루종일 먹을 걸 찾으러 다닌 것뿐이었다. 그러다 어느 날 아버지는 뭘 먹고는 시름시름 앓다가 죽어 버렸고 어머니는 얼마 지나지 않아 날씨가 춥던 날 잠에 든 이후 다시는 일어나지 못했다.\n\n그 밖의 기억이라면 그들이 종종 나누던 대화 내용 따위였다. 둘의 대화에는 키안이 모르는 단어가 많이 등장했다.\n\n영지, 귀족, 왕. 의미는 몰랐지만 바깥에는 그런 것들이 있고 자신의 부모는 그런 것들을 잃어버린 듯했다. 그래서 이곳을 나가고 싶어했다.\n\n그러나 바깥 세상에 두고 온 것이 없는 키안에게는 이곳을 벗어나려는 열망도 없었다.\n\n그의 관심사는 오직 그날 하루를 덜 굶주리는 것뿐이었다.\n\n엘프 영토와 인접한 어느 숲 속에서 그는 지금, 죽은 나무 기둥에 자란 처음 보는 버섯을 보며 먹어도 될지 고민하는 중이었다.\n\n한참을 고민하던 키안은 결국 버섯을 향해 손을 뻗었다.\n\n“그거 먹으면 죽어.”\n\n등 뒤에서 젊은 여자의 목소리가 들렸다. 반사적으로 뒤를 돌아보자 그곳에는 뾰족한 귀를 가진 여자가 서 있었다. 얼굴만 보면 아직 소녀 정도로 보였지만 키는 꽤 커서 성인 여자와 비슷한 정도였다.\n\n옷차림은 긴 바지에 헐렁한 후드 원피스를 입었는데 위 아래가 모두 검은색이었다. 처음에 옷만 봤을 때는 여행자인가 싶었다. 여행자란 가끔 이 근처를 지나가는 부류의 족속으로 키안 같은 자들을 만나면 먹다 남긴 걸 주고 가는 등 도움이 되긴 하는 존재들이었다.\n\n하지만 흰 피부와 윤기가 흐르는 은발을 보아하니, 자신의 두 다리로 밖을 돌아다닐 유형의 인물 같아 보이지는 않았다. 저런 생김새는 마차를 타고 다니는 자들에게 흔히 보였는데 그들은 여행자들과는 달리 키안을 보면 욕지거리만 내뱉고 사라지는 자들이었다.\n\n둘 중 어느 쪽인지는 모르겠지만 얼굴에 굶주린 기색이 없는 걸 보니 이곳에 사는 사람은 아니란 건 확실했다.\n\n소녀는 땅에 자기 키만한 지팡이를 짚고 서있는데 제대로 된 지팡이라기 보단 마치 커다란 나뭇가지를 꺾어 그대로 지팡이로 삼은 모양새였다.\n\n“내가 살려 줬지? 고맙다고 해.”\n\n소녀가 의기양양한 표정으로 말했다. 그녀의 당당한 태도에 키안의 입에서 반사적으로 대답이 튀어나왔다.\n\n“어? 어. 고마워.”\n\n“이 정도로 뭘.”\n\n소녀는 싱긋 웃으며 어깨를 으쓱했다. 그 모습을 잠시 바라보던 키안은 그녀를 내버려 둔 채 발걸음을 옮겼다. 기껏 찾아낸 버섯이 식용 불가 판정을 받았으니 서둘러 다른 먹거리를 찾아야 했다.\n\n대충 주변을 두리번 거리자 근처 땅바닥에 열매를 맺고 있는 식물이 보였다. 알이 굵고 빨간 것이 어쩐지 먹음직스럽게 보였다. 그 앞에 쭈그려 앉아 자세히 살펴보고 있을 때 머리 위에서 또다시 목소리가 들려왔다.\n\n“아, 이건 방울뱀딸기야. 먹어도 죽진 않지만 배가 죽을 만큼 아플 걸?”\n\n은발의 소녀는 어느새 뒤를 따라와 허리를 숙인 채 그의 어깨 너머로 고개를 내밀고 있었다. 키안은 반사적으로 말소리가 난 쪽으로 고개를 돌렸다. 소녀의 얼굴은 그의 코 끝이 그녀의 뺨에 닿을 듯 바로 옆에 있었다.\n\n그녀의 얼굴이 그의 생각보다 가까이 있었는지 키안의 어깨가 움찔거리며 그의 자세가 옆으로 무너졌다.\n\n“그건 나도 알아.”\n\n키안은 엉거주춤한 자세로 일어서며 대답했다. 소녀도 숙이고 있던 허리를 천천히 폈다.\n\n“내가 또 도와줬네?”\n\n소녀는 발랄한 미소를 띄운 채 무언가 할 말이 있지 않냐는 듯 키안을 빤히 쳐다봤다. 하지만 키안은 의미를 모르겠다는 듯 눈썹 하나를 치켜 올린 표정으로 그녀의 얼굴을 가만히 바라보고만 있었다.\n\n둘 사이에 잠깐 침묵이 흐른 후 키안이 먼저 입을 열었다.\n\n“나도 이건 먹으면 안 되는 거 알아. 옛날에 먹어봤으니까.”\n\n그가 고개를 돌리며 대답하는 순간 그녀의 입에서 ‘풉’ 하고 작은 웃음소리가 터져 나왔다. 이건 또 무슨 의미인가 싶어 다시 그녀를 돌아봤을 때는 소녀는 웃음을 참는 듯 다른 쪽으로 고개를 돌리고 있었다. 그녀의 입가가 한 쪽만 올라가 있는 것이 키안은 왠지 기분이 나빴다.\n\n눈동자만 움직여 자신을 흘겨 보는 소녀를 내버려두고 키안은 재빨리 그곳을 벗어났다.\n\n다시 먹을 만한 걸 찾아 숲을 돌아다니는 내내 그의 뒤에서 발소리 하나가 자꾸만 따라왔다.\n\n그 소리가 신경 쓰여 멈춰 서 뒤를 돌아보면 뾰족 귀를 가진 소녀가 서 있었다. 그녀는 그와 눈이 마주칠 때면 휘파람을 불며 주변 풍경을 이리저리 둘러봤다.\n\n“왜 따라 오는 거야?”\n\n그의 질문에 소녀가 뚱딴지 같은 소리를 다 듣는다는 표정을 지으며 고개를 갸웃거렸다.\n\n“따라 간 적 없는데? 나는 내가 갈 길을 가는 거야.”\n\n“어디로 가는 건데?”\n\n“어딜 가려는 건 아니고······. 그냥 찾는 게 좀 있어서.”\n\n“뭘 찾는데?”\n\n“알려주면 찾는 거 네가 도와 줄 거야?”\n\n키안은 조용히 고개를 돌렸다. 오늘 먹을 것도 못 찾았는데 남의 사정 따윈 알 바 아니었다.\n\n한참을 돌아다니다 보니 가슴 높이 정도로 자란 덤불이 보였다. 어디서 본 기억이 있어 가까이 가자, 쪽빛 열매가 덤불 사이사이에 달려 있었다. 이건 먹을 수 있는 것이라 확신을 가지고 말 할 수 있었다. 비록 식감은 물컹하고 맛은 시큼하지만 어쨌든 먹을 수는 있었다.\n\n키안이 셔츠 밑단을 위로 올려 바구니 모양을 만든 후 열매를 가득 담기 시작했다. 그 모습을 지켜보던 소녀가 셔츠 위의 열매 더미에 불쑥 손을 뻗었다. 그리고는 손가락으로 이리저리 열매를 고르더니 한 알 씩 집어 땅에 버리기 시작했다.\n\n“뭐 하는 거야!”\n\n키안이 소스라치게 놀라며 소리쳤다. 그는 셔츠에 담은 열매가 쏟아질까 조심스레 그 자리에 앉으며 소녀가 버린 열매를 다시 주워 담았다.\n\n소녀는 그가 다시 주워 담은 열매를 다시 골라 내어 멀리 던져 버렸다.\n\n“이건 다 못 먹는 것들이야. 이건 덜 익은 거고, 이건 벌레가 속을 파먹었네. 가만히 좀 있어봐 내가 알아서 골라 줄 테니까.\n\n“다 먹을 수 있는 거야!”\n\n소녀는 짧게 한숨을 쉬며 열매 하나를 골라 키안의 입가에 가져다 댔다.\n\n“이건 네가 고른 거야.”\n\n키안은 버벅거리는 동작으로 열매를 입안에 넣었다. 씹으니 물컹한 식감과 시큼한 맛이 느껴졌다. 끝맛이 씁쓸한 것이 조금 거슬렸지만 어쨌든 씹고 삼키는 데는 문제 없었다.\n\n“맛없지? 이건 내가 고른 거야.”\n\n소녀가 다른 열매 하나를 다시 그의 입에 가져다 댔다.\n\n물컹한 것은 똑같았지만 시큼하지 않았다. 오히려 은은한 단맛과 새콤한 맛이 조화를 이루고 있었다.\n\n맛있었다.\n\n“어때?”\n\n소녀가 특유의 의기양양한 표정으로 물었다. 그러나 처음부터 그의 대답을 기다릴 생각은 없었던 모양이었다.\n\n“아니 대답하지 마. 표정만 봐도 알겠네.”\n\n키안은 입안에 남은 단맛을 음미하느라 잠시 뜸을 들인 후 대답했다.\n\n“다 먹을 수 있는 거야. 버릴 필요는 없잖아.”\n\n“먹을 수 있다고 다 먹으면 그게 사람이야? 짐승이지.”\n\n소녀는 기가 차다는 듯 코웃음을 쳤다.\n\n“근데 뭐 잊은 거 없어?”\n\n“응? 아······.”\n\n키안은 허리를 꼿꼿히 세운 체 앉으며 소녀가 버린 열매에 손을 뻗었다. 맛은 없지만 중요한 양식이다.\n\n“아니 그거 말고.”\n\n소녀는 그가 집어려던 열매를 먼저 집어들고는 아까보다 더 멀리 던져 버렸다. 눈으로 쫓을 수 없을 만큼 빠른 동작이었다.\n\n“도움을 받았으면 고맙다고 해야지. 인간들은 왜 이렇게 예의가 없어?”\n\n“아, 고마······.”\n\n“아냐, 됐어. 억지로 안 해도 돼. 그 정도로 힘든 일은 아니었으니까.”\n\n소녀는 가슴을 펴며 뿌듯한 표정을 지었다. 중간에 말이 잘린 탓에 키안의 입은 어중간하게 벌어진 채 굳어버렸다. 그 상태로 그의 눈이 두어번 꿈벅거렸다.\n\n소녀는 멍청해 보이는 표정을 짓고 있는 키안을 무시하고 그의 셔츠 위의 열매 더미에 눈길을 보냈다.\n\n“근데 그걸 혼자 다 먹으려고?”\n\n그녀의 물음에 키안은 입을 닫고 열매를 내려다 봤다. 그는 곧바로 고개를 들고 대답했다.\n\n“같이 나눠 먹을 거야.”\n\n“응? 나랑?”\n\n키안은 소녀를 향해 미간을 살짝 찌푸린 후 다시 셔츠 위를 내려다 봤다.\n\n“동생들이랑 나눠 먹을 거야.”\n\n“뭐? 설마 가족이 있어?“\n\n소녀는 뭔가 의외라는 표정으로 듯 몇 걸음 뒤로 물러나며 콧소리 섞인 투로 중얼거렸다.\n\n“이건 생각한 거랑 좀 다른데······.”\n\n소녀는 그 자리에 선 채 무언가 생각에 빠진 듯한 표정을 지으며 멍하니 허공을 바라봤다. 갑자기 입을 다문 그녀를 살펴보던 키안은 조심스레 그녀의 곁을 지나쳐 왔던 길을 되돌아 갔다. 이상한 여자를 만나서 시간은 좀 걸렸지만 어쨌든 먹을 걸 구했으니 동생들에게 돌아갈 차례였다.\n\n그가 열매를 한 알이라도 흘릴까 셔츠를 내려다보며 조심스레 발걸음을 옮기고 있을 때 갑자기 정면에서 짜증 섞인 목소리가 들렸다.\n\n“그냥 가는 게 어딨어!”\n\n큰  목소리에 키안은 눈썹을 움찔하며 셔츠에서 고개를 들었다. 그러자 정면에는 은발의 소녀가 팔짱을 낀 채 짝다리로 서 있었다.\n\n“이렇게 뜻밖의 도움을 받았으면 ‘정말 감사드립니다. 저는 누구라고 합니다. 도움을 주신 분의 성함을 알고 싶군요.’ 이렇게 말해야지.”\n\n키안은 눈을 한 번 감았다 뜨며 그녀에게 물었다.\n\n“···왜?”\n\n순수한 의문이었다. 키안은 그녀가 말하는 행동의 흐름이 왜 있어야 하는지 이해되지 않았다.\n\n얼빠진 표정을 하고 있는 키안을 본 소녀가 탄식을 내뱉었다.\n\n“하······. 이런 기본적인 것도 모르면 어쩌란 거야.”\n\n소녀는 바닥을 돌멩이 하나를 발 끝으로 굴리며 말을 이었다.\n\n“우리 엘프 아이들은 알아서 잘 자라는데 인간들은 누가 안 가르쳐주면 아무것도 못 하는구나······.”\n\n혼자 알 수 없는 말을 중얼거리는 소녀를 향해 키안이 혼잣말인지 질문인지 모를 말을 내뱉었다.\n\n“엘프? 그게 뭐지?”\n\n그의 질문에 소녀의 입이 숨을 삼키는 소리와 함께 조금 벌어졌다. 꽤나 당황했는지 그녀의 눈꺼풀이 여러번 깜빡거렸다.\n\n“···엘프가 뭔지 몰라? 네 눈 앞에 있잖아.”\n\n그녀의 말에 키안은 어리둥절한 표정을 한 채 셔츠 위의 열매를 내려다봤다.\n\n“그거는 블루 베리고. 내가 엘프잖아.”\n\n“엘프?”\n\n그의 입에서 또 한 번 얼빠진 목소리가 나왔다. 소녀는 자신의 귀를 가리키며 그에게 눈썹을 으쓱해 보였다. 하지만 키안은 여전히 멍하니 서 있었다.\n\n“귀가 뾰족하잖아. 그럼 엘프잖아?”\n\n“아······. 엘프.”\n\n귀가 뾰족하면 엘프. 자신이 먹은 열매가 블루 베리란 것과 함께 오늘 처음 안 사실이었다.\n\n그의 반응을 본 엘프의 어깨가 축 쳐졌다. 그녀는 아무 말 없이 어깨 부근의 머리카락을 손가락으로 꼬며 주변 풍경을 멍하니 바라보았다.\n\n잠시 후 그녀는 꼬고 있던 머리카락을 손가락으로 훑으며 가지런히 정리했다. 그리고 아까보다 낮아진 목소리로 입을 열었다.\n\n“뭐, 내 덕에 엘프가 뭔지도 알게 됐지? “\n\n키안은 조심스레 고개를 끄덕거렸다. 그 모습을 본 엘프의 눈가가 매섭게 좁아졌다.\n\n“고, 고마워.”\n\n“그게 끝이야?”\n\n키안은 그녀의 날카로운 시선을 피해 허공에 시선을 던지며 다음으로 해야 할 행동을 떠올려 봤다.\n\n“내가 어떻게 하라고 했지?”\n\n“아! 난 키안··· 이라고 해. 그, 음, 도움을 주신 분의 이름을 알 수 있을까요······?”\n\n엘프 소녀는 무표정한 얼굴로 그를 잠시 바라보다 눈을 감고 고개를 끄덕였다. 그녀는 자신의 이름을 말하기 전 헛기침을 하며 목을 가다듬었다.\n\n“난 이브노바야. 이브노바 델 이코벨라우나.”\n\n자신의 이름을 말하는 그녀의 목소리는 크고 또렷했다. 하지만 키안에겐 너무 길어서 기억하기 힘든 이름이었다.\n\n서로 이름을 밝힌 둘 사이에 어색한 침묵이 흘렀다. 키안이 이브노바의 눈치를 살피며 조심스레 다시 발걸음을 옮기려 하는 순간 그녀가 큰 걸음으로 그에 앞에 걸어와 섰다.\n\n“그런데 말이야. 도움을 받으면 그걸 보답하는 게 또 예의거든?”\n\n그녀는 키안 앞을 좌우로 왔다갔다 하며 말을 이었다.\n\n“기억할지는 모르겠지만 내가 찾는 게 있는 데 말이야. 그걸 좀 같이 찾아줬으면 해.”\n\n움직이던 그녀는 멈춰선 후 고개를 들고 키안과 눈을 마주쳤다. 그녀의 짙은 녹색 눈동자가 키안의 평범한 갈색 눈동자에 비춰졌다.\n\n“그게 뭔데?”\n\n“말해줘도 모를 거야. 그보다 네 동생들은 어디 살아? 한번 보고 가도 되지?”\n\n키안은 꺼림직하게 고개를 끄덕였다. 어차피 오늘 이브노바의 행적으로 미뤄봐서는 자신이 데려가지 않아도 본인 마음대로 따라올 것 같았다. 생각을 마친 키안은 조심스레 발걸음을 옮겼고 이브노바는 지팡이를 짚으며 그의 뒤를 따라 나섰다.\n\n이브노바를 뒤에 달고 길을 걸으며 그녀가 여기서 뭘 찾으려는 건지 나름대로 추측해보던 키안은 문득 한 기억이 떠올랐다.\n\n“그러고 보니까 지난번에 다른 사람도 너처럼 뭔가 찾고 있다고 하던데.”\n\n“···누가? 뭐를?”\n\n잠깐 시간을 두고 나온 이브노바의 목소리가 아까보다 살짝 낮아졌지만 키안은 눈치채지 못했다.\n\n“누군지는 몰라. 어떤 여자가 너처럼 귀가 뾰족한 여자를 봤는지 물어보더라고. 근데 난 그런 여자는 오늘 처음 본 거니까 그 땐 본 적 없다고 했지. 아, 아니구나.”\n\n키안의 미간이 살짝 찌푸려지며 눈동자가 위를 향했다.\n\n“그 여자도 귀가 뾰족했던 거 같은데.”\n";
        }
        else
        {
            data["contents"] = "에브게니스는 지금이야 비록 아무도 살지 않는 땅이지만 처음부터 아무도 살지 않았던 것은 아니었다.\n\n그 흔적으로 숲 속에는 버려진 오두막이 있었다. 지붕은 두꺼운 풀로 뒤덮혀 있고 벽은 덩굴에 감싸져 언제 지었는지도 모를 정도로 오래된 집이었다.\n\n이곳에 키안과 네 명의 아이들이 살고 있었다.\n\n모두 부모는 달랐지만 다들 지금은 부모 없이 서로에게 의지하며 살아가는 처지였다. 지난해 겨울 추위를 피할 곳을 찾던 도중 우연히 이곳을 발견한 뒤로 계속 여기서 지내고 있었다.\n\n겉보기에는 언제 무너져도 이상하지 않았으나 내부는 의외로 깨끗하고 아늑했다. 전에 살던 이가 가구를 모두 남겨 두고 떠난 덕에 생활하기에도 불편함이 없었다.\n\n침대나 탁자 같은 것뿐만 아니라 특이한 것도 남아 있었다. 천장에는 주먹만한 구슬이 여러 개 박혀 있었고 출입문 옆에는 나무로 된 작은 레버가 달려 있었다. 그 레버를 위로 올리면 구슬이 아침 햇살처럼 연노란 색의 빛을 발했고 아래쪽으로 내리면 빛은 사라졌다.\n\n무슨 원리인지는 모르겠지만 해가 저물어도 빛이 있다는 것은 생활에 크나큰 도움이 되었다. 가끔 불빛에 이끌려 벌레나 짐승 같은 불청객들이 찾아오는 점만 제외하면.\n\n\n***\n\n\n어느 날 저녁, 여느 때와 마찬가지로 오두막 문 틈 사이로 빛과 함께 소란스러운 소리가 흘러나오고 있었다.\n\n대여섯 살 정도 되어보이는 붉은 머리에 녹색 눈을 가진 남자 아이가 낮에 주워 온 밤톨을 탁자위에 쏟아냈다. 밤은 탁자 위에서 요란한 소리를 내며 바닥으로 굴러 떨어졌다.\n\n그보다 한 두 살 정도 많아 보이는 까만 머리에 까만 눈을 가진 여자 아이가 바닥에 떨어진 밤을 주우며 그에게 화를 냈다. 남자 아이도 지지 않고 받아치자 그녀와 비슷한 또래로 보이는 금발에 벽안을 가진 여자 아이는 둘의 싸움을 보며 지겹다는 듯 고개를 저었다.\n\n둘을 말리기 위해 나선 건 키안이었다.\n\n“둘 다. 싸우지 마.”\n\n키안은 둘의 사이에 양 팔을 집어 넣고 그들을 밀어냈다. 그들보다 열 살 정도 많은 그였기에 싸움을 말리는 것은 쉬웠다.\n\n심통이 난 표정으로 검은 머리의 여자 아이를 쳐다보던 남자 아이가 우스꽝스러운 표정을 지으며 그녀에게 혀를 내밀어 보였다. 여자 아이는 입술을 깨물며 아직 손에 쥐고 있던 밤 하나를 던져 그의 머리를 맞추었다.\n\n“아야! 뭘 던지는 거야 멜리나!”\n\n“션. 내가 먹을 거 땅에 흘리지 말라고 했어, 안 했어!”\n\n션과 멜리나가 다시 싸움을 시작하자 금발의 여자 아이는 재밌다는 듯 웃기 시작했다. 키안은 그녀를 보며 지친 목소리로 중얼거렸다.\n\n“이반나. 웃지만 말고 쟤들 좀 말려 봐.”\n\n“야, 그만 해.”\n\n이반나라고 불린 여자애가 그들과 멀찍이 선 채 무심한 투로 대충 내뱉었다.\n\n“흘리면 어때서! 다시 주우면 되잖아!”\n\n“네가 떨어트려 놓고 네가 안 주우니까 그러지!”\n\n말려도 싸움을 계속하는 모습에 키안은 참지 못하고 빽 소리를 질렀다.\n\n“집 안에서 소리지르지 좀 마!”\n\n하지만 션과 멜리나는 그보다 더 큰 목소리를 내며 말다툼을 멈추지 않았다.\n\n키안마저 소리를 지르자 시끄러운 걸 싫어하는 이반나가 인상을 찌푸리며 신경질 섞인 목소리로 다시 말했다.\n\n“그만 하라고. 시끄러워 죽겠네.”\n\n“······.”\n\n“······.”\n\n이반나의 푸념이 끝난 순간 방금 전까지 소란이 일었다고는 생각할 수 없을 정도로 무거운 정적이 흘렀다. 션과 멜리나는 딱딱히 굳은 얼굴로 입을 다물었다. 둘 뿐만 아니라 이반나와 키안도 마찬가지였다.\n\n그들은 방금까지 싸웠다고는 생각할 수 없을 만큼 딱딱하게 굳은 얼굴로 서로를 응시했다. 잠시 후 모두의 고개가 천천히 돌아가며 같은 곳을 쳐다봤다. 대문 쪽이었다.\n\n똑 똑 똑.\n\n다시 한 번, 문을 두드리는 소리가 들렸다. 조금 전 아이들이 다투는 소리 사이로 어렴풋이 들렸던 바로 그 소리였다. 못 듣고 지나칠 만한 작은 소리였지만 집 안에 있는 모두가 그 소리를 놓치지 않고 분명히 들었다.\n\n처음 들어보는 소리였으니까.\n\n똑 똑 똑.\n\n집 안이 갑자기 조용해진 것이 이상한지 밖에 있는 누군가가 다시 문을 두드렸다. 정적 끝에 숨이 막힌다는 듯 누군가 툭 내뱉었다.\n\n“밖에 누가 왔나 봐.”\n\n션이었다. 그가 떨리는 목소리로 중얼거리자 멜리나는 어이 없다는 듯 그를 흘겨 보며 빈정거렸다.\n\n“오긴 누가 왔다는 거야?”\n\n하지만 그녀의 입에서 나온 목소리에도 긴장이 배여 있었다. 그녀의 말대로 이곳을 찾아올 사람은 없었다. 그들 모두 집에 초대할 친구 같은 건 없는 처지였다.\n\n이반나는 션과 멜리나를 번갈아 보고는 애써 침착한 목소리로 물었다.\n\n“혹시 누구 여기 없는 사람 있어?”\n\n“없는 사람?”\n\n션은 그녀의 말을 따라하면서 고개를 홱홱 돌렸다. 그러다 갑자기 그의 입꼬리가 올라가면서 표정이 한층 밝아졌다. 하지만 눈은 여전히 긴장한 채였다.\n\n“조르디가 없네!”\n\n그는 문을 향해 걸어가며 투덜거렸다.\n\n“얘는 맨날 늦게 온다니까.”\n\n그들 중 가장 어린 조르디는 행동이 느리고 길눈이 어두운 아이였다. 이번에도 분명 숲을 헤매다 이제서야 돌아온 모양이었다.\n\n션은 까치발을 들며 문고리에 손을 뻗었다. 그 순간, 강한 힘이 그의 손목을 재빨리 낚아 챘다. 어느새 키안이 그의 옆에 와 있었다.\n\n이반나도 조심스레 다가와 키안의 곁에 섰다. 손목에 느껴지는 압박감 때문에 이마를 찡그리고 있는 션에게 그녀가 물었다.\n\n“조르디면 그냥 들어오면 되는데 왜 문을 두드려?”\n\n이 집에 사는 그들 중 누구도 대문을 두드려 본 적이 없었다. 자신의 집에 들어오는데 문을 두드릴 이유가 있을까. 남의 집을 방문한다면야 노크를 하는 것이 예의겠지만 이들은 남의 집을 방문할 일도 없었고 설령 방문한다 해도 그런 예의는 배운 적이 없었다. 이러나 저러나 문을 두드릴 이유는 없었다.\n\n키안은 션을 뒤로 잡아 끌며 자신이 대문 앞에 똑바로 섰다.\n\n“조르디?”\n\n돌아오는 대답은 없었다.\n\n“그냥 들어 와. 문은 왜 두드리고 그래?”\n\n키안은 조르디가 문을 열고 들어오길 기다렸지만 문은 움직이지 않았다. 오히려 이상하게도 문 너머로 아무런 인기척도 느껴지지 않았다.\n\n키안은 조심스레 문고리에 손을 뻗었다. 문고리를 돌리고 앞으로 밀자 녹슨 경첩이 비명을 질렀다.\n\n“형!”\n\n“어?”\n\n문 앞에는 갈색 머리에 연약한 인상을 가진 남자 아이가 서 있었다. 조르디였다.\n\n“갑자기 어두워져서 집에 못 돌아올 뻔 했는데 이 누나가 데려다 줬어.”\n\n그가 자신의 뒤쪽을 올려다 보며 말했다. 키안도 아이를 내려다보고 있던 시선을 위로 옮겼다. 조르디의 뒤로 까만 머리를 길게 기른 한 여인이 서 있는 것이 보였다. 해가 떨어진 주변 풍경보다 더 어두워 오히려 눈에 띌 정도로 짙은 검정이었다.\n\n키안은 여인과 눈이 마주쳤다. 그녀의 눈동자도 깊고 어두웠다. 그 속에서 세어 나오는 음산한 느낌에 그는 자신도 모르게 숨을 삼켰다.\n\n“안녕하세요.”\n\n여인이 빙긋 웃으며 인사를 건냈다. 그녀가 고개를 까딱거릴 때 머리카락 사이로 뾰족한 귀가 얼핏 보인 것 같았다.\n\n\n***\n\n\n“그래서?”\n\n한 쌍의 남녀가 어둑해진 숲 속을 걷고 있었다. 남자는 옷자락 위에 블루 베리를 가득 담은 채 조심스레 걸었고 여자는 한 손에는 지팡이를 짚은 채 다리를 휘적거리며 그를 따라가고 있었다.\n\n아직 해는 사라지지 않고 서쪽 하늘에 걸려 있었지만, 울창한 나뭇가지 탓에 길은 이미 어두웠다.\n\n“그래서? 그 여자랑 무슨 얘기 했어?”\n\n이브노바가 키안을 앞지르며 멈춰선 채 뒤를 돌아보았다. 그녀가 짚고 있는 지팡이 덕분인지는 몰라도 울퉁불퉁한 숲 길을 걷는 데도 그녀는 키안과 달리 발걸음이 가벼웠다.\n\n“딱히. 별 얘기는 안 했어.”\n\n“그래서 그 별 것 아닌 얘기가 뭐냐니까?”\n\n“그냥 뭐, 자기처럼 귀가 뾰족한 여자를 본 적이 있냐고 하던데.”\n\n키안은 그녀를 지나치며 대답했다. 뒤에서 지팡이 끝이 바닥에 쌓인 낙엽을 깊게 푸욱 찌르는 소리가 들렸다.\n\n“그거 말고 다른 건?”\n\n이브노바가 그의 옆에 다가와 나란히 걸으며 물었다.\n\n“글쎄. 기억 안 나.”\n\n“그 여자 이름은 뭐래?”\n\n“글쎄. 모르겠네. 안 물어봤던 거 같기도 하고······.”\n\n“그럼 어떻게 생겼는지는 기억나?”\n\n“응? 그냥 여자였던 것 밖에는 딱히······.”\n\n그의 대답이 마음에 들지 않은 이브노바가 짧게 혀를 찼다 .\n\n“기억력이 왜 이렇게 나빠? 너 혹시 머리가 좀 모자란 건 아니지?”\n\n억양이나 목소리로 보아 그녀는 진심으로 그리 생각하는 듯 했다.\n\n이브노바의 말에 키안은 그녀에게 기분 나쁘다는 듯한 눈초리를 보냈다.\n\n그는 지금까지 자신의 기억력이 좋다고 생각해왔다. 스스로 생각하기에 자신은 무엇이 먹을 수 있는 것이고 그것들이 어디 있는지 전부 기억했기 때문에 지금까지 살아남을 수 있었다. 그렇기에 기억력은 그의 작은 자부심이었다.\n\n“뭘 째려 봐?”\n\n“나 기억력 좋아.”\n\n“기억력이 좋은데 그 여자 얼굴도 기억을 못 해?”\n\n그녀가 다그치자 키안은 작게 신음 소리를 냈다. 불과 며칠 전의 일인데도 그 때의 상황이 기억이 나지 않았다. 꽤 길게 얘기를 나눴던 것 같은데 기억 나는 장면은 그녀가 자신의 귀를 보여주며 이런 귀를 가진 여자를 본 적이 있냐고 물었던 것 뿐이었다.\n\n“됐어. 인간이 멍청한 건 딱히 새삼스러운 건 아니니까.”\n\n그가 멍청한 표정으로 안간힘을 쓰며 기억을 떠올리고 있는 걸 지켜보던 이브노바가 이윽고 쌀쌀맞게 내뱉었다. 그녀는 키안의 뒤로 돌아가 그의 등을 강하게 떠밀며 소리쳤다. 그 때문에 그는 하마터면 블루 베리를 쏟을 뻔했다.\n\n“빨리 좀 걸어! 집으로 돌아가는 건데 왜 이렇게 느려. 집이 어딨는지도 까먹은 거야?”\n\n하지만 키안의 발걸음은 전혀 빨라지지 않았다. 몇번이고 신경질을 내며 자꾸 등을 떠미는 그녀로부터 블루 베리를 겨우 지키며 집에 도착했을 때는 이미 어두워진 후였다.\n\n“저기야.”\n\n키안은 턱를 까딱거리며 나무 사이로 보이는 불빛을 가리켰다. 그 순간 누군가 뒤에서 옷자락을 잡아당기는 것이 느껴졌다. 이브노바였다.\n\n“저 불빛은 뭐야?”\n\n그녀가 낮은 목소리로 물으며 지팡이를 고쳐 잡았다. 키안은 갑자기 딱딱해진 그녀의 분위기에 당황했지만 대수롭지 않게 대답했다.\n\n“저거? 나도 잘 몰라. 집 천장에 무슨 구슬이 박혀 있던데 거기서 불빛이 나오더라고.”\n\n“구슬?”\n\n이브노바는 의심 가득한 눈빛으로 불빛을 빤히 노려보았다. 그녀의 반응을 본 키안은 스스로 빛을 내는 구슬이라니, 직접 본 적이 없다면 믿기 힘든 이야기일 것이라 생각했다.\n\n“‘아테포마루스의 돌’인가? 그것만으로는 이런 마력이 느껴지진 않을 텐데······. 무슨 집 안에 태양이 떠 있는 것도 아니고.”\n\n알 수 없는 그녀의 말을 뒤로 한 채 키안은 집으로 향했다. 이브노바도 발걸음을 옮겼다. 그녀는 아까보다 좀 더 거리를 둔 채 그의 뒤를 따라갔다.\n\n문 앞에 선 키안은 옷자락을 쥔 손을 조심스레 손가락만 움직여 문고리를 돌렸다. 그의 뒤에는 이브노바가 지팡이를 양손에 쥔 채 서 빛이 닿지 않는 곳에 멀찍이 떨어져 서 있었다.\n\n키안은 문고리를 돌리고 어깨로 문을 밀었다. 낡은 경첩에서 녹슨 쇳가루가 떨어지며 기분 나쁜 높은 소리가 났다. 그가 뻑뻑한 문을 조심히 밀고 있던 찰나에, 안쪽에서 누군가 문고리를 잡아 당기며 문이 완전히 활짝 열렸다.\n\n“형!”\n\n“오빠!”\n\n문 너머에는 네 명의 아이들이 키안을 올려다보고 있었다. 좀전까지 자기들끼리 재밌게 놀고 있었는지 뺨은 붉게 달아올랐고 입가에는 웃음기가 가득했다.\n\n“안녕하세요. 또 뵙네요.”\n\n“어······.”\n\n그리고 그들의 뒤에 검은 머리를 길게 기른 여인이 서 있었다. 그녀는 머리카락과 대비되는 흰 셔츠에 가죽바지를 입은 모습이었다.\n\n키안의 갈색 눈동자가 그녀의 새까만 눈동자와 눈이 마주쳤다. 키안은 깊은 물 속에 빠지는 듯한 감각을 느끼며 그 자리에 얼어붙었다.\n\n“형! 이 누나가 형 올 때까지 우리랑 놀아줬어.”\n\n붉은 머리를 가진 션이 시선을 검은 머리의 여자에게 옮기며 말했다.\n\n“이 분 재미있는 이야기를 엄청 많이 알고 있어요!”\n\n평소에는 쌀쌀 맞은 이반나도 즐거운 듯 웃고 있었다.\n\n“오빠는 재미없는데 이 누나는 너무 재밌어!”\n\n“아니야 형도 재밌어······.”\n\n멜리나와 조르디도 한 마디씩 곁들였다.\n\n여인은 옅은 미소를 지으며 아이들을 지그시 내려다 보았다. 그녀가 고개를 숙이자 옆 머리카락이 흘러내리며 뾰족한 귀가 드러났다.\n\n“아이들이 참 귀엽네요. 그건 그렇고 한 번 더 여쭤 볼 게 있어서 왔는데 안 계시더라구요.”\n\n그녀는 고개를 들며 다시 키안의 눈을 똑바로 쳐다봤다. 잠시 얼어 붙어있는 그와 눈을 마주치던 그녀는 그의 뒤로 시선을 옮겼다.\n\n“같이 오신 분은 누구죠?”\n\n순간, 강력한 힘이 키안을 앞에서 강하게 미는 동시에 뒤에서 잡아당겼다. 그가 서 있던 자리에는 그의 옷자락에서 떨어진 블루 베리만이 공중에 흩뿌려졌다.\n\n키안은 폐가 터질 것 같은 압박감을 느끼며 바닥에 내동댕이쳐졌다. 그가 기침을 토해내며 고개를 들자 이브노바가 그의 곁에 서 있는 것이 보였다. 어두워서 표정은 보이지 않았지만 무슨 이유인지 그녀의 주위 공기에 날이 바짝 서 있었다.\n\n“켁···. 쿨럭, 뭐야······?”\n\n“거기 가만히 있어.”\n\n이브노바는 지팡이를 오두막 내부를 향하여 겨눈 채 앞으로 천천히 걸어갔다. 그녀가 다가오자 아이들은 검은 머리의 여인의 뒤로 몸을 숨겼다.\n\n“어쩐지 이상하다고 했어. 이런 마력이 다 낡은 오두막 한 채에서 나올 수 있는 게 아니지.”\n\n이브노바가 미간을 구기며 중얼거렸다. 그녀는 대문에서 열 걸음 정도 떨어진 거리에 멈춰 섰다. 집에서 새어 나온 빛이 그녀의 얼굴을 비추자 사납게 변한 표정이 드러났다.\n\n“클로타, 감히 배신자 주제에 내 앞에 나타나는구나”\n\n클로타라 불린 여인은 빙긋 웃으며 대꾸했다.\n\n“누군가 했더니 전하셨군요. 여긴 어쩐 일이신가요?”\n\n“말장난 하지 마라. 다크 엘프가 여기까진 왜 왔지?”\n\n그녀는 바닥에 거칠게 지팡이를 꽂으며 차갑게 대꾸했다.\n\n“오크가 떼어준 땅에나 빌붙어 있을 것이지 감히 엔필리스 근처까지 기어들어 와? 너희는 엔필리스에는 돌아올 수 없다.”\n\n표정을 일그러트린 이브노바와 달리 클로타는 여전히 웃는 얼굴이었다.\n\n“엔필리스에 돌아갈 생각은 없어요. 그냥 찾는 게 있어서 돌아다니다 보니 여기까지 오게 된 거예요.”\n\n“그래. 뾰족한 귀를 가진 여자를 찾는다던데.”\n\n그녀의 말에 클로타의 얼굴에서 웃음기가 사라졌다. 그녀는 의외라는 듯한 표정으로 멀리 널부러져 있는 키안을 흘깃 쳐다보았다.\n\n“저 인간이 말해 줬나요? 생각보다 기억력이 좋은 인간이군요. 분명 기억을 다 흩트려놨는데.”\n\n“그래서?”\n\n이브노바는 여전히 그녀에게서 눈을 때지 않은 채 물었다.\n\n“찾던 걸 찾은 거 같은데 이제 어쩔 셈이냐?”\n\n“글쎄요...”\n\n클로타는 여전히 키안을 바라보고 있었다. 정확히는 방향만 키안을 향하고 있었다. 그녀의 초점은 그보다 더 뒤에 맺혀 있었다.\n\n“어떻게 할까요?”\n\n그녀가 물었다. 목소리의 크기로 보아 혼잣말이 아니었다. 키안의 뒤에 있는 누군가에게 거는 말이었다.\n\n“체라?”\n\n“여기서 죽인다.”\n\n어둠 속에서 중후한 목소리가 들려 왔다. 키안이 뒤를 돌아 보자 목소리의 주인이 나무 사이로 걸어 나오고 있었다.\n\n어두워서 잘 보이진 않았지만 키안의 두 배는 될 것 같은 거대한 덩치였다. 나뭇잎 사이로 비치는 달빛이 그의 얼굴을 희미하게 비추었다. 입 밖으로 드러난 송곳니에 인간과 확연히 다른 녹색 피부를 가진 거한이었다.\n\n대륙 서쪽의 초원과 산악 지대에서 살아가는 강인하고 야만적인 종족, 오크였다.\n";
        }
        data["illustrationUrl"] = "";
        var value = JsonConvert.SerializeObject(data);

        mContractManager.resGetStoryDataFull(value);
    }

    int commentLastIdx = 100;
    public void reqGetCommentLast(int _novelId, int _count)
    {
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        for (int i = 0; i < _count; i++)
        {
            int commentId = commentLastIdx - i;
            if (commentId < 0)
            {
                break;
            }
            Dictionary<string, object> dic = new Dictionary<string,object>();
            dic["id"] = commentId;
            dic["nickname"] = "Crow" + commentId;
            dic["block"] = 1234567890;
            dic["comment"] = "나는 크로우다.";

            data.Add(dic);
        }

        var value = JsonConvert.SerializeObject(data);

        mContractManager.resGetComment(value);
    }

    public void reqGetComment(int _novelId, int _fromCommentId, int _count)
    {
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        for (int i = 0; i < _count; i++)
        {
            int commentId = _fromCommentId - i;
            if (commentId < 0)
            {
                break;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["id"] = commentId;
            dic["nickname"] = "Crow" + commentId;
            dic["block"] = 1234567890;
            dic["comment"] = "나는 크로우다.";

            data.Add(dic);
        }

        var value = JsonConvert.SerializeObject(data);

        mContractManager.resGetComment(value);
    }

    public void reqSendComment(int _novelId, string _mainTitle, string _comment)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["id"] = ++commentLastIdx;
        data["nickname"] = "Crow" + commentLastIdx;
        data["block"] = 1234567890;
        data["comment"] = _comment;

        var value = JsonConvert.SerializeObject(data);

        mContractManager.resSendComment(value);
    }

    List<CountryData> countryDataArchive = new List<CountryData>();

    private void initCountryArchive()
    {
        if (countryDataArchive.Count > 0)
        {
            return;
        }

        for (int cid = 0; cid < CountryManager.COUNTRY_MAX; cid++)
        {
            CountryData countryData = new CountryData();
            countryData.id = cid;
            countryData.population = 0;
            
            int logCount = UnityEngine.Random.Range(0, 150);
            for (int lid = 0; lid < logCount; lid++)
            {
                LogData logData = new LogData();
                logData.blockNum = 123456789 + lid;
                logData.logType = UnityEngine.Random.Range(0, 11) + 1;
                logData.whoAddr = "0x000000000000000" + lid;
                logData.whoNickName = "Crow";
                if (logData.logType == 4)
                {
                    logData.dataInt = BigInteger.Parse("1000000000000000000");
                }
                else if (logData.logType == 3)
                {
                    logData.dataInt = BigInteger.Parse("100000");
                }
                else if (logData.logType == 12)
                {
                    logData.dataInt = BigInteger.Parse("0");
                }
                logData.dataStr = "Crow castle";

                countryData.addLog(logData);
            }

            if (cid == 0)
            {
                PropertyData propertyData = new PropertyData();
                propertyData.propertyCategory = CountryManager.PROPERTY_CATEGORY_EVEGENIS_RAYNOR_BLESS;
                propertyData.propertyType = CountryManager.PROPERTY_TYPE_MINING_INC;
                propertyData.value = 100000;
                propertyData.startBlock = 0;

                countryData.propertyList.Add(propertyData);
            }

            countryData.castleData.hasMonarch = UnityEngine.Random.Range(0, 2) == 0;
            countryData.castleData.name = "Crow castle";
            countryData.castleData.monarchId = UnityEngine.Random.Range(0, 10000);
            countryData.castleData.monarchOwnerNickname = "Crow #" + countryData.castleData.monarchId.ToString("0000");

            for (int i = 0; i < 10; i++)
            {
                countryData.castleData.formerMonarchList.Add(UnityEngine.Random.Range(0, 10000));
            }

            MiningTaxData miningTaxData = new MiningTaxData();
            miningTaxData.startBlock = 123456789;
            miningTaxData.endBlock = 0;
            miningTaxData.tax = UnityEngine.Random.Range(0, 70) * 10000;
            countryData.castleData.lastMiningTaxData = miningTaxData;

            countryData.castleData.treasury = BigInteger.Parse("100000000000000") * UnityEngine.Random.Range(0, 100000000);
            countryData.castleData.personalSafe = BigInteger.Parse("100000000000000") * UnityEngine.Random.Range(0, 100000000);
            countryData.castleData.nextTaxSettableBlock = 0;

            countryDataArchive.Add(countryData);
        }
        
    }

    public void reqCountryData(int _cid)
    {
        initCountryArchive();

        Dictionary<string, object> data = new Dictionary<string, object>();

        data["id"] = _cid;
        data["population"] = countryDataArchive[_cid].population;

        // latest 30 logs
        List<Dictionary<string, object>> logList = new List<Dictionary<string, object>>();
        for (int i = 0; i < 30; i++)
        {
            if (countryDataArchive[_cid].logList.Count <= i)
            {
                break;
            }

            LogData lData = countryDataArchive[_cid].logList[i];

            Dictionary<string, object> logData = new Dictionary<string, object>();
            logData["id"] = lData.id;
            logData["blockNum"] = lData.blockNum;
            logData["logType"] = lData.logType;
            logData["who"] = lData.whoAddr;
            logData["nickName"] = lData.whoNickName;
            logData["dataInt"] = lData.dataInt;
            logData["dataStr"] = lData.dataStr;

            logList.Add(logData);
        }
        data["logList"] = logList;

        List<Dictionary<string, object>> propertyList = new List<Dictionary<string, object>>();

        for (int i = 0; i < countryDataArchive[_cid].propertyList.Count; i++)
        {
            PropertyData pData = countryDataArchive[_cid].propertyList[i];
            Dictionary<string, object> propertyData = new Dictionary<string, object>();
            propertyData["propertyCategory"] = pData.propertyCategory;
            propertyData["propertyType"] = pData.propertyType;
            propertyData["value"] = pData.value;
            propertyData["startBlock"] = pData.startBlock;

            propertyList.Add(propertyData);
        }
        data["propertyList"] = propertyList;

        CastleData cData = countryDataArchive[_cid].castleData;
        Dictionary<string, object> castleData = new Dictionary<string, object>();
        castleData["hasMonarch"] = cData.hasMonarch;
        castleData["name"] = cData.name;
        castleData["monarchId"] = cData.monarchId;
        castleData["monarchOwnerNickname"] = cData.monarchOwnerNickname;

        castleData["formerMonarchList"] = cData.formerMonarchList;

        MiningTaxData mtData = countryDataArchive[_cid].castleData.lastMiningTaxData;
        Dictionary<string, object> miningTaxData = new Dictionary<string, object>();
        miningTaxData["tax"] = mtData.tax;
        miningTaxData["startBlock"] = mtData.startBlock;
        miningTaxData["endBlock"] = mtData.endBlock;

        castleData["lastMiningTaxData"] = miningTaxData;

        castleData["treasury"] = cData.treasury;
        castleData["personalSafe"] = cData.personalSafe;
        castleData["nextTaxSettableBlock"] = cData.nextTaxSettableBlock;

        data["castleData"] = castleData;

        var value = JsonConvert.SerializeObject(data);
        mContractManager.resCountryData(value);
    }

    public void reqDonate(int _cid, BigInteger _value)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data["cid"] = _cid;
        data["value"] = _value.ToString();

        countryDataArchive[_cid].castleData.treasury += _value;

        var value = JsonConvert.SerializeObject(data);

        mContractManager.resDonate(value);
    }

    public void reqSetMiningTax(int _cid, int _tax)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data["cid"] = _cid;
        data["tax"] = _tax;

        countryDataArchive[_cid].castleData.lastMiningTaxData.tax = _tax;
        countryDataArchive[_cid].castleData.lastMiningTaxData.startBlock = SystemInfoManager.instance.blockNumber;
        countryDataArchive[_cid].castleData.nextTaxSettableBlock = SystemInfoManager.instance.blockNumber + 30;

        var value = JsonConvert.SerializeObject(data);

        mContractManager.resSetMiningTax(value);
    }

    public void reqDepositMonarchSafe(int _cid, BigInteger _value)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data["cid"] = _cid;
        data["value"] = _value.ToString();

        countryDataArchive[_cid].castleData.personalSafe += _value;

        var value = JsonConvert.SerializeObject(data);

        mContractManager.resDepositMonarchSafe(value);
    }

    public void reqWithdrawMonarchSafe(int _cid, BigInteger _value)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data["cid"] = _cid;
        data["value"] = _value.ToString();

        countryDataArchive[_cid].castleData.personalSafe -= _value;

        var value = JsonConvert.SerializeObject(data);

        mContractManager.resWithdrawMonarchSafe(value);
    }

    public void reqMoreLogData(int _cid, int _fromId, int _count)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data["id"] = _cid;
        // latest 30 logs
        int startIdx = 0;
        for (int i = 0; i < countryDataArchive[_cid].logList.Count; i++)
        {
            if (countryDataArchive[_cid].logList[i].id == _fromId)
            {
                startIdx = i;
                break;
            }
        }

        List<Dictionary<string, object>> logList = new List<Dictionary<string, object>>();
        for (int i = 0; i < _count; i++)
        {
            if (countryDataArchive[_cid].logList.Count <= i + startIdx)
            {
                break;
            }

            LogData lData = countryDataArchive[_cid].logList[i + startIdx];

            Dictionary<string, object> logData = new Dictionary<string, object>();
            logData["id"] = lData.id;
            logData["blockNum"] = lData.blockNum;
            logData["logType"] = lData.logType;
            logData["who"] = lData.whoAddr;
            logData["nickName"] = lData.whoNickName;
            logData["dataInt"] = lData.dataInt;
            logData["dataStr"] = lData.dataStr;

            logList.Add(logData);
        }
        data["logList"] = logList;


        var value = JsonConvert.SerializeObject(data);
        mContractManager.resMoreLogData(value);
    }

    private int generatedRound = 0;
    private List<List<CandidateData>> candidateListArchive = new List<List<CandidateData>>();
    private List<List<int>> votingDataListArchive = new List<List<int>>();
    private List<List<int>> totalVotingCountListArchive = new List<List<int>>();
    private void initElectionArchive(int _maxRound)
    {
        if (generatedRound >= _maxRound)
        {
            return;
        }

        // add index 0
        if (candidateListArchive.Count == 0)
        {
            candidateListArchive.Add(new List<CandidateData>());
            votingDataListArchive.Add(new List<int>());
            totalVotingCountListArchive.Add(new List<int>());
        }

        for (int round = generatedRound + 1; round <= _maxRound; round++)
        {
            bool pastRound = round <= ElectionManager.instance.getElectionRound();

            List<CandidateData> candidateList = new List<CandidateData>();
            List<int> votingDataMap = new List<int>();
            List<int> votingTotalCountList = new List<int>();
            candidateListArchive.Add(candidateList);
            votingDataListArchive.Add(votingDataMap);
            totalVotingCountListArchive.Add(votingTotalCountList);

            for (int cid = 0; cid < CountryManager.COUNTRY_ALL; cid++)
            {
                votingTotalCountList.Add(0);
                int maxCandidateCount = UnityEngine.Random.Range(0, 15);
                for (int i = 1; i <= maxCandidateCount; i++)
                {
                    CandidateData candidateData = new CandidateData();
                    candidateData.id = i;
                    candidateData.round = round;
                    candidateData.tokenId = UnityEngine.Random.Range(0, 10000);
                    candidateData.countryId = cid;
                    candidateData.address = "0x10000000000000000" + round + cid + i;
                    candidateData.nickname = "닉네임" + round + cid + i;
                    candidateData.title = "이게 나라냐!! " + round + cid + i;
                    candidateData.contents = "저를 뽑아주세요!! " + +round + cid + i;
                    candidateData.url = "https://taleofraynornft.com/";
                    candidateData.canceled = UnityEngine.Random.Range(0, 5) == 0;
                    if (!candidateData.canceled)
                    {
                        candidateData.votingCount = UnityEngine.Random.Range(0, 1000);
                        votingTotalCountList[cid] += candidateData.votingCount;
                    }
                    candidateData.nftReturned = UnityEngine.Random.Range(0, 3) != 0;
                    candidateData.registBlock = 123456789;

                    candidateList.Add(candidateData);
                }
            }

        }
        generatedRound = _maxRound;

        Debug.Log("initElectionArchive maxRound = " + _maxRound);
    }

    public void reqRoundCandidateList(int _round)
    {
        initElectionArchive(_round);

        Dictionary<string, object> data = new Dictionary<string, object>();

        data["round"] = _round;
        bool pastRound = _round <= ElectionManager.instance.getElectionRound();

        List<Dictionary<string, object>> candidateDataList = new List<Dictionary<string, object>>();

        foreach(CandidateData cData in candidateListArchive[_round])
        {
            if (cData.canceled)
            {
                continue;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["id"] = cData.id;
            dic["round"] = cData.round;
            dic["tokenId"] = cData.tokenId;
            dic["country"] = cData.countryId;
            dic["address"] = cData.address;
            dic["nickname"] = cData.nickname;
            dic["title"] = cData.title;
            dic["contents"] = cData.contents;
            dic["url"] = cData.url;
            dic["canceled"] = cData.canceled;
            dic["votingCount"] = pastRound ? cData.votingCount : 0;
            dic["nftReturned"] = cData.nftReturned;
            dic["registBlock"] = cData.registBlock;

            candidateDataList.Add(dic);
        }

        if (pastRound)
        {
            data["votingCountList"] = totalVotingCountListArchive[_round];
        } else
        {
            List<int> list = new List<int>();
            for (int cid = 0; cid < CountryManager.COUNTRY_MAX; cid++)
            {
                list.Add(0);
            }
            data["votingCountList"] = list;
        }

        data["list"] = candidateDataList;

        var value = JsonConvert.SerializeObject(data);
        mContractManager.resRoundCandidateList(value);
    }

    public void addCandidateData(CandidateData _data)
    {
        int round = _data.round;
        List<CandidateData> list = candidateListArchive[round];
        int maxId = 0;
        foreach(CandidateData cData in list)
        {
            if (cData.countryId != _data.countryId)
            {
                continue;
            }
            if (maxId < cData.id)
            {
                maxId = cData.id;
            }
        }
        _data.id = maxId + 1;
        list.Add(_data);

        _data.canceled = false;
        _data.votingCount = UnityEngine.Random.Range(0, 1000);
        totalVotingCountListArchive[_data.round][_data.countryId] += _data.votingCount;
        _data.nftReturned = false;
        _data.registBlock = SystemInfoManager.instance.blockNumber;

        for(int i = 0; i < characterListArchive.Count; i++)
        {
            CharacterData characterData = characterListArchive[i];
            if (characterData.tokenId == _data.tokenId)
            {
                characterListArchive.RemoveAt(i);
                stakingListArchive.Add(characterData);

                StakingData stakingData = new StakingData();
                stakingData.tokenId = characterData.tokenId;
                stakingData.startBlock = SystemInfoManager.instance.blockNumber;
                stakingData.endBlock = 0;
                stakingData.purpose = StakingManager.PURPOSE_MONARCH;
                stakingDataArchive.Add(characterData.tokenId, stakingData);
                break;
            }
        }

        responceCandidateData(_data);
    }

    public void editCandidateData(CandidateData _data)
    {
        int round = _data.round;
        List<CandidateData> list = candidateListArchive[round];
        for(int i = 0; i < list.Count; i++)
        {
            CandidateData cData = list[i];
            if (cData.countryId == _data.countryId && cData.id == _data.id)
            {
                list.RemoveAt(i);
                break;
            }
        }
        list.Add(_data);

        _data.canceled = false;
        _data.votingCount = 0;
        _data.nftReturned = false;

        responceCandidateData(_data);
    }

    public void cancelCandidateData(CandidateData _data)
    {
        int round = _data.round;
        List<CandidateData> list = candidateListArchive[round];
        for (int i = 0; i < list.Count; i++)
        {
            CandidateData cData = list[i];
            if (cData.countryId == _data.countryId && cData.id == _data.id)
            {
                cData.canceled = true;
                cData.nftReturned = true;
                break;
            }
        }

        _data.canceled = true;
        _data.nftReturned = true;

        for (int i = 0; i < stakingListArchive.Count; i++)
        {
            CharacterData characterData = stakingListArchive[i];
            if (characterData.tokenId == _data.tokenId)
            {
                stakingListArchive.RemoveAt(i);
                characterListArchive.Add(characterData);

                stakingDataArchive.Remove(characterData.tokenId);
                break;
            }
        }

        responceCandidateData(_data);
    }

    public void appointmentCandidateData(CandidateData _data)
    {
        int round = _data.round;
        List<CandidateData> list = candidateListArchive[round];
        for (int i = 0; i < list.Count; i++)
        {
            CandidateData cData = list[i];
            if (cData.countryId == _data.countryId && cData.id == _data.id)
            {
                cData.nftReturned = true;
                break;
            }
        }

        _data.nftReturned = true;

        for (int i = 0; i < stakingListArchive.Count; i++)
        {
            CharacterData characterData = stakingListArchive[i];
            if (characterData.tokenId == _data.tokenId)
            {
                stakingListArchive.RemoveAt(i);
                characterListArchive.Add(characterData);

                stakingDataArchive.Remove(characterData.tokenId);
                break;
            }
        }

        CastleData castleData = countryDataArchive[_data.countryId].castleData;
        castleData.formerMonarchList.Add(castleData.monarchId);
        castleData.monarchId = _data.tokenId;
        castleData.hasMonarch = true;

        responceCandidateData(_data, true);
    }

    public void returnCandidateData(CandidateData _data)
    {
        int round = _data.round;
        List<CandidateData> list = candidateListArchive[round];
        for (int i = 0; i < list.Count; i++)
        {
            CandidateData cData = list[i];
            if (cData.countryId == _data.countryId && cData.id == _data.id)
            {
                cData.nftReturned = true;
                break;
            }
        }

        _data.nftReturned = true;

        for (int i = 0; i < stakingListArchive.Count; i++)
        {
            CharacterData characterData = stakingListArchive[i];
            if (characterData.tokenId == _data.tokenId)
            {
                stakingListArchive.RemoveAt(i);
                characterListArchive.Add(characterData);

                stakingDataArchive.Remove(characterData.tokenId);
                break;
            }
        }

        responceCandidateData(_data);
    }

    private void responceCandidateData(CandidateData _data, bool _isAppointment = false)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["id"] = _data.id;
        dic["round"] = _data.round;
        dic["tokenId"] = _data.tokenId;
        dic["country"] = _data.countryId;
        dic["address"] = _data.address;
        dic["nickname"] = _data.nickname;
        dic["title"] = _data.title;
        dic["contents"] = _data.contents;
        dic["url"] = _data.url;
        dic["canceled"] = _data.canceled;
        dic["votingCount"] = _data.votingCount;
        dic["nftReturned"] = _data.nftReturned;
        dic["registBlock"] = _data.registBlock;

        var value = JsonConvert.SerializeObject(dic);
        mContractManager.resUpdateCandidateData(value);
        if (_isAppointment)
        {
            mContractManager.resAppointmentMonarch(value);
        }
    }

    public void reqNotVotedCharacterList(int _round, int[] _list)
    {
        List<int> notVotedCharacterIdList = new List<int>();
        foreach(int id in _list)
        {
            if (!votingDataListArchive[_round].Contains(id))
            {
                notVotedCharacterIdList.Add(id);
            }
        }

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["characterIdList"] = notVotedCharacterIdList.ToArray();
        var value = JsonConvert.SerializeObject(dic);
        ContractManager.instance.resNotVotedCharacterList(value);
    }

    public void reqVoteMonarchElection(int _round, int[] _candidateIds, int[] _voteCounts, int[] _idList)
    {
        foreach (int id in _idList)
        {
            votingDataListArchive[_round].Add(id);
        }

        for (int cid = 0; cid < _candidateIds.Length; cid++)
        {
            if (_candidateIds[cid] == -1)
            {
                continue;
            }

            foreach(CandidateData candidateData in candidateListArchive[_round])
            {
                if (candidateData.countryId == cid && candidateData.id == _candidateIds[cid])
                {
                    candidateData.votingCount += _voteCounts[cid];
                    totalVotingCountListArchive[_round][cid] += _voteCounts[cid];
                    break;
                }
            }
        }

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["characterIdList"] = _idList;
        var value = JsonConvert.SerializeObject(dic);
        ContractManager.instance.resVoteMonarchElection(value);
    }

    public void reqConstantValues()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["startBlock"] = Const.START_BLOCK;
        dic["electionStartBlock"] = Const.ELECTION_START_BLOCK;
        dic["subscribeFee"] = Const.SUBSCRIBE_FEE;
        dic["monarchRegistFee"] = Const.MONARCH_REGIST_FEE;
        dic["miningTaxSettlingDelay"] = Const.MINING_TAX_SETTLING_DELAY;

        var value = JsonConvert.SerializeObject(dic);
        mContractManager.resConstantValues(value);
    }
}
