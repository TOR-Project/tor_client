using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class CharacterCardController : MonoBehaviour
{
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

    private CharacterData characterData;
    private bool isSelected = false;

    public void setCharacterId(CharacterData _data)
    {
        characterData = _data;

        nameText.text = _data.name;
        stateText.text = LanguageManager.instance.getText(StakingManager.instance.getStakingText(_data.stakingData.purpose));

        characterImage.enabled = false;
        loadingObject.SetActive(true);

        AssetsLoadManager.instance.requestSprite(_data.image, updateCharacterImage);
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
        isSelected = _set;
        selectFrameImage.enabled = _set;
    }
}