using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using Coffee.UIExtensions;

public class CharacterCardController : MonoBehaviour, MiningDataObserever
{
    public const string BACKGROUND_IMAGE_URL = "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/background/background.png";
    public const string MIRROR_IMAGE_URL = "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/frame/frame.png";

    public const int STATE_PUB = 1;
    public const int STATE_WAITING_ROOM = 2;
    public const int STATE_WORKING_PLACE = 3;

    [SerializeField]
    GameObject componentsGroup;

    [SerializeField]
    Image backgroundImage;
    [SerializeField]
    Image mirrorImage;
    [SerializeField]
    Image flagImage;
    [SerializeField]
    Image avatarImage;
    [SerializeField]
    Image[] gemImageList;
    [SerializeField]
    GameObject[] gemParticleList;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text stateText;

    [SerializeField]
    GameObject selectFrame;

    [SerializeField]
    GameObject loadingObject;
    [SerializeField]
    GameObject workingObject;

    CharacterData characterData;
    bool selected = false;
    int cardState = 0;
    Func<CharacterCardController, bool> clickCallback;

    private void OnDestroy()
    {
        removeListener();
    }

    private void OnDisable()
    {
        removeListener();
    }

    public void setEnableComponents(bool _set)
    {
        componentsGroup.SetActive(_set);
    }

    public void setClickCallback(Func<CharacterCardController, bool> _callback)
    {
        clickCallback = _callback;
    }

    private void removeListener()
    {
        if (characterData != null)
        {
            MiningManager.instance.removeMiningDataObserver(characterData.tokenId, this);
        }
    }

    public void setCharacterId(CharacterData _data, int _state)
    {
        removeListener();

        if (_state == STATE_WORKING_PLACE)
        {
            MiningManager.instance.addMiningDataObserver(_data.tokenId, this);
        }

        characterData = _data;
        cardState = _state;

        nameText.text = _data.name;
        stateText.text = "";
        updateStateText();

        loadingObject.SetActive(true);
        setSelected(false);

        updateCharacterImage();
    }

    private void updateStateText()
    {
        switch (cardState)
        {
            case STATE_PUB:
                stateText.text = StakingManager.instance.getStakingText(characterData.stakingData.purpose);
                break;
            case STATE_WORKING_PLACE:
                workingObject.SetActive(true);
                break;
            case STATE_WAITING_ROOM:
            default:
                break;
        }
    }

    public void setAllParticleEnabled(bool _set)
    {
        if (_set)
        {
            updateGemImageAndParticle();
            selectFrame.SetActive(selected);
        } else
        {
            for (int i = 0; i < gemParticleList.Length; i++)
            {
                gemParticleList[i].SetActive(false);
            }
            selectFrame.SetActive(false);
        }
    }

    private bool updateCharacterImage()
    {
        AssetsLoadManager.instance.requestSprite(BACKGROUND_IMAGE_URL, (_sprite) =>
        {
            backgroundImage.sprite = _sprite;
            backgroundImage.enabled = true;
            return true;
        }, null);
        AssetsLoadManager.instance.requestSprite(MIRROR_IMAGE_URL, (_sprite) =>
        {
            mirrorImage.sprite = _sprite;
            mirrorImage.enabled = true;
            loadingObject.SetActive(false);
            return true;
        }, null);
        flagImage.sprite = CountryManager.instance.getFlagImage(characterData.country);
        avatarImage.sprite = CharacterManager.instance.getAvatarImage(characterData.job);
        flagImage.enabled = true;
        avatarImage.enabled = true;

        updateGemImageAndParticle();

        return true;
    }

    private void updateGemImageAndParticle()
    {
        if (characterData == null)
        {
            return;
        }

        EquipItemData[] equipItemDataList = new EquipItemData[]
        {
            ItemManager.instance.getEquipItem(characterData.equipData.head),
            ItemManager.instance.getEquipItem(characterData.equipData.weapon),
            ItemManager.instance.getEquipItem(characterData.equipData.accessory),
            ItemManager.instance.getEquipItem(characterData.equipData.armor),
            ItemManager.instance.getEquipItem(characterData.equipData.pants),
            ItemManager.instance.getEquipItem(characterData.equipData.shoes),
        };

        for (int i = 0; i < equipItemDataList.Length; i++)
        {
            EquipItemData ed = equipItemDataList[i];
            if (ed == null)
            {
                gemImageList[i].enabled = false;
                gemParticleList[i].SetActive(false);
            }
            else
            {
                gemImageList[i].enabled = true;
                gemImageList[i].sprite = ItemManager.instance.getGemImage(ed.grade);
                gemParticleList[i].SetActive(ed.grade == ItemGrade.LEGEND);
            }
        }
    }

    public void setSelected(bool _set)
    {
        selected = _set;
        selectFrame.SetActive(_set);
    }

    public bool isSelected()
    {
        return selected;
    }

    public CharacterData getCharacterData()
    {
        return characterData;
    }

    public void onClick()
    {
        clickCallback?.Invoke(this);
    }

    public void onMiningDataChaged(MiningData _data)
    {
        BigInteger totalAmount = 0;
        for (int i = 0; i < MiningManager.IDX_MAX; i++)
        {
            if (i == MiningManager.IDX_COMMISSION || i == MiningManager.IDX_FINAL)
            {
                continue;
            }
            totalAmount += _data.amount[i];
        }

        stateText.text = Utils.convertPebToTorStr(totalAmount) + " " + Const.TOR_COIN; // display total amount not final
    }
}