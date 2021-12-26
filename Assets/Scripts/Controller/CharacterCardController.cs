using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class CharacterCardController : MonoBehaviour, MiningDataObserever
{
    public const int STATE_PUB = 1;
    public const int STATE_WAITING_ROOM = 2;
    public const int STATE_WORKING_PLACE = 3;

    [SerializeField]
    Image characterImage;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text stateText;

    [SerializeField]
    Image selectFrameImage;

    [SerializeField]
    GameObject loadingObject;
    [SerializeField]
    GameObject workingObject;

    CharacterData characterData;
    bool selected = false;
    int cardState = 0;
    Func<CharacterCardController, bool> clickCallback;

    // TEMP
    [SerializeField]
    Sprite characterSprite;

    private void OnDestroy()
    {
        if (characterData != null)
        {
            MiningManager.instance.removeMiningDataObserver(characterData.tokenId, this);
        }
    }

    public void setClickCallback(Func<CharacterCardController, bool> _callback)
    {
        clickCallback = _callback;
    }

    public void setCharacterId(CharacterData _data, int _state)
    {
        characterData = _data;
        cardState = _state;

        nameText.text = _data.name;
        updateStateText();

        characterImage.enabled = false;
        loadingObject.SetActive(true);
        setSelected(false);

        updateCharacterImage(characterSprite);

        if (_state == STATE_WORKING_PLACE)
        {
            MiningManager.instance.addMiningDataObserver(characterData.tokenId, this);
        }
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

    private bool updateCharacterImage(Sprite _sprite)
    {
        loadingObject.SetActive(false);
        characterImage.sprite = _sprite;
        characterImage.enabled = true;

        return true;
    }

    public void setSelected(bool _set)
    {
        selected = _set;
        selectFrameImage.enabled = _set;
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